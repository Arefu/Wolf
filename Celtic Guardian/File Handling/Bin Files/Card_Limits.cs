using System.Collections.Generic;
using System.IO;

namespace Celtic_Guardian.File_Handling.Bin_Files
{
    public class Card_Limits : File_Data
    {
        public Card_Limits()
        {
            Forbidden = new HashSet<short>();
            Limited = new HashSet<short>();
            SemiLimited = new HashSet<short>();
        }

        public HashSet<short> Forbidden { get; }
        public HashSet<short> Limited { get; }
        public HashSet<short> SemiLimited { get; }

        public override void Load(BinaryReader reader, long length)
        {
            ReadCardIds(reader, Forbidden);
            ReadCardIds(reader, Limited);
            ReadCardIds(reader, SemiLimited);
        }

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