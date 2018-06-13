using System;

namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    /// <summary>
    ///     What is the Genre of the card.
    /// </summary>
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
}