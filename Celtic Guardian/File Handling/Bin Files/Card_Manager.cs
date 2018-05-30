using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Celtic_Guardian.File_Handling.Miscellaneous_Files;
using Celtic_Guardian.File_Handling.Utility;
using Celtic_Guardian.File_Handling.ZIB_Files;

namespace Celtic_Guardian.File_Handling.Bin_Files
{
    public class Card_Manager
    {
        private readonly Dictionary<Localized_Text.Language, Dictionary<string, Card_Info>> _cardsByName;

        public Card_Manager(Manager manager)
        {
            Manager = manager;
            Cards = new Dictionary<short, Card_Info>();
            CardsByIndex = new List<Card_Info>();
            Tags = new List<Card_Tag_Info>();
            _cardsByName = new Dictionary<Localized_Text.Language, Dictionary<string, Card_Info>>();
            CardNameTypes = new Dictionary<CardNameType, HashSet<short>>();
        }

        public Manager Manager { get; }
        public Dictionary<short, Card_Info> Cards { get; }
        public List<Card_Info> CardsByIndex { get; }
        public Dictionary<CardNameType, HashSet<short>> CardNameTypes { get; }
        public List<Card_Tag_Info> Tags { get; }

        public Card_Info FindCardByName(Localized_Text.Language language, string name)
        {
            _cardsByName[language].TryGetValue(name, out var cardInfo);
            return cardInfo;
        }

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

        private void LoadCardNameTypes(Dictionary<short, Card_Info> cards, IDictionary<CardNameType, HashSet<short>> cardNameTypes)
        {
            using (var reader = new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/CARD_Named.bin").LoadBuffer())))
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
            using (var reader = new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/CARD_Genre.bin").LoadBuffer())))
            {
                foreach (var card in cards) card.Genre = (CardGenre) reader.ReadUInt64();
            }
        }

        private void LoadCardProps(IEnumerable<Card_Info> cards, IDictionary<short, Card_Info> cardsById, IReadOnlyDictionary<short, ZIB_File> cardImagesById)
        {
            using (var reader = new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/CARD_Prop.bin").LoadBuffer())))
            {
                foreach (var card in cards)
                {
                    LoadCardProp(card, cardsById, reader.ReadUInt32(), reader.ReadUInt32());
                    if (card.CardId > 0) card.ImageFile = cardImagesById[card.CardId];
                }
            }
        }

        private static void LoadCardProp(Card_Info card, IDictionary<short, Card_Info> cardsById, uint a1, uint a2)
        {
            var first = (a1 << 18) | (((a1 & 0x7FC000) | (a1 >> 18)) >> 5);

            var second = (((a2 & 1u) | (a2 << 21)) & 0x80000001) | (((a2 & 0x7800) | (((a2 & 0x780) | ((a2 & 0x7E) << 10)) << 8)) << 6) |
                         (((a2 & 0x38000) | (((a2 & 0x7C0000) | (((a2 & 0x7800000) | ((a2 >> 8) & 0x780000)) >> 9)) >> 8)) >> 1);

            var cardId = (short) ((first >> 18) & 0x3FFF);
            var atk = (first >> 9) & 0x1FF;
            var def = first & 0x1FF;
            var cardType = (CardType) ((second >> 25) & 0x3F);
            var attribute = (CardAttribute) ((second >> 21) & 0xF);
            var level = (second >> 17) & 0xF;
            var spellType = (SpellType) ((second >> 14) & 7);
            var monsterType = (MonsterType) ((second >> 9) & 0x1F);
            var pendulumScale1 = (second >> 1) & 0xF;
            var pendulumScale2 = (second >> 5) & 0xF;

            card.CardId = cardId;
            card.Atk = (int) (atk * 10);
            card.Def = (int) (def * 10);
            card.Level = (byte) level;
            card.Attribute = attribute;
            card.CardType = cardType;
            card.SpellType = spellType;
            card.MonsterType = monsterType;
            card.PendulumScale1 = (byte) pendulumScale1;
            card.PendulumScale2 = (byte) pendulumScale2;

            cardsById.Add(cardId, card);

            Debug.Assert(cardId < Constants.MaxCardId + 1);

            if (!Enum.IsDefined(typeof(MonsterType), monsterType) ||
                !Enum.IsDefined(typeof(SpellType), spellType) ||
                !Enum.IsDefined(typeof(CardType), cardType) ||
                !Enum.IsDefined(typeof(CardAttribute), attribute))
                Debug.Assert(false);
        }

        private static void LoadCardNamesAndDescriptions(Localized_Text.Language language, IList<Card_Info> cards, IReadOnlyDictionary<Localized_Text.Language, byte[]> indxByLanguage, IReadOnlyDictionary<Localized_Text.Language, byte[]> namesByLanguage, IReadOnlyDictionary<Localized_Text.Language, byte[]> descriptionsByLanguage)
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

        private void LoadRelatedCards(IReadOnlyList<Card_Info> cards, IReadOnlyDictionary<short, Card_Info> cardsByCardId, IList<Card_Tag_Info> tags, IReadOnlyDictionary<Localized_Text.Language, byte[]> taginfos)
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

            using (var reader = new BinaryReader(new MemoryStream(Manager.Archive.Root.FindFile("bin/tagdata.bin").LoadBuffer())))
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
                        for (var j = 0; j < tagCount; j++) card.RelatedCards.Add(new RelatedCardInfo(cardsByCardId[reader.ReadInt16()], Tags[reader.ReadInt16()]));
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

        private void PrintLimitedCardList()
        {
            Debug.WriteLine("========================== Forbidden ==========================");
            foreach (var cardId in Manager.CardLimits.Forbidden) Debug.WriteLine(Cards[cardId].Name);

            Debug.WriteLine("========================== Limited ==========================");
            foreach (var cardId in Manager.CardLimits.Limited) Debug.WriteLine(Cards[cardId].Name);

            Debug.WriteLine("========================== Semi-limited ==========================");
            foreach (var cardId in Manager.CardLimits.SemiLimited) Debug.WriteLine(Cards[cardId].Name);
        }
    }

    public class Card_Info
    {
        public Card_Info(int index)
        {
            CardIndex = index;
            Name = new Localized_Text();
            Description = new Localized_Text();
            RelatedCards = new List<RelatedCardInfo>();
            CardEffectTags = new HashSet<Card_Tag_Info.CardEffectType>();
            NameTypes = new HashSet<CardNameType>();
            SetIds = new List<int>();
        }

        public int CardIndex { get; set; }
        public short CardId { get; set; }
        public ZIB_File ImageFile { get; set; }
        public Localized_Text Name { get; set; }
        public Localized_Text Description { get; set; }
        public List<RelatedCardInfo> RelatedCards { get; }
        public HashSet<Card_Tag_Info.CardEffectType> CardEffectTags { get; }
        public HashSet<CardNameType> NameTypes { get; set; }
        public List<int> SetIds { get; }
        public int Atk { get; set; }
        public int Def { get; set; }
        public byte Level { get; set; }
        public bool IsUnknownAtk => Atk == 5110;
        public bool IsUnknownDef => Def == 5110;
        public CardAttribute Attribute { get; set; }
        public CardType CardType { get; set; }
        public SpellType SpellType { get; set; }
        public MonsterType MonsterType { get; set; }
        public byte PendulumScale1 { get; set; }
        public byte PendulumScale2 { get; set; }
        public byte PendulumScale => Math.Max(PendulumScale1, PendulumScale2);
        public CardLimitation Limit { get; set; }
        public CardGenre Genre { get; set; }
        public CardTypeFlags CardTypeFlags => GetCardTypeFlags(CardType);
        public bool IsMonsterToken => IsMonster && CardTypeFlags.HasFlag(CardTypeFlags.Token);
        public bool IsEffect => CardTypeFlags.HasFlag(CardTypeFlags.Effect);
        public bool IsMonster => Attribute != CardAttribute.Spell && Attribute != CardAttribute.Trap;
        public bool IsNormalMonster => FrameType == CardFrameType.Normal || FrameType == CardFrameType.PendulumNormal;
        public bool IsPendulum => CardTypeFlags.HasFlag(CardTypeFlags.Pendulum);
        public bool IsXyz => CardTypeFlags.HasFlag(CardTypeFlags.Xyz);
        public bool IsSynchro => CardTypeFlags.HasFlag(CardTypeFlags.Synchro);
        public bool IsFusion => CardTypeFlags.HasFlag(CardTypeFlags.Fusion);
        public bool IsMainDeckCard => !IsExtraDeckCard;
        public bool IsExtraDeckCard => CardTypeFlags.HasFlag(CardTypeFlags.Xyz) || CardTypeFlags.HasFlag(CardTypeFlags.Fusion) || CardTypeFlags.HasFlag(CardTypeFlags.Synchro);
        public bool IsSpell => Attribute == CardAttribute.Spell;
        public bool IsTrap => Attribute == CardAttribute.Trap;
        public string FrameName => GetFrameName(FrameType);

        public CardFrameType FrameType
        {
            get
            {
                if (IsSpell)
                    return CardFrameType.Spell;

                if (IsTrap)
                    return CardFrameType.Trap;

                var cardFlags = CardTypeFlags;

                if (cardFlags.HasFlag(CardTypeFlags.Synchro))
                    return cardFlags.HasFlag(CardTypeFlags.Pendulum) ? CardFrameType.PendulumSynchro : CardFrameType.Synchro;

                if (cardFlags.HasFlag(CardTypeFlags.Xyz))
                    return cardFlags.HasFlag(CardTypeFlags.Pendulum) ? CardFrameType.PendulumXyz : CardFrameType.Xyz;

                if (cardFlags.HasFlag(CardTypeFlags.Pendulum))
                    return cardFlags.HasFlag(CardTypeFlags.Effect) ? CardFrameType.PendulumEffect : CardFrameType.PendulumNormal;

                if (cardFlags.HasFlag(CardTypeFlags.Token))
                    return CardFrameType.Token;

                if (cardFlags.HasFlag(CardTypeFlags.Fusion))
                    return CardFrameType.Fusion;

                if (cardFlags.HasFlag(CardTypeFlags.Ritual))
                    return CardFrameType.Ritual;

                if (cardFlags.HasFlag(CardTypeFlags.Effect) || cardFlags.HasFlag(CardTypeFlags.SpecialSummon) || cardFlags.HasFlag(CardTypeFlags.Union) || cardFlags.HasFlag(CardTypeFlags.Toon) || cardFlags.HasFlag(CardTypeFlags.Gemini))
                    return CardFrameType.Effect;

                return CardFrameType.Normal;
            }
        }

        public string GetDescription(Localized_Text.Language language, bool pendulumDescription)
        {
            if (pendulumDescription && !IsPendulum)
                return string.Empty;

            var text = Description.GetText(language);
            if (!IsPendulum)
                return text;

            const string pendulumHeader = "[Pendulum Effect]";
            var index = text.IndexOf(pendulumHeader, StringComparison.Ordinal);
            if (pendulumDescription)
                return index == -1 ? string.Empty : text.Substring(index + pendulumHeader.Length);

            return index == -1 ? text : text.Substring(0, index);
        }

        public static CardTypeFlags GetCardTypeFlags(CardType cardType)
        {
            switch (cardType)
            {
                case CardType.Default: return CardTypeFlags.Default;
                case CardType.Effect: return CardTypeFlags.Effect;
                case CardType.Fusion: return CardTypeFlags.Fusion;
                case CardType.FusionEffect: return CardTypeFlags.Fusion | CardTypeFlags.Effect;
                case CardType.Ritual: return CardTypeFlags.Ritual;
                case CardType.RitualEffect: return CardTypeFlags.Ritual | CardTypeFlags.Effect;
                case CardType.ToonEffect: return CardTypeFlags.Toon | CardTypeFlags.Effect;
                case CardType.SpiritEffect: return CardTypeFlags.Spirit | CardTypeFlags.Effect;
                case CardType.UnionEffect: return CardTypeFlags.Union | CardTypeFlags.Effect;
                case CardType.GeminiEffect: return CardTypeFlags.Gemini | CardTypeFlags.Effect;
                case CardType.Token: return CardTypeFlags.Token;
                case CardType.Spell: return CardTypeFlags.Spell;
                case CardType.Trap: return CardTypeFlags.Trap;
                case CardType.Tuner: return CardTypeFlags.Tuner;
                case CardType.TunerEffect: return CardTypeFlags.Tuner | CardTypeFlags.Effect;
                case CardType.Synchro: return CardTypeFlags.Synchro;
                case CardType.SynchroEffect: return CardTypeFlags.Synchro | CardTypeFlags.Effect;
                case CardType.SynchroTunerEffect: return CardTypeFlags.Synchro | CardTypeFlags.Tuner | CardTypeFlags.Effect;
                case CardType.DarkTunerEffect: return CardTypeFlags.DarkTuner | CardTypeFlags.Effect;
                case CardType.DarkSynchroEffect: return CardTypeFlags.DarkSynchro | CardTypeFlags.Effect;
                case CardType.Xyz: return CardTypeFlags.Xyz;
                case CardType.XyzEffect: return CardTypeFlags.Xyz | CardTypeFlags.Effect;
                case CardType.FlipEffect: return CardTypeFlags.Flip | CardTypeFlags.Effect;
                case CardType.Pendulum: return CardTypeFlags.Pendulum;
                case CardType.PendulumEffect: return CardTypeFlags.Pendulum | CardTypeFlags.Effect;
                case CardType.EffectSp: return CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.ToonEffectSp: return CardTypeFlags.Toon | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.SpiritEffectSp: return CardTypeFlags.Spirit | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.TunerEffectSp: return CardTypeFlags.Tuner | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.DarkTunerEffectSp: return CardTypeFlags.DarkTuner | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.FlipTunerEffect: return CardTypeFlags.Flip | CardTypeFlags.Tuner | CardTypeFlags.Effect;
                case CardType.PendulumTunerEffect: return CardTypeFlags.Pendulum | CardTypeFlags.Tuner | CardTypeFlags.Effect;
                case CardType.XyzPendulumEffect: return CardTypeFlags.Xyz | CardTypeFlags.Pendulum | CardTypeFlags.Effect;
                case CardType.PendulumFlipEffect: return CardTypeFlags.Pendulum | CardTypeFlags.Flip | CardTypeFlags.Effect;
                case CardType.AnyNormal: return CardTypeFlags.Any | CardTypeFlags.Normal;
                case CardType.AnyFusion: return CardTypeFlags.Any | CardTypeFlags.Fusion;
                case CardType.AnyFlip: return CardTypeFlags.Any | CardTypeFlags.Flip;
                case CardType.AnyPendulum: return CardTypeFlags.Any | CardTypeFlags.Pendulum;
                case CardType.AnyRitual: return CardTypeFlags.Any | CardTypeFlags.Ritual;
                case CardType.AnySynchro: return CardTypeFlags.Any | CardTypeFlags.Synchro;
                case CardType.AnyTuner: return CardTypeFlags.Any | CardTypeFlags.Tuner;
                case CardType.AnyXyz: return CardTypeFlags.Any | CardTypeFlags.Xyz;
                default:
                    throw new NotSupportedException("CardType Not Valid! CHECK CODE PLEASE!");
            }
        }

        public static string GetFrameName(CardFrameType frameType)
        {
            switch (frameType)
            {
                case CardFrameType.Normal: return "card_nomal";
                case CardFrameType.Effect: return "card_kouka";
                case CardFrameType.Token: return "card_token";
                case CardFrameType.Ritual: return "card_gisiki";
                case CardFrameType.Fusion: return "card_yugo";
                case CardFrameType.PendulumEffect: return "card_pendulum";
                case CardFrameType.PendulumNormal: return "card_pendulum_n";
                case CardFrameType.PendulumSynchro: return "card_sync_pendulum";
                case CardFrameType.PendulumXyz: return "card_xyz_pendulum";
                case CardFrameType.Synchro: return "card_sync";
                case CardFrameType.Xyz: return "card_xyz";
                case CardFrameType.Spell: return "card_mahou";
                case CardFrameType.Trap: return "card_wana";
                default: return "card_normal";
            }
        }

        public static string GetFullMonsterTypeName(MonsterType monsterType, CardTypeFlags cardType)
        {
            string result = null;
            foreach (CardTypeFlags flag in Enum.GetValues(typeof(CardTypeFlags)))
            {
                if (!cardType.HasFlag(flag)) continue;

                var flagName = GetCardTypeFlagName(flag);
                if (!string.IsNullOrEmpty(flagName)) result = result == null ? flagName : flagName + "/" + result;
            }

            return "[" + GetMonsterTypeName(monsterType) + (result == null ? string.Empty : "/" + result) + "]";
        }

        public static string GetMonsterTypeName(MonsterType monsterType)
        {
            switch (monsterType)
            {
                case MonsterType.Dragon: return "Dragon";
                case MonsterType.Zombie: return "Zombie";
                case MonsterType.Fiend: return "Fiend";
                case MonsterType.Pyro: return "Pyro";
                case MonsterType.SeaSerpent: return "Sea Serpent";
                case MonsterType.Rock: return "Rock";
                case MonsterType.Machine: return "Machine";
                case MonsterType.Fish: return "Fish";
                case MonsterType.Dinosaur: return "Dinosaur";
                case MonsterType.Insect: return "Insect";
                case MonsterType.Beast: return "Beast";
                case MonsterType.BeastWarrior: return "Beast-Warrior";
                case MonsterType.Plant: return "Plant";
                case MonsterType.Aqua: return "Aqua";
                case MonsterType.Warrior: return "Warrior";
                case MonsterType.WingedBeast: return "Winged Beast";
                case MonsterType.Fairy: return "Fairy";
                case MonsterType.Spellcaster: return "Spellcaster";
                case MonsterType.Thunder: return "Thunder";
                case MonsterType.Reptile: return "Reptile";
                case MonsterType.Psychic: return "Psychic";
                case MonsterType.Wyrm: return "Wyrm";
                case MonsterType.DivineBeast: return "Divine-Beast";
                case MonsterType.CreatorGod: return "Creator";
                case MonsterType.Spell: return "Spell";
                case MonsterType.Trap: return "Trap";
                case MonsterType.Unknown:
                default:
                    return "?";
            }
        }

        public static string GetCardTypeFlagName(CardTypeFlags flag)
        {
            switch (flag)
            {
                default:
                case CardTypeFlags.Default: return null;
                case CardTypeFlags.Effect: return "Effect";
                case CardTypeFlags.Fusion: return "Fusion";
                case CardTypeFlags.Ritual: return "Ritual";
                case CardTypeFlags.Toon: return "Toon";
                case CardTypeFlags.Spirit: return "Spirit";
                case CardTypeFlags.Union: return "Union";
                case CardTypeFlags.Gemini: return "Gemini";
                case CardTypeFlags.Token: return "Token";
                case CardTypeFlags.Spell: return "Spell";
                case CardTypeFlags.Trap: return "Trap";
                case CardTypeFlags.Tuner: return "Tuner";
                case CardTypeFlags.DarkTuner: return "Dark Tuner";
                case CardTypeFlags.DarkSynchro: return "Dark Synchro";
                case CardTypeFlags.Synchro: return "Synchro";
                case CardTypeFlags.Xyz: return "Xyz";
                case CardTypeFlags.Flip: return "Flip";
                case CardTypeFlags.Pendulum: return "Pendulum";
            }
        }
    }

    public enum CardAttribute
    {
        Unknown = 0,
        LightMonster = 1,
        DarkMonster = 2,
        WaterMonster = 3,
        FireMonster = 4,
        EarthMonster = 5,
        WindMonster = 6,
        DivineMonster = 7,
        Spell = 8,
        Trap = 9
    }

    public enum CardType
    {
        Default = 0,
        Effect = 1,
        Fusion = 2,
        FusionEffect = 3,
        Ritual = 4,
        RitualEffect = 5,
        ToonEffect = 6,
        SpiritEffect = 7,
        UnionEffect = 8,
        GeminiEffect = 9,
        Token = 10,
        Spell = 13,
        Trap = 14,
        Tuner = 15,
        TunerEffect = 16,
        Synchro = 17,
        SynchroEffect = 18,
        SynchroTunerEffect = 19,
        DarkTunerEffect = 20,
        DarkSynchroEffect = 21,
        Xyz = 22,
        XyzEffect = 23,
        FlipEffect = 24,
        Pendulum = 25,
        PendulumEffect = 26,
        EffectSp = 27,
        ToonEffectSp = 28,
        SpiritEffectSp = 29,
        TunerEffectSp = 30,
        DarkTunerEffectSp = 31,
        FlipTunerEffect = 32,
        PendulumTunerEffect = 33,
        XyzPendulumEffect = 34,
        PendulumFlipEffect = 35,
        AnyNormal = 37,
        AnySynchro = 38,
        AnyXyz = 39,
        AnyTuner = 40,
        AnyFusion = 41,
        AnyRitual = 42,
        AnyPendulum = 43,
        AnyFlip = 44
    }

    [Flags]
    public enum CardTypeFlags : uint
    {
        Default = 0,
        Effect = 1 << 0,
        Fusion = 1 << 1,
        Ritual = 1 << 2,
        Toon = 1 << 3,
        Spirit = 1 << 4,
        Union = 1 << 5,
        Gemini = 1 << 6,
        Token = 1 << 7,
        Spell = 1 << 8,
        Trap = 1 << 9,
        Tuner = 1 << 10,
        DarkTuner = 1 << 11,
        DarkSynchro = 1 << 12,
        Synchro = 1 << 13,
        Xyz = 1 << 14,
        Flip = 1 << 15,
        Pendulum = 1 << 16,
        SpecialSummon = 1 << 17,
        Normal = 1 << 19,
        Any = 1 << 20
    }

    public enum MonsterType
    {
        Unknown = 0,
        Dragon = 1,
        Zombie = 2,
        Fiend = 3,
        Pyro = 4,
        SeaSerpent = 5,
        Rock = 6,
        Machine = 7,
        Fish = 8,
        Dinosaur = 9,
        Insect = 10,
        Beast = 11,
        BeastWarrior = 12,
        Plant = 13,
        Aqua = 14,
        Warrior = 15,
        WingedBeast = 16,
        Fairy = 17,
        Spellcaster = 18,
        Thunder = 19,
        Reptile = 20,
        Psychic = 21,
        Wyrm = 22,
        DivineBeast = 23,
        CreatorGod = 24,
        Spell = 25,
        Trap = 26
    }

    public enum SpellType
    {
        Normal = 0,
        Counter = 1,
        Field = 2,
        Equip = 3,
        Continuous = 4,
        QuickPlay = 5,
        Ritual = 6
    }

    public enum CardFrameType
    {
        Normal,
        Effect,
        Token,
        Ritual,
        Fusion,
        PendulumEffect,
        PendulumNormal,
        PendulumSynchro,
        PendulumXyz,
        Synchro,
        Xyz,
        Spell,
        Trap
    }

    public enum CardLimitation
    {
        NotLimited,
        Forbidden,
        Limited,
        SemiLimited
    }

    [Flags]
    public enum CardGenre : ulong
    {
        None = 0,
        RecoverLP = 1UL << 0, //0x0000000000000001 ICON_ID_GENRE_LPUP
        DamageLP = 1UL << 1, //0x0000000000000002 ICON_ID_GENRE_LPDOWN
        HelpDraw = 1UL << 2, //0x0000000000000004 ICON_ID_GENRE_DRAW
        SpecialSummon = 1UL << 3, //0x0000000000000008 ICON_ID_GENRE_SPSUMMON
        NegateEffect = 1UL << 4, //0x0000000000000010 ICON_ID_GENRE_DISABLE
        SearchDeck = 1UL << 5, //0x0000000000000020 ICON_ID_GENRE_DECKSEARCH
        RecoverFromGraveyard = 1UL << 6, //0x0000000000000040 ICON_ID_GENRE_USEGRAVE
        IncreaseDecreaseAtkDef = 1UL << 7, //0x0000000000000080 ICON_ID_GENRE_POWER
        ChangeBattlePosition = 1UL << 8, //0x0000000000000100 ICON_ID_GENRE_POSITION
        SetControls = 1UL << 9, //0x0000000000000200 ICON_ID_GENRE_CONTROL
        DestroyMonster = 1UL << 10, //0x0000000000000400 ICON_ID_GENRE_BREAKMONST
        DestroySpellCard = 1UL << 11, //0x0000000000000800 ICON_ID_GENRE_BREAKMAGIC
        DestroyHand = 1UL << 12, //0x0000000000001000 ICON_ID_GENRE_HANDDES
        DestroyDeck = 1UL << 13, //0x0000000000002000 ICON_ID_GENRE_DECKDES
        RemoveCard = 1UL << 14, //0x0000000000004000 ICON_ID_GENRE_REMOVECARD
        ReturnCard = 1UL << 15, //0x0000000000008000 ICON_ID_GENRE_CARDBACK
        Piercing = 1UL << 16, //0x0000000000010000 ICON_ID_GENRE_SPEAR
        DirectAttack = 1UL << 17, //0x0000000000020000 ICON_ID_GENRE_DIRECTATK
        AttackMultipleTimes = 1UL << 18, //0x0000000000040000 ICON_ID_GENRE_MANYATK
        CannotBeDestroyed = 1UL << 19, //0x0000000000080000 ICON_ID_GENRE_UNBREAK
        LimitAttack = 1UL << 20, //0x0000000000100000 ICON_ID_GENRE_LIMITATK
        CannotNormalSummon = 1UL << 21, //0x0000000000200000 ICON_ID_GENRE_CANTSUMMON
        FlipEffectMonster = 1UL << 22, //0x0000000000400000 ICON_ID_GENRE_REVERSE
        ToonMonster = 1UL << 23, //0x0000000000800000 ICON_ID_GENRE_TOON
        SpiritMonster = 1UL << 24, //0x0000000001000000 ICON_ID_GENRE_SPIRIT
        UnionMonster = 1UL << 25, //0x0000000002000000 ICON_ID_GENRE_UNION
        GeminiMonster = 1UL << 26, //0x0000000004000000 ICON_ID_GENRE_DUAL
        LvMonster = 1UL << 27, //0x0000000008000000 ICON_ID_GENRE_LEVELUP
        Original = 1UL << 28, //0x0000000010000000 ICON_ID_GENRE_ORIGINAL
        FusionMaterialMonster = 1UL << 29, //0x0000000020000000 ICON_ID_GENRE_FUSION
        Ritual = 1UL << 30, //0x0000000040000000 ICON_ID_GENRE_RITUAL
        Token = 1UL << 31, //0x0000000080000000 ICON_ID_GENRE_TOKEN
        Counter = 1UL << 32, //0x0000000100000000 ICON_ID_GENRE_COUNTER
        Gamble = 1UL << 33, //0x0000000200000000 ICON_ID_GENRE_GAMBLE
        AttributeRelated = 1UL << 34, //0x0000000400000000 ICON_ID_GENRE_ATTR
        TypeRelated = 1UL << 35, //0x0000000800000000 ICON_ID_GENRE_TYPE
        Tuner = 1UL << 36, //0x0000001000000000 ICON_ID_GENRE_TUNER
        SynchroMonster = 1UL << 37, //0x0000002000000000 ICON_ID_GENRE_SYNC
        SendToGraveyard = 1UL << 38, //0x0000004000000000 ICON_ID_GENRE_DROPGRAVE

        // These values don't visibly appear in the game
        NormalMonsterRelated = 1UL << 39, //0x0000008000000000 ICON_ID_GENRE_NORMAL
        LightMonsterRelated = 1UL << 40, //0x0000010000000000 ICON_ID_GENRE_ATTR_LIGHT
        DarkMonsterRelated = 1UL << 41, //0x0000020000000000 ICON_ID_GENRE_ATTR_DARK
        EarthMonsterRelated = 1UL << 42, //0x0000040000000000 ICON_ID_GENRE_ATTR_EARTH
        WaterMonsterRelated = 1UL << 43, //0x0000080000000000 ICON_ID_GENRE_ATTR_WATER
        FireMonsterRelated = 1UL << 44, //0x0000100000000000 ICON_ID_GENRE_ATTR_FIRE
        WindMonsterRelated = 1UL << 45, //0x0000200000000000 ICON_ID_GENRE_ATTR_WIND

        XyzMonster = 1UL << 46, //0x0000400000000000 ICON_ID_GENRE_XYZ
        LevelModifier = 1UL << 47, //0x0000800000000000 ICON_ID_GENRE_LVUPDOWN
        Pendulum = 1UL << 48, //0x0001000000000000 ICON_ID_GENRE_PENDULUM

        // These values aren't on any cards (but appear in game if you force them on a card)
        DivineAttribute = 1UL << 49, //0x0002000000000000 ICON_ID_GENRE_ATTR_GOD
        NewCard = 1UL << 50, //0x0004000000000000 ICON_ID_GENRE_NEW
        GameOriginal = 1UL << 51, //0x0008000000000000 ICON_ID_GENRE_GAME_ORIGINAL

        CardVaritation = 1UL << 52 //0x0010000000000000 ICON_ID_GENRE_PICTURE (assumed)
    }

    public enum CardNameType
    {
        Null,
        Toon,
        Demon,
        Keeper,
        Guardian,
        Scorpion,
        Amazoness,
        Ninja,
        Level,
        Ehero,
        Dhero,
        NeosMaterial,
        NeosFusion,
        Neos,
        Ojama,
        Battery,
        DarkWorld,
        Bes,
        Antique,
        Sphinx,
        Machiners,
        Harpie,
        Roid,
        Vehicloid,
        Neospacian,
        Cocoon,
        Alien,
        Mythical,
        Hero,
        Allure,
        Gadget,
        Six,
        Jewel,
        Volcanic,
        BlazeCanon,
        Venom,
        Cloudian,
        Gladial,
        Weapon,
        Takemitsu,
        EvHero,
        Drunk,
        Arcana,
        Fossil,
        Gunner,
        Forbidden,
        Rainbow,
        CyberFusion,
        Icebarrier,
        Aoj,
        Saber,
        Worm,
        LightLord,
        Frog,
        Nitro,
        Genex,
        MistValley,
        Flamebell,
        NeosNhero,
        Deformer,
        Chain,
        Natul,
        Clear,
        RedEyes,
        BlackFeather,
        SlashBuster,
        Roaring,
        Jurac,
        RealGenex,
        EarthbindGod,
        Koakimail,
        Infernity,
        XSaber,
        FortuneLady,
        Dragnity,
        FortuneWitch,
        Synchron,
        Saviour,
        Reptiles,
        Shien,
        Junk,
        Tomabo,
        Sin,
        Gem,
        GemKnight,
        Laval,
        Vailon,
        Scrap,
        Eleki,
        Fusion,
        Infinity,
        Wisel,
        Tg,
        Karakuri,
        Ritua,
        Gusta,
        Invelds,
        Reactor,
        Agent,
        Polestar,
        PolestarBeast,
        PolestarGhost,
        PolestarAngel,
        PolestarItem,
        PoleGod,
        SoundWarrior,
        Resonator,
        Mhero,
        Vhero,
        MeklordEmp,
        MeklordSld,
        Meklord,
        Zenmai,
        Penguin,
        Evold,
        Evolder,
        TrapHole,
        TimeGod,
        Sacred,
        Velds,
        Numbers,
        Gagaga,
        Gogogo,
        Photon,
        Ninjutsu,
        Inzector,
        Invasion,
        Bouncer,
        Butterfly,
        HolySeal,
        Majin,
        Heroic,
        Ooparts,
        Spellbook,
        Madolce,
        Geargear,
        Xyz,
        Poseidon,
        Mermail,
        Abyss,
        Magical,
        Nimble,
        Duston,
        Medallion,
        NobleKnight,
        FireKing,
        Galaxy,
        HolySword,
        FireStar,
        FireDance,
        HazeBeast,
        Haze,
        ZexalWeapon,
        Hope,
        GimmickPuppet,
        Dododo,
        Bk,
        PhantomMek,
        FireKingBeast,
        ChaosNumbers,
        ChaosXyz,
        Geargearno,
        SdRobo,
        SdRobo2,
        Umbral,
        HolyLightning,
        Bujin,
        Kowakuma,
        Hole,
        CNo39,
        H_Challenger,
        Malicebolus,
        Ghostrick,
        Vampire,
        Cat,
        CyberDragon,
        Cybernetic,
        Shinra,
        Necrovalley,
        Zubaba,
        Fishborg,
        RUM,
        Medallion2,
        Artifact,
        Evolkaiser,
        GalaxyEyes,
        Tachyon,
        Over100,
        Wizard,
        OddEyes,
        LegendDragon,
        LegendKnight,
        WingedKuriboh,
        Stardust,
        Sprout,
        Artorius,
        Lancelot,
        Superheavy,
        Genso,
        Tellarknight,
        Shadoll,
        DragonStar,
        EM,
        Change,
        Higan,
        Ua,
        DD,
        DDD,
        Furnimal,
        Deathtoy,
        Qliphot,
        Bunborg,
        Goblin,
        Cthulhu,
        Contract,
        Gottoms,
        Yosen,
        Necroth,
        Spirit_All,
        Spirit_Tamer,
        Spirit_Beast,
        RR,
        Infernoid,
        Jinzo,
        Gaia,
        Monarch,
        Charmer,
        Possessed,
        Crystal,
        Warrior,
        PowerTool,
        BMG,
        EdgeImp,
        Sephira,
        GensoPrincess,
        Spirit_Rider,
        Stellarknight,
        Void,
        Em,
        Dragonsword,
        Igknight,
        Aroma,
        Empowered,
        AetherWeapon,
        FortunePrince,
        Aquaactress,
        Aquarium,
        ChaosSoldier,
        Majespecter,
        Gradle,
        SOz,
        Kaiju,
        SR,
        PSYFrame,
        RedDemon,
        Burgestoma,
        Dante,
        BusterBlader,
        BusterSword,
        Dynamist,
        Shiranui,
        Dragondevil,
        Exodia,
        PhantomKnight,
        Phantom,
        Super,
        Super_Quantum,
        Super_Machine,
        BlueEyes,
        HopeX,
        Moonlight,
        Amorphage,
        ElfSwordsman,
        MagicianGirl,
        BlackMagic,
        Metalphose,
        Tramid,
        ABF,
        Houkai,
        Chaos,
        CyberAngel,
        Cypher,
        Cardian,
        SilentSword,
        SilentMagic,
        MagnetWarrior,
        BlackMagic2,
        Kuriboh,
        Crystron,
        Kagoju,
        ApoQliphot,
        Chichukai,
        ChichukaiRyu,
        Spyral,
        SpyralGear,
        MakaiGekidan,
        MakaiDaihon,
        FallenAngel,
        WW,
        Beast12,
        PendDragon
    }

    public class RelatedCardInfo
    {
        public RelatedCardInfo(Card_Info card, Card_Tag_Info tagInfo)
        {
            Card = card;
            TagInfo = tagInfo;
        }

        public Card_Info Card { get; set; }
        public Card_Tag_Info TagInfo { get; set; }
    }

    public class Card_Tag_Info
    {
        public enum CardEffectType
        {
            None,
            AntiAttack,
            AntiDefense,
            AntiDiscard,
            AntiDraw,
            AntiEffectDamage,
            AntiFaceDown,
            AntiMonsterEffect,
            AntiPendulum,
            AntiSpell,
            AntiTrap,
            AtkReduction,
            AttackDirectly,
            AttributeDestruction,
            AttributeEquipBoost,
            BanishOpp,
            BanishPlayer,
            BoostNormal,
            BurnDamageAtk,
            BurnDamageCont,
            BurnDamageDirect,
            BurnDamageMons,
            BurnDamageTrib,
            CannotAttack,
            CannotChangePosition,
            CardDiscard,
            CardDraw,
            ChangeLevel,
            ChooseAttackTarget,
            CoinToss,
            Combo,
            ContinuousSpellTrib,
            DarkCardDraw,
            DefGain,
            DefReduction,
            DestroyType,
            Dice,
            DragonBoost,
            EquipDragon,
            EquipFairy,
            EquipMachine,
            EquipSpellcaster,
            EquipWarrior,
            FaceUp,
            FieldPowerAttr,
            FieldPowerType,
            FlipFaceDown,
            Fusion,
            GiveControl,
            GraveToHandMonster,
            GraveToHandSpellTrap,
            IceCount,
            LargeAtkGainEquip,
            LifeGain,
            LookAtDeck,
            LookAtHand,
            Mill,
            NegateAttack,
            RecycleToDeck,
            RestrictMonster,
            Ritual,
            SameAttributeBoost,
            Simochi,
            SpellTrapProtect,
            SsGraveyard,
            SsZombie,
            StopFlipNormalSummon,
            StopSpecialSummon,
            SwitchAtkDef,
            SynchroFusion,
            SynchroMaterial,
            TakeControl,
            ToGraveyard,
            Token,
            TokenOpponent,
            TrapMonster,
            Ultimaya,
            ZoneDeny,
            SpellDoubleSummon,
            SpellMonsterDestruction,
            SpellMonsterProtect,
            SpellPiercing,
            SpellPreventBattleDamage,
            SpellQuickAtKboost,
            SpellSsHandDeck,
            SpellStopFusion,
            SpellStopRitual,
            SpellStopSynchro,
            SpellStopTribute,
            SpellStopXyz
        }

        public enum ElementType
        {
            None = -1,
            AtkLessThanOrEquals = 0,
            DefLessThanOrEquals = 2,
            LevelLessThanOrEquals = 4,
            RankLessThanOrEquals = 8,
            AtkLessThan = 256,
            Attribute = 513,
            AtkEquals = 512,
            DefEquals = 514,
            CardType = 515,
            LevelEquals = 516,
            SpellType = 517,
            MonsterType = 518,
            DeckType = 519,
            RankEquals = 520,
            SpecialSummon = 521,
            Tribute = 522,
            Tribute2 = 523,
            AtkGreaterThanOrEquals = 768,
            LevelGreaterThanOrEquals = 772,
            RankGreaterThanOrEquals = 776
        }

        public enum ExactType
        {
            None,
            RelatedTo,
            FusionMonster,
            RitualMonster,
            SpellTrap,
            CardEffect,
            WorksWellWith,
            SpellCounter,
            CounterTrapFairy,
            BanishBeast,
            BanishDark,
            BanishFish,
            BanishRock
        }

        public enum Type
        {
            Exact = 0,
            Ad = 1,
            Find = 2,
            AdXyz = 257,
            FindXyz = 258
        }

        public Card_Tag_Info()
        {
            Elements = new Element[8];
            Text = new Localized_Text();
            DisplayText = new Localized_Text();
        }

        public int Index { get; set; }
        public ExactType Exact { get; set; }
        public CardEffectType CardEffect { get; set; }
        public Card_Info ExactCard { get; set; }
        public Type MainType { get; set; }
        public short MainValue { get; set; }
        public Element[] Elements { get; set; }
        public Localized_Text Text { get; }
        public Localized_Text DisplayText { get; }

        public struct Element
        {
            public ElementType Type;
            public short Value;
        }
    }

    public enum DeckType
    {
        Main = 0,
        Extra = 1,
        Side = 2
    }
}