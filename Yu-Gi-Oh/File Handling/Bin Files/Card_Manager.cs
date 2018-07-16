using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Utility;
using Yu_Gi_Oh.File_Handling.ZIB_Files;

namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    /// <summary>
    ///     This file handles a lot of stuff centered around managing card functions.
    /// </summary>
    public class Card_Manager
    {
        private readonly Dictionary<Localized_Text.Language, Dictionary<string, Card_Info>> _cardsByName;

        /// <summary>
        ///     Default Constructor.
        /// </summary>
        /// <param name="manager">A new instance of the Manager class.</param>
        public Card_Manager(Manager manager)
        {
            Manager = manager;
            Cards = new Dictionary<short, Card_Info>();
            CardsByIndex = new List<Card_Info>();
            Tags = new List<Card_Tag_Info>();
            _cardsByName = new Dictionary<Localized_Text.Language, Dictionary<string, Card_Info>>();
            CardNameTypes = new Dictionary<CardNameType, HashSet<short>>();
        }

        /// <summary>
        ///     An internal representation of the Manager class as defined in the contructor.
        /// </summary>
        public Manager Manager { get; }

        /// <summary>
        ///     A list of cards with their ID and the info about the card.
        /// </summary>
        public Dictionary<short, Card_Info> Cards { get; }

        /// <summary>
        ///     A list of cards based on card info.
        /// </summary>
        public List<Card_Info> CardsByIndex { get; }

        /// <summary>
        ///     A list of cards by their type.
        /// </summary>
        public Dictionary<CardNameType, HashSet<short>> CardNameTypes { get; }

        /// <summary>
        ///     A list of card tags.
        /// </summary>
        public List<Card_Tag_Info> Tags { get; }

        /// <summary>
        ///     A function that should be used to find a card based on its name.
        /// </summary>
        /// <param name="language">What localization to use.</param>
        /// <param name="name">The card name to search for</param>
        /// <returns>The Card_Info of the card if found.</returns>
        public Card_Info FindCardByName(Localized_Text.Language language, string name)
        {
            _cardsByName[language].TryGetValue(name, out var cardInfo);
            return cardInfo;
        }

        /// <summary>
        ///     Load all cards into memory.
        /// </summary>
        public void Load()
        {
            Cards.Clear();
            CardsByIndex.Clear();
            _cardsByName.Clear();
            Tags.Clear();

            var archive = Manager.Archive;

            var indx = archive.LoadLocalizedBuffer("CARD_Indx_", true);
            var names = archive.LoadLocalizedBuffer("CARD_Name_", true);
            var descriptions = archive.LoadLocalizedBuffer("CARD_Desc_", true);
            var taginfos = archive.LoadLocalizedBuffer("taginfo_", true);

            var allCardImages = new List<ZIB_File>();
            allCardImages.AddRange(archive.Root.FindFile("cardcropHD400.jpg.zib").LoadData<ZIB_Data>().Files.Values);
            allCardImages.AddRange(archive.Root.FindFile("cardcropHD401.jpg.zib").LoadData<ZIB_Data>().Files.Values);

            var cardImagesById = new Dictionary<short, ZIB_File>();
            foreach (var file in allCardImages)
            {
                var cardId = short.Parse(file.FileName.Substring(0, file.FileName.IndexOf('.')));
                cardImagesById.Add(cardId, file);
            }

            var cards = new List<Card_Info>();
            foreach (Localized_Text.Language language in Enum.GetValues(typeof(Localized_Text.Language)))
                if (language != Localized_Text.Language.Unknown)
                {
                    LoadCardNamesAndDescriptions(language, cards, indx, names, descriptions);

                    var languageCardsByName = new Dictionary<string, Card_Info>();
                    _cardsByName.Add(language, languageCardsByName);
                    foreach (var card in cards)
                        languageCardsByName[card.Name.GetText(language)] = card;
                }

            CardsByIndex.AddRange(cards);

            LoadCardProps(cards, Cards, cardImagesById);

            ProcessLimitedCardList(Cards);
            LoadCardGenre(cards);
            LoadRelatedCards(cards, Cards, Tags, taginfos);
            LoadCardNameTypes(Cards, CardNameTypes);
        }

        private void LoadCardNameTypes(Dictionary<short, Card_Info> cards,
            IDictionary<CardNameType, HashSet<short>> cardNameTypes)
        {
            using (var reader =
                new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/CARD_Named.bin").LoadBuffer())))
            {
                var numArchetypes = reader.ReadUInt16();
                var numCards = reader.ReadUInt16();

                long cardsStartOffset = 4 + numArchetypes * 4;
                var cardsEndOffset = cardsStartOffset + numCards * 2;
                Debug.Assert(reader.BaseStream.Length == cardsEndOffset);

                for (var i = 0; i < numArchetypes; i++)
                {
                    int offset = reader.ReadInt16();
                    int count = reader.ReadInt16();
                    var cardIds = new HashSet<short>();
                    cardNameTypes.Add((CardNameType) i, cardIds);

                    var tempOffset = reader.BaseStream.Position;
                    reader.BaseStream.Position = cardsStartOffset + offset * 2;
                    for (var j = 0; j < count; j++)
                    {
                        var cardId = reader.ReadInt16();
                        Cards[cardId].NameTypes.Add((CardNameType) i);
                        cardIds.Add(cardId);
                    }

                    reader.BaseStream.Position = tempOffset;
                }
            }
        }

        private void LoadCardGenre(IEnumerable<Card_Info> cards)
        {
            using (var reader =
                new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/CARD_Genre.bin").LoadBuffer())))
            {
                foreach (var card in cards) card.Genre = (CardGenre) reader.ReadUInt64();
            }
        }

        private void LoadCardProps(IEnumerable<Card_Info> cards, IDictionary<short, Card_Info> cardsById,
            IReadOnlyDictionary<short, ZIB_File> cardImagesById)
        {
            using (var reader =
                new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/CARD_Prop.bin").LoadBuffer())))
            {
                foreach (var card in cards)
                {
                    LoadCardProp(card, cardsById, reader.ReadUInt32(), reader.ReadUInt32());
                    if (card.CardId > 0) card.ImageFile = cardImagesById[card.CardId];
                }
            }
        }

        private static void LoadCardProp(Card_Info card, IDictionary<short, Card_Info> CardsById, uint A1, uint A2)
        {
            var First = (A1 << 18) | (((A1 & 0x7FC000) | (A1 >> 18)) >> 5);

            var Second = (((A2 & 1u) | (A2 << 21)) & 0x80000001) |
                         (((A2 & 0x7800) | (((A2 & 0x780) | ((A2 & 0x7E) << 10)) << 8)) << 6) |
                         (((A2 & 0x38000) |
                           (((A2 & 0x7C0000) | (((A2 & 0x7800000) | ((A2 >> 8) & 0x780000)) >> 9)) >> 8)) >> 1);

            var CardId = (short) ((First >> 18) & 0x3FFF);
            var Atk = (First >> 9) & 0x1FF;
            var Def = First & 0x1FF;
            var CardType = (CardType) ((Second >> 25) & 0x3F);
            var Attribute = (CardAttribute) ((Second >> 21) & 0xF);
            var Level = (Second >> 17) & 0xF;
            var SpellType = (SpellType) ((Second >> 14) & 7);
            var MonsterType = (MonsterType) ((Second >> 9) & 0x1F);
            var PendulumScale1 = (Second >> 1) & 0xF;
            var PendulumScale2 = (Second >> 5) & 0xF;

            card.CardId = CardId;
            card.Atk = (int) (Atk * 10);
            card.Def = (int) (Def * 10);
            card.Level = (byte) Level;
            card.Attribute = Attribute;
            card.CardType = CardType;
            card.SpellType = SpellType;
            card.MonsterType = MonsterType;
            card.PendulumScale1 = (byte) PendulumScale1;
            card.PendulumScale2 = (byte) PendulumScale2;

            CardsById.Add(CardId, card);

            Debug.Assert(CardId < Constants.MaxCardId + 1);

            if (!Enum.IsDefined(typeof(MonsterType), MonsterType) || !Enum.IsDefined(typeof(SpellType), SpellType) ||
                !Enum.IsDefined(typeof(CardType), CardType) || !Enum.IsDefined(typeof(CardAttribute), Attribute))
                Debug.Assert(false);
        }

        private static void LoadCardNamesAndDescriptions(Localized_Text.Language language, IList<Card_Info> cards,
            IReadOnlyDictionary<Localized_Text.Language, byte[]> indxByLanguage,
            IReadOnlyDictionary<Localized_Text.Language, byte[]> namesByLanguage,
            IReadOnlyDictionary<Localized_Text.Language, byte[]> descriptionsByLanguage)
        {
            if (language == Localized_Text.Language.Unknown) return;

            var indx = indxByLanguage[language];
            var names = namesByLanguage[language];
            var descriptions = descriptionsByLanguage[language];

            using (var indxReader = new BinaryReader(new MemoryStream(indx)))
            using (var namesReader = new BinaryReader(new MemoryStream(names)))
            using (var descriptionsReader = new BinaryReader(new MemoryStream(descriptions)))
            {
                var namesByOffset = ReadStrings(namesReader);
                var descriptionsByOffset = ReadStrings(descriptionsReader);

                var index = 0;
                while (true)
                {
                    var nameOffset = indxReader.ReadUInt32();
                    var descriptionOffset = indxReader.ReadUInt32();

                    if (indxReader.BaseStream.Position >= indxReader.BaseStream.Length) break;

                    Card_Info card = null;
                    if (cards.Count > index)
                        card = cards[index];
                    else
                        cards.Add(card = new Card_Info(index));

                    card.Name.SetText(language, namesByOffset[nameOffset]);
                    card.Description.SetText(language, descriptionsByOffset[descriptionOffset]);

                    index++;
                }
            }
        }

        private static Dictionary<uint, string> ReadStrings(BinaryReader reader)
        {
            var result = new Dictionary<uint, string>();
            while (reader.BaseStream.Position < reader.BaseStream.Length)
            {
                var offset = (uint) reader.BaseStream.Position;
                var name = reader.ReadNullTerminatedString(Encoding.Unicode);
                result.Add(offset, name);
            }

            return result;
        }

        private void LoadRelatedCards(IReadOnlyList<Card_Info> cards,
            IReadOnlyDictionary<short, Card_Info> cardsByCardId, IList<Card_Tag_Info> tags,
            IReadOnlyDictionary<Localized_Text.Language, byte[]> taginfos)
        {
            foreach (Localized_Text.Language language in Enum.GetValues(typeof(Localized_Text.Language)))
            {
                if (language == Localized_Text.Language.Unknown) continue;

                using (var reader = new BinaryReader(new MemoryStream(taginfos[language])))
                {
                    var count = reader.ReadInt32();
                    for (var i = 0; i < count; i++)
                    {
                        Card_Tag_Info tagInfo = null;
                        if (i >= tags.Count)
                        {
                            tagInfo = new Card_Tag_Info();
                            tags.Add(tagInfo);
                        }
                        else
                        {
                            tagInfo = tags[i];
                        }

                        tagInfo.Index = i;
                        tagInfo.MainType = (Card_Tag_Info.Type) reader.ReadInt16();
                        tagInfo.MainValue = reader.ReadInt16();
                        for (var j = 0; j < tagInfo.Elements.Length; j++)
                        {
                            tagInfo.Elements[j].Type = (Card_Tag_Info.ElementType) reader.ReadInt16();
                            tagInfo.Elements[j].Value = reader.ReadInt16();
                        }

                        var stringOffset1 = reader.ReadInt64();
                        var stringOffset2 = reader.ReadInt64();

                        var tempOffset = reader.BaseStream.Position;

                        reader.BaseStream.Position = stringOffset1;
                        tagInfo.Text.SetText(language, reader.ReadNullTerminatedString(Encoding.Unicode));

                        reader.BaseStream.Position = stringOffset2;
                        tagInfo.DisplayText.SetText(language, reader.ReadNullTerminatedString(Encoding.Unicode));

                        reader.BaseStream.Position = tempOffset;
                    }
                }
            }

            using (var reader =
                new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/tagdata.bin").LoadBuffer())))
            {
                var dataStart = reader.BaseStream.Position + cards.Count * 8;

                for (var i = 0; i < cards.Count; i++)
                {
                    var shortoffset = reader.ReadUInt32();
                    var tagCount = reader.ReadUInt32();

                    var tempOffset = reader.BaseStream.Position;

                    var start = dataStart + shortoffset * 4;
                    reader.BaseStream.Position = start;
                    if (tagCount > 0 && i >= 0)
                    {
                        var card = cards[i];
                        card.RelatedCards.Clear();
                        for (var j = 0; j < tagCount; j++)
                            card.RelatedCards.Add(new RelatedCardInfo(cardsByCardId[reader.ReadInt16()],
                                Tags[reader.ReadInt16()]));
                    }

                    reader.BaseStream.Position = tempOffset;
                }
            }

            var knownMainTagTypes = (Card_Tag_Info.Type[]) Enum.GetValues(typeof(Card_Tag_Info.Type));
            var knownElementTagTypes = (Card_Tag_Info.ElementType[]) Enum.GetValues(typeof(Card_Tag_Info.ElementType));
            foreach (var tag in tags)
            {
                Debug.Assert(knownMainTagTypes.Contains(tag.MainType));
                Debug.Assert(tag.MainValue <= 1);

                foreach (var element in tag.Elements)
                {
                    if (element.Type == Card_Tag_Info.ElementType.None) continue;

                    Debug.Assert(knownElementTagTypes.Contains(element.Type));
                }

                if (tag.MainType == Card_Tag_Info.Type.Exact)
                {
                    // Need english here
                    var language = Localized_Text.Language.English;
                    var displayText = tag.DisplayText.GetText(language);
                    var text = tag.Text.GetText(Localized_Text.Language.English);
                    var splitIndex = displayText == null ? -1 : displayText.IndexOf(':');
                    if (splitIndex >= 0)
                    {
                        var typeStr = displayText.Substring(0, splitIndex).Trim();
                        var value = displayText.Substring(splitIndex + 1).Trim();
                        switch (typeStr.ToLower())
                        {
                            case "related to":
                                tag.Exact = Card_Tag_Info.ExactType.RelatedTo;
                                tag.ExactCard = FindCardByName(language, value);
                                break;
                            case "card effect":
                                tag.Exact = Card_Tag_Info.ExactType.CardEffect;
                                break;
                            case "ritual monster":
                                tag.Exact = Card_Tag_Info.ExactType.RitualMonster;
                                tag.ExactCard = FindCardByName(language, value);
                                break;
                            case "fusion monster":
                                tag.Exact = Card_Tag_Info.ExactType.FusionMonster;
                                tag.ExactCard = FindCardByName(language, value);
                                break;
                            case "spell and trap":
                                tag.Exact = Card_Tag_Info.ExactType.SpellTrap;
                                break;
                            case "works well with":
                                tag.Exact = Card_Tag_Info.ExactType.WorksWellWith;
                                tag.ExactCard = FindCardByName(language, value);
                                break;
                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }
                    else
                    {
                        switch (text.ToLower())
                        {
                            case "banishbeast":
                                tag.Exact = Card_Tag_Info.ExactType.BanishBeast;
                                break;
                            case "banishdark":
                                tag.Exact = Card_Tag_Info.ExactType.BanishDark;
                                break;
                            case "banishfish":
                                tag.Exact = Card_Tag_Info.ExactType.BanishFish;
                                break;
                            case "banishrock":
                                tag.Exact = Card_Tag_Info.ExactType.BanishRock;
                                break;
                            case "countertrapfairy":
                                tag.Exact = Card_Tag_Info.ExactType.CounterTrapFairy;
                                break;
                            case "spellcounter":
                                tag.Exact = Card_Tag_Info.ExactType.SpellCounter;
                                break;
                            default:
                                Debug.Assert(false);
                                break;
                        }
                    }

                    switch (tag.Exact)
                    {
                        case Card_Tag_Info.ExactType.CardEffect:
                            Card_Tag_Info.CardEffectType cardEffect;
                            if (!Enum.TryParse(text, true, out cardEffect)) Debug.Assert(false);
                            tag.CardEffect = cardEffect;
                            break;

                        case Card_Tag_Info.ExactType.SpellTrap:
                            Card_Tag_Info.CardEffectType spellEffect;
                            if (!Enum.TryParse("Spell_" + text, true, out spellEffect)) Debug.Assert(false);
                            tag.CardEffect = spellEffect;
                            break;
                    }
                }
            }

            foreach (var card in cards)
            foreach (var relatedCardInfo in card.RelatedCards)
                if (relatedCardInfo.TagInfo.CardEffect != Card_Tag_Info.CardEffectType.None)
                    card.CardEffectTags.Add(relatedCardInfo.TagInfo.CardEffect);
        }

        private void ProcessLimitedCardList(Dictionary<short, Card_Info> cardsById)
        {
            foreach (var card in cardsById.Values) card.Limit = CardLimitation.NotLimited;

            foreach (var cardId in Manager.CardLimits.Forbidden) cardsById[cardId].Limit = CardLimitation.Forbidden;

            foreach (var cardId in Manager.CardLimits.Limited) cardsById[cardId].Limit = CardLimitation.Limited;

            foreach (var cardId in Manager.CardLimits.SemiLimited) cardsById[cardId].Limit = CardLimitation.SemiLimited;
        }
    }
}