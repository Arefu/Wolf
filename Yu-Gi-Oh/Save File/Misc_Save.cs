using System;
using System.IO;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.Save_File
{
    public class Misc_Save : Save_Data_Chunk
    {
        public Misc_Save()
        {
            Challenges = new DeulistChallengeState[Constants.NumDeckDataSlots];
            UnlockedRecipes = new bool[Constants.NumDeckDataSlots];
            UnlockedAvatars = new bool[153];
        }

        public int DuelPoints { get; set; }
        public DeulistChallengeState[] Challenges { get; }
        public bool[] UnlockedRecipes { get; set; }
        public bool[] UnlockedAvatars { get; set; }

        public CompleteTutorials CompleteTutorials { get; set; }
        public UnlockedContent UnlockedContent { get; set; }
        public UnlockedShopPacks UnlockedShopPacks { get; set; }
        public UnlockedBattlePacks UnlockedBattlePacks { get; set; }

        public override void Clear()
        {
            DuelPoints = 1000;

            for (var i = 0; i < Challenges.Length; i++) Challenges[i] = DeulistChallengeState.Locked;

            for (var i = 0; i < UnlockedRecipes.Length; i++) UnlockedRecipes[i] = false;

            CompleteTutorials = CompleteTutorials.None;
            UnlockedContent = UnlockedContent.None;
            UnlockedShopPacks = UnlockedShopPacks.None;
            UnlockedBattlePacks = UnlockedBattlePacks.None;
        }

        public override void Load(BinaryReader reader)
        {
            reader.ReadBytes(16);

            DuelPoints = (int) reader.ReadInt64();

            var unlockedAvatarsBuffer = reader.ReadBytes(32);
            for (var i = 0; i < UnlockedAvatars.Length; i++)
            {
                var byteIndex = i / 8;
                var bitIndex = i % 8;
                UnlockedAvatars[i] = (unlockedAvatarsBuffer[byteIndex] & (byte) (1 << bitIndex)) != 0;
            }

            for (var i = 0; i < Constants.NumDeckDataSlots; i++) Challenges[i] = (DeulistChallengeState) reader.ReadInt32();

            var unlockedRecipesBuffer = reader.ReadBytes(60);
            for (var i = 0; i < Constants.NumDeckDataSlots; i++)
            {
                var byteIndex = i / 8;
                var bitIndex = i % 8;
                UnlockedRecipes[i] = (unlockedRecipesBuffer[byteIndex] & (byte) (1 << bitIndex)) != 0;
            }

            UnlockedShopPacks = (UnlockedShopPacks) reader.ReadUInt32();
            UnlockedBattlePacks = (UnlockedBattlePacks) reader.ReadUInt32();
            reader.ReadBytes(8);

            CompleteTutorials = (CompleteTutorials) reader.ReadInt32();
            UnlockedContent = (UnlockedContent) reader.ReadInt32();
        }

        public override void Save(BinaryWriter writer)
        {
            writer.Write(new byte[16]);
            writer.Write((long) DuelPoints);
            var unlockedAvatarsBuffer = new byte[32];
            for (var i = 0; i < UnlockedAvatars.Length; i++)
            {
                if (!UnlockedAvatars[i]) continue;

                var byteIndex = i / 8;
                var bitIndex = i % 8;
                unlockedAvatarsBuffer[byteIndex] |= (byte) (1 << bitIndex);
            }

            writer.Write(unlockedAvatarsBuffer);

            for (var i = 0; i < Constants.NumDeckDataSlots; i++) writer.Write((int) Challenges[i]);

            var unlockedRecipesBuffer = new byte[60];
            for (var i = 0; i < UnlockedRecipes.Length; i++)
            {
                if (!UnlockedRecipes[i]) continue;

                var byteIndex = i / 8;
                var bitIndex = i % 8;
                unlockedRecipesBuffer[byteIndex] |= (byte) (1 << bitIndex);
            }

            writer.Write(unlockedRecipesBuffer);

            writer.Write((uint) UnlockedShopPacks);
            writer.Write((uint) UnlockedBattlePacks);
            writer.Write(new byte[8]);

            writer.Write((int) CompleteTutorials);
            writer.Write((int) UnlockedContent);
        }
    }

    public enum DeulistChallengeState
    {
        Locked = 0,
        Available = 1,
        Failed = 2,
        Complete = 3
    }

    [Flags]
    public enum UnlockedShopPacks : uint
    {
        None = 0,
        GrandpaMuto = 1 << 1,
        MaiValentine = 1 << 2,
        Bakura = 1 << 3,
        JoeyWheeler = 1 << 4,
        SetoKaiba = 1 << 5,
        Yugi = 1 << 6,
        AlexisRhodes = 1 << 7,
        BastionMisawa = 1 << 8,
        ChazzPrinceton = 1 << 9,
        SyrusTruesdale = 1 << 10,
        JesseAnderson = 1 << 11,
        JadenYuki = 1 << 12,
        TetsuTrudge = 1 << 13,
        LeoLuna = 1 << 14,
        AkizaIzinski = 1 << 15,
        JackAtlas = 1 << 16,
        Crow = 1 << 17,
        YuseiFudo = 1 << 18,
        CathyKatherine = 1 << 19,
        Quinton = 1 << 20,
        KiteTenjo = 1 << 21,
        Shark = 1 << 22,
        YumaTsukumo = 1 << 23,
        Pendulum = 1 << 24,
        GongStrong = 1 << 25,
        ZuzuBoyle = 1 << 26,
        All = 0xFFFFFFFF
    }

    [Flags]
    public enum UnlockedBattlePacks
    {
        None = 0,
        WarOfTheGiants = 1 << 0,
        WarOfTheGiantsRound2 = 1 << 1,
        All = WarOfTheGiants | WarOfTheGiantsRound2
    }

    [Flags]
    public enum UnlockedContent
    {
        None = 0,
        DuelistChallenges = 1 << 0,
        BattlePack = 1 << 1,
        CardShop = 1 << 2,
        All = DuelistChallenges | BattlePack | CardShop
    }

    [Flags]
    public enum CompleteTutorials
    {
        None = 0,
        Tut01 = 1 << 1,
        Tut02 = 1 << 2,
        Tut03 = 1 << 3,
        Tut04 = 1 << 4,
        Tut05 = 1 << 5,
        Tut06 = 1 << 6,
        Tut07 = 1 << 7,
        Tut08 = 1 << 8,
        Tut09 = 1 << 9,
        Tut10 = 1 << 10,
        Tut11 = 1 << 11,
        Tut12 = 1 << 12,
        Tut13 = 1 << 13,
        Tut14 = 1 << 14,
        Tut15 = 1 << 15,
        Tut16 = 1 << 17,
        Tut17 = 1 << 18,
        Tut18 = 1 << 19,
        Tut19 = 1 << 20,
        Tut20 = 1 << 21
    }
}