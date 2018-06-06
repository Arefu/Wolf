using System;

namespace Yu_Gi_Oh.Save_File
{
    /// <summary>
    /// This is a public enum to determine what content is unlocked.
    /// </summary>
    /// <seealso cref="UnlockedBattlePacks"/>
    /// <seealso cref="DeulistChallengeState"/>
    /// <seealso cref="UnlockedShopPacks"/>
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