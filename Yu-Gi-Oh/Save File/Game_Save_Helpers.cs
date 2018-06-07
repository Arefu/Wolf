using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Yu_Gi_Oh.File_Handling.LOTD_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public partial class Game_Save
    {
        /// <summary>
        /// Set the users duel points, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="duelPoints"></param>
        /// <seealso cref="Save()"/>
        public void SetDuelPoints(int duelPoints)
        {
            Misc.DuelPoints = duelPoints;
        }

        /// <summary>
        /// Unlocks all padlocked content, be sure to call Save after to keep changes!
        /// </summary>
        /// <seealso cref="Save()"/>
        public void UnlockPadlockedContent()
        {
            Misc.UnlockedContent = UnlockedContent.All;
            Misc.UnlockedShopPacks = UnlockedShopPacks.All;
            Misc.UnlockedBattlePacks = UnlockedBattlePacks.All;
        }

        /// <summary>
        /// Unlock all recipes, be sure to call Save after to keep changes!
        /// </summary>
        /// <seealso cref="Save()"/>
        public void UnlockAllRecipes()
        {
            for (var i = 0; i < Misc.UnlockedRecipes.Length; i++) Misc.UnlockedRecipes[i] = true;
        }

        /// <summary>
        /// Unlock all avatars, be sure to call Save after to keep changes!
        /// </summary>
        /// <seealso cref="Save()"/>
        public void UnlockAllAvatars()
        {
            for (var i = 0; i < Misc.UnlockedAvatars.Length; i++) Misc.UnlockedAvatars[i] = true;
        }

        /// <summary>
        /// Set Challenges to specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="state">What State To Set The Challenges To</param>
        /// <seealso cref="Save()"/>
        public void SetAllChallenges(DeulistChallengeState state)
        {
            for (var i = 0; i < Misc.Challenges.Length; i++) Misc.Challenges[i] = state;
        }

        /// <summary>
        /// Set campaign duels to the specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="state">What state to set the challenges to</param>
        /// <seealso cref="Save()"/>
        public void SetAllCampaignDuels(CampaignDuelState state)
        {
            SetAllCampaignDuels(state, CampaignDuelState.Locked, false);
        }

        /// <summary>
        /// Set campaign duels to the specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="state">What state to set the challenges to</param>
        /// <param name="reverseState">What state to set the reverse duel to</param>
        /// <seealso cref="Save()"/>
        public void SetAllCampaignDuels(CampaignDuelState state, CampaignDuelState reverseState)
        {
            SetAllCampaignDuels(state, reverseState, true);
        }

        /// <summary>
        /// Set campaign duels to the specified state, be sure to call Save after to keep changes!
        /// </summary>
        /// <param name="state">What state to set the challenges to</param>
        /// <param name="reverseState">what state to set the reverse duel to</param>
        /// <param name="setReverseState">Should the reverse state be set</param>
        /// <seealso cref="Save()"/>
        private void SetAllCampaignDuels(CampaignDuelState state, CampaignDuelState reverseState, bool setReverseState)
        {
            for (var count = 0; count < Campaign_Save.DuelsPerSeries; count++)
            {
                var tempState = count == 0 ? CampaignDuelState.Available : state;

                Campaign.DuelsBySeries[Duel_Series.YuGiOh][count].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhGX][count].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOh5D][count].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhZEXAL][count].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhARCV][count].State = tempState;

                if (!setReverseState) continue;

                Campaign.DuelsBySeries[Duel_Series.YuGiOh][count].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhGX][count].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOh5D][count].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhZEXAL][count].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhARCV][count].ReverseDuelState = reverseState;
            }
        }

        /// <summary>
        /// Unlocks 3 copies of each card. Call save after!
        /// </summary>
        /// <seealso cref="Save()"/>
        public void UnlockAllCards()
        {
            SetAllOwnedCardsCount(3);
        }

        /// <summary>
        /// sets owned cards to own the specified amount. Call save after!
        /// </summary>
        /// <param name="cardCount">Tested with a max of 3 more might break the game!</param>
        /// <seealso cref="Save()"/>
        public void SetAllOwnedCardsCount(byte cardCount)
        {
            SetAllOwnedCardsCount(cardCount, true);
        }

        /// <summary>
        /// Sets all owned
        /// </summary>
        /// <param name="cardCount"></param>
        /// <param name="seen"></param>
        public void SetAllOwnedCardsCount(byte cardCount, bool seen)
        {
            for (var i = 0; i < CardList.Cards.Length; i++)
            {
                CardList.Cards[i].Seen = seen;
                CardList.Cards[i].Count = cardCount;
            }
        }

        private static void SaveSignature(byte[] buffer)
        {
            var saveCount = BitConverter.ToUInt32(buffer, 16) + 1;
            var saveCountBuf = BitConverter.GetBytes(saveCount);
            Buffer.BlockCopy(saveCountBuf, 0, buffer, 16, 4);

            var signature = GetSignature(buffer);
            var signatureBuf = BitConverter.GetBytes(signature);
            Buffer.BlockCopy(signatureBuf, 0, buffer, 12, 4);
        }

        private static uint GetSignature(IList<byte> buffer)
        {
            for (var i = 0; i < 4; i++)
            {
                buffer[12 + i] = 0;
            }
            
            return (uint)buffer.Aggregate<byte, ulong>(0xFFFFFFFF, (current, file) => ((uint) current >> 8) ^ XorTable[(byte) current ^ file]);
        }

        /// <summary>
        /// Fix the game signature, this should be called if you've done manual edits to ensure the game thinks the save is still valid.
        /// </summary>
        public void FixGameSaveSignatureOnDisk()
        {
            FixGameSaveSignatureOnDisk(GetSaveFilePath());
        }

        /// <summary>
        /// Fix the game signature on disk to make the game think the save is still valid
        /// </summary>
        /// <seealso cref="FixGameSaveSignatureOnDisk()"/>
        /// <param name="path">Path to the save file (use for pirated copies)</param>
        public void FixGameSaveSignatureOnDisk(string path)
        {
            if (!File.Exists(path)) return;

            var buffer = File.ReadAllBytes(path);
            SaveSignature(buffer);
            File.WriteAllBytes(path, buffer);
        }
        
        /// <summary>
        /// Get The Save File Path On Disk.
        /// </summary>
        /// <returns>The Location Of The Save File.</returns>
        public static string GetSaveFilePath()
        {
            var installDir = LOTD_Archive.GetInstallDirectory();
            if (string.IsNullOrEmpty(installDir)) return null;
            try
            {
                var steamAppId = 0;

                var appIdFile = Path.Combine(installDir, "steam_appid.txt");
                if (File.Exists(appIdFile))
                {
                    var lines = File.ReadAllLines(appIdFile);
                    if (lines.Length > 0) int.TryParse(lines[0], out steamAppId);
                }

                if (steamAppId > 0)
                {
                    var userdataDir = Path.Combine(installDir, "..\\..\\..\\userdata\\");
                    if (Directory.Exists(Path.GetFullPath(userdataDir)))
                    {
                        var dirs = Directory.GetDirectories(userdataDir);
                        foreach (var dir in dirs)
                        {
                            var dirName = new DirectoryInfo(dir).Name;

                            if (!long.TryParse(dirName, out var userid)) continue;
                            var saveDataDir = Path.Combine(dir, string.Empty + steamAppId, "remote");
                            if (Directory.Exists(saveDataDir))
                            {
                                var saveDataFile = Path.Combine(saveDataDir, "savegame.dat");
                                if (File.Exists(saveDataFile)) return Path.GetFullPath(saveDataFile);
                            }

                            break;
                        }
                    }
                }
            }
            catch
            {
                //TODO: Ask User For Save File Location.
            }

            return null;
        }
    }
}