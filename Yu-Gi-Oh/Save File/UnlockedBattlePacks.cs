using System;

namespace Yu_Gi_Oh.Save_File
{
    [Flags]
    public enum UnlockedBattlePacks
    {
        None = 0,
        WarOfTheGiants = 1 << 0,
        WarOfTheGiantsRound2 = 1 << 1,
        All = WarOfTheGiants | WarOfTheGiantsRound2
    }
}