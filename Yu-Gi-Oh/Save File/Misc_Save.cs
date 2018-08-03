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

        /// <summary>
        ///     Duel Points the user has.
        /// </summary>
        public int DuelPoints { get; set; }

        /// <summary>
        ///     What Challenges the user has completed.
        /// </summary>
        public DeulistChallengeState[] Challenges { get; }

        /// <summary>
        ///     What Recipes the user has completed.
        /// </summary>
        public bool[] UnlockedRecipes { get; set; }

        /// <summary>
        ///     What Avatars the user has completed.
        /// </summary>
        public bool[] UnlockedAvatars { get; set; }

        /// <summary>
        ///     The Tutorials the user has completed.
        /// </summary>
        public CompleteTutorials CompleteTutorials { get; set; }

        /// <summary>
        ///     What unlocked the user has.
        /// </summary>
        public UnlockedContent UnlockedContent { get; set; }

        /// <summary>
        ///     The Shop Packs the user has unlocked.
        /// </summary>
        public UnlockedShopPacks UnlockedShopPacks { get; set; }

        /// <summary>
        ///     The Battle Packs the user has unlocked.
        /// </summary>
        public UnlockedBattlePacks UnlockedBattlePacks { get; set; }

        /// <summary>
        ///     Resets the save completly.
        /// </summary>
        public override void Clear()
        {
            DuelPoints = 1000;

            for (var Count = 0; Count < Challenges.Length; Count++) Challenges[Count] = DeulistChallengeState.Locked;

            for (var Count = 0; Count < UnlockedRecipes.Length; Count++) UnlockedRecipes[Count] = false;

            CompleteTutorials = CompleteTutorials.None;
            UnlockedContent = UnlockedContent.None;
            UnlockedShopPacks = UnlockedShopPacks.None;
            UnlockedBattlePacks = UnlockedBattlePacks.None;
        }

        /// <summary>
        ///     Loads the save.
        /// </summary>
        /// <param name="Reader">New instance of BinaryReader loading the Save File.</param>
        public override void Load(BinaryReader Reader)
        {
            Reader.ReadBytes(16);

            DuelPoints = (int) Reader.ReadInt64();

            var UnlockedAvatarsBuffer = Reader.ReadBytes(32);
            for (var Counter = 0; Counter < UnlockedAvatars.Length; Counter++)
            {
                var ByteIndex = Counter / 8;
                var BitIndex = Counter % 8;
                UnlockedAvatars[Counter] = (UnlockedAvatarsBuffer[ByteIndex] & (byte) (1 << BitIndex)) != 0;
            }

            for (var Count = 0; Count < Constants.NumDeckDataSlots; Count++)
                Challenges[Count] = (DeulistChallengeState) Reader.ReadInt32();

            var UnlockedRecipesBuffer = Reader.ReadBytes(60);
            for (var Counter = 0; Counter < Constants.NumDeckDataSlots; Counter++)
            {
                var ByteIndex = Counter / 8;
                var BitIndex = Counter % 8;
                UnlockedRecipes[Counter] = (UnlockedRecipesBuffer[ByteIndex] & (byte) (1 << BitIndex)) != 0;
            }

            UnlockedShopPacks = (UnlockedShopPacks) Reader.ReadUInt32();
            UnlockedBattlePacks = (UnlockedBattlePacks) Reader.ReadUInt32();
            Reader.ReadBytes(8);

            CompleteTutorials = (CompleteTutorials) Reader.ReadInt32();
            UnlockedContent = (UnlockedContent) Reader.ReadInt32();
        }

        /// <summary>
        ///     Writes the save back to the file..
        /// </summary>
        /// <param name="Writer">New instance of BinaryWriter for writing the Save File.</param>
        public override void Save(BinaryWriter Writer)
        {
            Writer.Write(new byte[16]);
            Writer.Write((long) DuelPoints);
            var UnlockedAvatarsBuffer = new byte[32];
            for (var Counter = 0; Counter < UnlockedAvatars.Length; Counter++)
            {
                if (!UnlockedAvatars[Counter]) continue;

                var ByteIndex = Counter / 8;
                var BitIndex = Counter % 8;
                UnlockedAvatarsBuffer[ByteIndex] |= (byte) (1 << BitIndex);
            }

            Writer.Write(UnlockedAvatarsBuffer);

            for (var Counter = 0; Counter < Constants.NumDeckDataSlots; Counter++)
                Writer.Write((int) Challenges[Counter]);

            var UnlockedRecipesBuffer = new byte[60];
            for (var Counter = 0; Counter < UnlockedRecipes.Length; Counter++)
            {
                if (!UnlockedRecipes[Counter]) continue;

                var ByteIndex = Counter / 8;
                var BitIndex = Counter % 8;
                UnlockedRecipesBuffer[ByteIndex] |= (byte) (1 << BitIndex);
            }

            Writer.Write(UnlockedRecipesBuffer);

            Writer.Write((uint) UnlockedShopPacks);
            Writer.Write((uint) UnlockedBattlePacks);
            Writer.Write(new byte[8]);

            Writer.Write((int) CompleteTutorials);
            Writer.Write((int) UnlockedContent);
        }
    }
}