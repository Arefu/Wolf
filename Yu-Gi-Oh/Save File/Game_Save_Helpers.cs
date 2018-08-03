using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Yu_Gi_Oh.File_Handling.LOTD_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public partial class Game_Save
    {
        /// <summary>
        ///     Get The Save File Path On Disk.
        /// </summary>
        /// <returns>The Location Of The Save File.</returns>
        public static string SaveFileLocation = "";

        /// <summary>
        ///     Set the users duel points, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="DuelPoints"></param>
        /// <seealso cref="Save()" />
        public void SetDuelPoints(int DuelPoints)
        {
            Misc.DuelPoints = DuelPoints;
        }

        /// <summary>
        ///     Unlocks all padlocked content, be sure to call Save after to keep changes!
        /// </summary>
        /// <seealso cref="Save()" />
        public void UnlockPadlockedContent()
        {
            Misc.UnlockedContent = UnlockedContent.All;
            Misc.UnlockedShopPacks = UnlockedShopPacks.All;
            Misc.UnlockedBattlePacks = UnlockedBattlePacks.All;
        }

        /// <summary>
        ///     Unlock all recipes, be sure to call Save after to keep changes!
        /// </summary>
        /// <seealso cref="Save()" />
        public void UnlockAllRecipes()
        {
            for (var Counter = 0; Counter < Misc.UnlockedRecipes.Length; Counter++)
                Misc.UnlockedRecipes[Counter] = true;
        }

        /// <summary>
        ///     Unlock all avatars, be sure to call Save after to keep changes!
        /// </summary>
        /// <seealso cref="Save()" />
        public void UnlockAllAvatars()
        {
            for (var Counter = 0; Counter < Misc.UnlockedAvatars.Length; Counter++)
                Misc.UnlockedAvatars[Counter] = true;
        }

        /// <summary>
        ///     Set Challenges to specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="State">What State To Set The Challenges To</param>
        /// <seealso cref="Save()" />
        public void SetAllChallenges(DeulistChallengeState State)
        {
            for (var Counter = 0; Counter < Misc.Challenges.Length; Counter++) Misc.Challenges[Counter] = State;
        }

        /// <summary>
        ///     Set campaign duels to the specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="State">What state to set the challenges to</param>
        /// <seealso cref="Save()" />
        public void SetAllCampaignDuels(CampaignDuelState State)
        {
            SetAllCampaignDuels(State, CampaignDuelState.Locked, false);
        }

        /// <summary>
        ///     Set campaign duels to the specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="State">What state to set the challenges to</param>
        /// <param name="ReverseState">What state to set the reverse duel to</param>
        /// <seealso cref="Save()" />
        public void SetAllCampaignDuels(CampaignDuelState State, CampaignDuelState ReverseState)
        {
            SetAllCampaignDuels(State, ReverseState, true);
        }

        /// <summary>
        ///     Set campaign duels to the specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="State">What state to set the challenges to</param>
        /// <param name="ReverseState">what state to set the reverse duel to</param>
        /// <param name="SetReverseState">Should the reverse state be set</param>
        /// <seealso cref="Save()" />
        private void SetAllCampaignDuels(CampaignDuelState State, CampaignDuelState ReverseState, bool SetReverseState)
        {
            for (var Counter = 0; Counter < Campaign_Save.DuelsPerSeries; Counter++)
            {
                var TempState = Counter == 0 ? CampaignDuelState.Available : State;

                Campaign.DuelsBySeries[Duel_Series.YuGiOh][Counter].State = TempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhGX][Counter].State = TempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOh5D][Counter].State = TempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhZEXAL][Counter].State = TempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhARCV][Counter].State = TempState;

                if (!SetReverseState) continue;

                Campaign.DuelsBySeries[Duel_Series.YuGiOh][Counter].ReverseDuelState = ReverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhGX][Counter].ReverseDuelState = ReverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOh5D][Counter].ReverseDuelState = ReverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhZEXAL][Counter].ReverseDuelState = ReverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhARCV][Counter].ReverseDuelState = ReverseState;
            }
        }

        /// <summary>
        ///     Unlocks 3 copies of each card. Call save after!
        /// </summary>
        /// <seealso cref="Save()" />
        public void UnlockAllCards()
        {
            SetAllOwnedCardsCount(3);
        }

        /// <summary>
        ///     sets owned cards to own the specified amount. Call save after!
        /// </summary>
        /// <param name="CardCount">Tested with a max of 3 more might break the game!</param>
        /// <seealso cref="Save()" />
        public void SetAllOwnedCardsCount(byte CardCount)
        {
            SetAllOwnedCardsCount(CardCount, true);
        }

        /// <summary>
        ///     Sets all owned
        /// </summary>
        /// <param name="CardCount"></param>
        /// <param name="Seen"></param>
        public void SetAllOwnedCardsCount(byte CardCount, bool Seen)
        {
            for (var Counter = 0; Counter < CardList.Cards.Length; Counter++)
            {
                CardList.Cards[Counter].Seen = Seen;
                CardList.Cards[Counter].Count = CardCount;
            }
        }

        private static void SaveSignature(byte[] Buffer)
        {
            var SaveCount = BitConverter.ToUInt32(Buffer, 16) + 1;
            var SaveCountBuf = BitConverter.GetBytes(SaveCount);
            System.Buffer.BlockCopy(SaveCountBuf, 0, Buffer, 16, 4);

            var Signature = GetSignature(Buffer);
            var SignatureBuf = BitConverter.GetBytes(Signature);
            System.Buffer.BlockCopy(SignatureBuf, 0, Buffer, 12, 4);
        }

        private static uint GetSignature(IList<byte> Buffer)
        {
            for (var Counter = 0; Counter < 4; Counter++) Buffer[12 + Counter] = 0;

            return (uint) Buffer.Aggregate<byte, ulong>(0xFFFFFFFF,
                (Current, File) => ((uint) Current >> 8) ^ XorTable[(byte) Current ^ File]);
        }

        /// <summary>
        ///     Fix the game signature, this should be called if you've done manual edits to ensure the game thinks the save is
        ///     still valid.
        /// </summary>
        public void FixGameSaveSignatureOnDisk()
        {
            FixGameSaveSignatureOnDisk(GetSaveFilePath());
        }

        /// <summary>
        ///     Fix the game signature on disk to make the game think the save is still valid
        /// </summary>
        /// <seealso cref="FixGameSaveSignatureOnDisk()" />
        /// <param name="Path">Path to the save file (use for pirated copies)</param>
        public void FixGameSaveSignatureOnDisk(string Path)
        {
            if (!File.Exists(Path)) return;

            var Buffer = File.ReadAllBytes(Path);
            SaveSignature(Buffer);
            File.WriteAllBytes(Path, Buffer);
        }

        public static string GetSaveFilePath()
        {
            var InstallDir = LOTD_Archive.GetInstallDirectory();
            if (string.IsNullOrEmpty(InstallDir)) return null;

            var SteamAppId = 0;

            var AppIdFile = Path.Combine(InstallDir, "steam_appid.txt");
            if (File.Exists(AppIdFile))
            {
                var Lines = File.ReadAllLines(AppIdFile);
                if (Lines.Length > 0) int.TryParse(Lines[0], out SteamAppId);
            }

            if (SteamAppId > 0)
            {
                var UserdataDir = Path.Combine(InstallDir, "..\\..\\..\\userdata\\");
                if (Directory.Exists(Path.GetFullPath(UserdataDir)))
                {
                    var Dirs = Directory.GetDirectories(UserdataDir);
                    foreach (var Dir in Dirs)
                    {
                        var DirName = new DirectoryInfo(Dir).Name;

                        if (!long.TryParse(DirName, out var Userid)) continue;
                        var SaveDataDir = Path.Combine(Dir, string.Empty + SteamAppId, "remote");
                        if (Directory.Exists(SaveDataDir))
                        {
                            var SaveDataFile = Path.Combine(SaveDataDir, "savegame.dat");
                            if (File.Exists(SaveDataFile)) return Path.GetFullPath(SaveDataFile);
                        }

                        break;
                    }
                }
            }

            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Please locate Save File";
                var Res = Ofd.ShowDialog();
                if (Res != DialogResult.OK) return null;

                SaveFileLocation = Ofd.FileName;
                return SaveFileLocation;
            }
        }
    }
}