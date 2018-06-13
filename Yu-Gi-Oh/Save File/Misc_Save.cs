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

            for (var i = 0; i < Challenges.Length; i++) Challenges[i] = DeulistChallengeState.Locked;

            for (var i = 0; i < UnlockedRecipes.Length; i++) UnlockedRecipes[i] = false;

            CompleteTutorials = CompleteTutorials.None;
            UnlockedContent = UnlockedContent.None;
            UnlockedShopPacks = UnlockedShopPacks.None;
            UnlockedBattlePacks = UnlockedBattlePacks.None;
        }

        /// <summary>
        ///     Loads the save.
        /// </summary>
        /// <param name="reader">New instance of BinaryReader loading the Save File.</param>
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

        /// <summary>
        ///     Writes the save back to the file..
        /// </summary>
        /// <param name="writer">New instance of BinaryWriter for writing the Save File.</param>
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
}