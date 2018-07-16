using System;
using System.Collections.Generic;
using Yu_Gi_Oh.File_Handling.Utility;
using Yu_Gi_Oh.File_Handling.ZIB_Files;

namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    /// <summary>
    ///     Card Info class which holds information about a card.
    /// </summary>
    public class Card_Info
    {
        /// <summary>
        ///     Default contructor
        /// </summary>
        /// <param name="index">The index the card is at in a list.</param>
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

        /// <summary>
        ///     A public representation of the Index
        /// </summary>
        public int CardIndex { get; set; }

        /// <summary>
        ///     The game ID of the card.
        /// </summary>
        public short CardId { get; set; }

        /// <summary>
        ///     The ZIB the card image belongs to.
        /// </summary>
        public ZIB_File ImageFile { get; set; }

        /// <summary>
        ///     The name of the card.
        /// </summary>
        public Localized_Text Name { get; set; }

        /// <summary>
        ///     The description of the card.
        /// </summary>
        public Localized_Text Description { get; set; }

        /// <summary>
        ///     A list of related cards.
        /// </summary>
        public List<RelatedCardInfo> RelatedCards { get; }

        /// <summary>
        ///     A collection of card effect tags.
        /// </summary>
        public HashSet<Card_Tag_Info.CardEffectType> CardEffectTags { get; }

        /// <summary>
        ///     A collection of name types.
        /// </summary>
        public HashSet<CardNameType> NameTypes { get; set; }

        /// <summary>
        ///     A list of set IDs the card belogns to.
        /// </summary>
        public List<int> SetIds { get; }

        /// <summary>
        ///     The ATK of the card if it's a Monster.
        /// </summary>
        public int Atk { get; set; }

        /// <summary>
        ///     The DEF of the card if it's a Monster.
        /// </summary>
        public int Def { get; set; }

        /// <summary>
        ///     The Level of the Card if it's a Monster.
        /// </summary>
        public byte Level { get; set; }

        /// <summary>
        ///     Will be True if I don't know the ATK.
        /// </summary>
        public bool IsUnknownAtk => Atk == 5110;

        /// <summary>
        ///     Will be True if I don't know the DEF.
        /// </summary>
        public bool IsUnknownDef => Def == 5110;

        /// <summary>
        ///     Attribute of the Card.
        /// </summary>
        public CardAttribute Attribute { get; set; }

        /// <summary>
        ///     The type of card.
        /// </summary>
        public CardType CardType { get; set; }

        /// <summary>
        ///     The type of spell.
        /// </summary>
        public SpellType SpellType { get; set; }

        /// <summary>
        ///     The type of Monster.
        /// </summary>
        public MonsterType MonsterType { get; set; }

        /// <summary>
        ///     The Pendulum scale of the monster. 1/2
        /// </summary>
        public byte PendulumScale1 { get; set; }

        /// <summary>
        ///     The Pendulum scale of the monster. 2/2
        /// </summary>
        public byte PendulumScale2 { get; set; }

        /// <summary>
        ///     The final Pendulum scale of the monster.
        /// </summary>
        public byte PendulumScale => Math.Max(PendulumScale1, PendulumScale2);

        /// <summary>
        ///     What is the limiation status of this card.
        /// </summary>
        public CardLimitation Limit { get; set; }

        /// <summary>
        ///     What is the Genre of the card.
        /// </summary>
        public CardGenre Genre { get; set; }

        /// <summary>
        ///     What is the CardType.
        /// </summary>
        public CardTypeFlags CardTypeFlags => GetCardTypeFlags(CardType);

        /// <summary>
        ///     Is this card a token.
        /// </summary>
        public bool IsMonsterToken => IsMonster && CardTypeFlags.HasFlag(CardTypeFlags.Token);

        /// <summary>
        ///     Is the card an effect.
        /// </summary>
        public bool IsEffect => CardTypeFlags.HasFlag(CardTypeFlags.Effect);

        /// <summary>
        ///     Is the card a monster.
        /// </summary>
        public bool IsMonster => Attribute != CardAttribute.Spell && Attribute != CardAttribute.Trap;

        /// <summary>
        ///     Is the card a normal monster.
        /// </summary>
        public bool IsNormalMonster => FrameType == CardFrameType.Normal || FrameType == CardFrameType.PendulumNormal;

        /// <summary>
        ///     Is the card a Pendulum.
        /// </summary>
        public bool IsPendulum => CardTypeFlags.HasFlag(CardTypeFlags.Pendulum);

        /// <summary>
        ///     Is the card an Xyz.
        /// </summary>
        public bool IsXyz => CardTypeFlags.HasFlag(CardTypeFlags.Xyz);

        /// <summary>
        ///     Is the card a Synchro.
        /// </summary>
        public bool IsSynchro => CardTypeFlags.HasFlag(CardTypeFlags.Synchro);

        /// <summary>
        ///     Is the card a Fusion monster.
        /// </summary>
        public bool IsFusion => CardTypeFlags.HasFlag(CardTypeFlags.Fusion);

        /// <summary>
        ///     Is the card part of the MainDeck.
        /// </summary>
        public bool IsMainDeckCard => !IsExtraDeckCard;

        /// <summary>
        ///     Does the card belong in the side deck.
        /// </summary>
        public bool IsExtraDeckCard => CardTypeFlags.HasFlag(CardTypeFlags.Xyz) ||
                                       CardTypeFlags.HasFlag(CardTypeFlags.Fusion) ||
                                       CardTypeFlags.HasFlag(CardTypeFlags.Synchro);

        /// <summary>
        ///     Is the card a spell.
        /// </summary>
        public bool IsSpell => Attribute == CardAttribute.Spell;

        /// <summary>
        ///     Is the card a trap.
        /// </summary>
        public bool IsTrap => Attribute == CardAttribute.Trap;

        /// <summary>
        ///     What is the name of the frame to load.
        /// </summary>
        public string FrameName => GetFrameName(FrameType);

        /// <summary>
        ///     What is the frame type of the card
        /// </summary>
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
                    return cardFlags.HasFlag(CardTypeFlags.Pendulum)
                        ? CardFrameType.PendulumSynchro
                        : CardFrameType.Synchro;

                if (cardFlags.HasFlag(CardTypeFlags.Xyz))
                    return cardFlags.HasFlag(CardTypeFlags.Pendulum) ? CardFrameType.PendulumXyz : CardFrameType.Xyz;

                if (cardFlags.HasFlag(CardTypeFlags.Pendulum))
                    return cardFlags.HasFlag(CardTypeFlags.Effect)
                        ? CardFrameType.PendulumEffect
                        : CardFrameType.PendulumNormal;

                if (cardFlags.HasFlag(CardTypeFlags.Token))
                    return CardFrameType.Token;

                if (cardFlags.HasFlag(CardTypeFlags.Fusion))
                    return CardFrameType.Fusion;

                if (cardFlags.HasFlag(CardTypeFlags.Ritual))
                    return CardFrameType.Ritual;

                if (cardFlags.HasFlag(CardTypeFlags.Effect) || cardFlags.HasFlag(CardTypeFlags.SpecialSummon) ||
                    cardFlags.HasFlag(CardTypeFlags.Union) || cardFlags.HasFlag(CardTypeFlags.Toon) ||
                    cardFlags.HasFlag(CardTypeFlags.Gemini))
                    return CardFrameType.Effect;

                return CardFrameType.Normal;
            }
        }


        /// <summary>
        ///     Gets the description of the monster.
        /// </summary>
        /// <param name="language">What language to get for.</param>
        /// <param name="pendulumDescription">Should I get pendulum description?</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Gets the flags for the card.
        /// </summary>
        /// <param name="cardType">The CardType of the card.</param>
        /// <returns></returns>
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
                case CardType.SynchroTunerEffect:
                    return CardTypeFlags.Synchro | CardTypeFlags.Tuner | CardTypeFlags.Effect;
                case CardType.DarkTunerEffect: return CardTypeFlags.DarkTuner | CardTypeFlags.Effect;
                case CardType.DarkSynchroEffect: return CardTypeFlags.DarkSynchro | CardTypeFlags.Effect;
                case CardType.Xyz: return CardTypeFlags.Xyz;
                case CardType.XyzEffect: return CardTypeFlags.Xyz | CardTypeFlags.Effect;
                case CardType.FlipEffect: return CardTypeFlags.Flip | CardTypeFlags.Effect;
                case CardType.Pendulum: return CardTypeFlags.Pendulum;
                case CardType.PendulumEffect: return CardTypeFlags.Pendulum | CardTypeFlags.Effect;
                case CardType.EffectSp: return CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.ToonEffectSp:
                    return CardTypeFlags.Toon | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.SpiritEffectSp:
                    return CardTypeFlags.Spirit | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.TunerEffectSp:
                    return CardTypeFlags.Tuner | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.DarkTunerEffectSp:
                    return CardTypeFlags.DarkTuner | CardTypeFlags.Effect | CardTypeFlags.SpecialSummon;
                case CardType.FlipTunerEffect: return CardTypeFlags.Flip | CardTypeFlags.Tuner | CardTypeFlags.Effect;
                case CardType.PendulumTunerEffect:
                    return CardTypeFlags.Pendulum | CardTypeFlags.Tuner | CardTypeFlags.Effect;
                case CardType.XyzPendulumEffect:
                    return CardTypeFlags.Xyz | CardTypeFlags.Pendulum | CardTypeFlags.Effect;
                case CardType.PendulumFlipEffect:
                    return CardTypeFlags.Pendulum | CardTypeFlags.Flip | CardTypeFlags.Effect;
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

        /// <summary>
        ///     Gets a string representation of the frame to know what file to load
        /// </summary>
        /// <param name="frameType">The enum type of CardFrame</param>
        /// <returns></returns>
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

        /// <summary>
        ///     This gets the full monster's type name.
        /// </summary>
        /// <param name="monsterType">The type of Monster</param>
        /// <param name="cardType">The general card type.</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Returns a string represntation of the MonsterType.
        /// </summary>
        /// <param name="monsterType">Type of Monster.</param>
        /// <returns></returns>
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

        /// <summary>
        ///     Returns a string representation of the CardTypeFlags
        /// </summary>
        /// <param name="flag">CardFlags</param>
        /// <returns></returns>
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
}