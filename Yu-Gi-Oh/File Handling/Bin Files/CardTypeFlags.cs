using System;

namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    /// <summary>
    ///     What are the flags of the card.
    /// </summary>
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
}