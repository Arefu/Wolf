using System;
using System.Collections.Generic;
using System.IO;
using Yu_Gi_Oh.File_Handling.LOTD_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public partial class Game_Save
    {
        public void SetDuelPoints(int duelPoints)
        {
            Misc.DuelPoints = duelPoints;
        }

        public void UnlockPadlockedContent()
        {
            Misc.UnlockedContent = UnlockedContent.All;
            Misc.UnlockedShopPacks = UnlockedShopPacks.All;
            Misc.UnlockedBattlePacks = UnlockedBattlePacks.All;
        }

        public void UnlockAllRecipes()
        {
            for (var i = 0; i < Misc.UnlockedRecipes.Length; i++) Misc.UnlockedRecipes[i] = true;
        }

        public void UnlockAllAvatars()
        {
            for (var i = 0; i < Misc.UnlockedAvatars.Length; i++) Misc.UnlockedAvatars[i] = true;
        }

        public void SetAllChallenges(DeulistChallengeState state)
        {
            for (var i = 0; i < Misc.Challenges.Length; i++) Misc.Challenges[i] = state;
        }

        public void SetAllCampaignDuels(CampaignDuelState state)
        {
            SetAllCampaignDuels(state, CampaignDuelState.Locked, false);
        }

        public void SetAllCampaignDuels(CampaignDuelState state, CampaignDuelState reverseState)
        {
            SetAllCampaignDuels(state, reverseState, true);
        }

        private void SetAllCampaignDuels(CampaignDuelState state, CampaignDuelState reverseState, bool setReverseState)
        {
            for (var i = 0; i < Campaign_Save.DuelsPerSeries; i++)
            {
                var tempState = i == 0 ? CampaignDuelState.Available : state;

                Campaign.DuelsBySeries[Duel_Series.YuGiOh][i].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhGX][i].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOh5D][i].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhZEXAL][i].State = tempState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhARCV][i].State = tempState;

                if (!setReverseState) continue;

                Campaign.DuelsBySeries[Duel_Series.YuGiOh][i].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhGX][i].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOh5D][i].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhZEXAL][i].ReverseDuelState = reverseState;
                Campaign.DuelsBySeries[Duel_Series.YuGiOhARCV][i].ReverseDuelState = reverseState;
            }
        }

        public void UnlockAllCards()
        {
            SetAllOwnedCardsCount(3);
        }

        public void SetAllOwnedCardsCount(byte cardCount)
        {
            SetAllOwnedCardsCount(cardCount, true);
        }

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
            for (var i = 0; i < 4; i++) buffer[12 + i] = 0;

            ulong result = 0xFFFFFFFF;
            foreach (var file in buffer) result = ((uint) result >> 8) ^ xorTable[(byte) result ^ file];
            return (uint) result;
        }

        public void FixGameSaveSignatureOnDisk()
        {
            FixGameSaveSignatureOnDisk(GetSaveFilePath());
        }

        public void FixGameSaveSignatureOnDisk(string path)
        {
            if (!File.Exists(path)) return;

            var buffer = File.ReadAllBytes(path);
            SaveSignature(buffer);
            File.WriteAllBytes(path, buffer);
        }

        internal void CopyChunk(int chunkIndex, string fromPath, string toPath)
        {
            if (!File.Exists(fromPath) || !File.Exists(toPath))
            {
                Console.WriteLine("Bad file path for CopyChunk");
                return;
            }

            if (!GetChunkRange(chunkIndex, out var chunkStart, out var chunkEnd)) return;

            var fromFileBuffer = File.ReadAllBytes(fromPath);
            var toFileBuffer = File.ReadAllBytes(toPath);

            Buffer.BlockCopy(fromFileBuffer, chunkStart, toFileBuffer, chunkStart, chunkEnd - chunkStart);

            SaveSignature(toFileBuffer);
            File.WriteAllBytes(toPath, toFileBuffer);
        }

        private int GetChunkSize(int chunkIndex)
        {
            if (GetChunkRange(chunkIndex, out var chunkStart, out var chunkEnd))
                return chunkEnd - chunkStart;

            return -1;
        }

        private static bool GetChunkRange(int chunkIndex, out int chunkStart, out int chunkEnd)
        {
            int[] offsets =
            {
                UnkOffset1, StatsOffset, BattlePacksOffset, MiscDataOffset, CampaignDataOffset,
                DecksOffset, CardListOffset, FileLength
            };

            chunkStart = -1;
            chunkEnd = -1;

            if (chunkIndex < 0 || chunkIndex > 6) return false;

            chunkStart = offsets[chunkIndex];
            chunkEnd = offsets[chunkIndex + 1];
            return true;
        }

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
                    var userdataDir = Path.Combine(installDir, "../../../userdata/");
                    if (Directory.Exists(userdataDir))
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
            }

            return null;
        }
    }
}