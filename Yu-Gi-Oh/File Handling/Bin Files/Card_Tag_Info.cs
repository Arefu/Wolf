using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    /// <summary>
    ///     The card tag information
    /// </summary>
    public class Card_Tag_Info
    {
        /// <summary>
        ///     What effect does the card have
        /// </summary>
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
            Spell_DoubleSummon,
            Spell_MonsterDestruction,
            Spell_MonsterProtect,
            Spell_Piercing,
            Spell_PreventBattleDamage,
            Spell_QuickATKboost,
            Spell_SSHandDeck,
            Spell_StopFusion,
            Spell_StopRitual,
            Spell_StopSynchro,
            Spell_StopTribute,
            Spell_StopXyz
        }

        /// <summary>
        ///     What is it's element type.
        /// </summary>
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

        /// <summary>
        ///     What's its exact type.
        /// </summary>
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

        /// <summary>
        ///     The normal type.
        /// </summary>
        public enum Type
        {
            Exact = 0,
            Ad = 1,
            Find = 2,
            AdXyz = 257,
            FindXyz = 258
        }

        /// <summary>
        ///     Default contructor.
        /// </summary>
        public Card_Tag_Info()
        {
            Elements = new Element[8];
            Text = new Localized_Text();
            DisplayText = new Localized_Text();
        }

        /// <summary>
        ///     The index of the card.
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        ///     The exact type of the card.
        /// </summary>
        public ExactType Exact { get; set; }

        /// <summary>
        ///     The effect of the card.
        /// </summary>
        public CardEffectType CardEffect { get; set; }

        /// <summary>
        ///     The exact card info.
        /// </summary>
        public Card_Info ExactCard { get; set; }

        /// <summary>
        ///     The cards main type.
        /// </summary>
        public Type MainType { get; set; }

        /// <summary>
        ///     The main value.
        /// </summary>
        public short MainValue { get; set; }

        /// <summary>
        ///     The card elements.
        /// </summary>
        public Element[] Elements { get; set; }

        /// <summary>
        ///     The back-end text of the card.
        /// </summary>
        public Localized_Text Text { get; }

        /// <summary>
        ///     The display text of the card.
        /// </summary>
        public Localized_Text DisplayText { get; }

        /// <summary>
        ///     The element for the card.
        /// </summary>
        public struct Element
        {
            public ElementType Type;
            public short Value;
        }
    }
}