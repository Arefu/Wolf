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
    public enum UnlockedBattlePacks
    {
        None = 0,
        WarOfTheGiants = 1 << 0,
        WarOfTheGiantsRound2 = 1 << 1,
        All = WarOfTheGiants | WarOfTheGiantsRound2
    }
}