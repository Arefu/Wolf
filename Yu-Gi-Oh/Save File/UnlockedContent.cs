using System;

namespace Yu_Gi_Oh.Save_File
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     This enum should only be used internally within this DLL.
    /// </summary>
    /// <remarks>
    ///     Johnathon, 6/06/2018.
    /// </remarks>
    /// -------------------------------------------------------------------------------------------------
    [Flags]
    public enum UnlockedContent
    {
        None = 0,
        DuelistChallenges = 1 << 0,
        BattlePack = 1 << 1,
        CardShop = 1 << 2,
        All = DuelistChallenges | BattlePack | CardShop
    }
}