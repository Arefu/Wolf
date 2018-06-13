using System.Collections.Generic;
using System.IO;

namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    /// <summary>
    ///     This class handles loading of pd_limits.bin.
    ///     This file contains the information of Forbidden, Limited, and Semi-Limted cards that are in use in the game.
    /// </summary>
    public class Card_Limits : File_Data
    {
        /// <summary>
        ///     Default Constructor For Card_Limits.
        /// </summary>
        public Card_Limits()
        {
            Forbidden = new HashSet<short>();
            Limited = new HashSet<short>();
            SemiLimited = new HashSet<short>();
        }

        /// <summary>
        ///     List of card IDs that are Forbidden Cards.
        /// </summary>
        public HashSet<short> Forbidden { get; }

        /// <summary>
        ///     List of card IDs that are Limited Cards.
        /// </summary>
        public HashSet<short> Limited { get; }

        /// <summary>
        ///     List of card IDs that are Semi-Limited Cards.
        /// </summary>
        public HashSet<short> SemiLimited { get; }

        /// <summary>
        ///     This handles reading of card IDs from pd_limits.bin.
        /// </summary>
        /// <param name="reader">A new instance of a BinaryReader which has pd_limits.bin as it's loaded file.</param>
        /// <param name="length">Not used.</param>
        public override void Load(BinaryReader reader, long length)
        {
            ReadCardIds(reader, Forbidden);
            ReadCardIds(reader, Limited);
            ReadCardIds(reader, SemiLimited);
        }

        /// <summary>
        ///     This handles writing of card IDs to pd_limits.bin.
        /// </summary>
        /// <param name="writer">A new instance of a BinaryWriter which has pd_limits.bin as it's loaded file.</param>
        public override void Save(BinaryWriter writer)
        {
            WriteCardIds(writer, Forbidden);
            WriteCardIds(writer, Limited);
            WriteCardIds(writer, SemiLimited);
        }

        private static void ReadCardIds(BinaryReader reader, ISet<short> cardIds)
        {
            cardIds.Clear();

            var count = reader.ReadInt16();
            for (var i = 0; i < count; i++) cardIds.Add(reader.ReadInt16());
        }

        private static void WriteCardIds(BinaryWriter writer, IReadOnlyCollection<short> cardIds)
        {
            writer.Write((short) cardIds.Count);
            foreach (var cardId in cardIds) writer.Write(cardId);
        }
    }
}