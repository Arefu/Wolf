using System.Collections.Generic;
using System.IO;

namespace Celtic_Guardian.Bin_Files
{
    public class Card_Limits : File_Data
    {
        public HashSet<short> Forbidden { get; private set; }
        public HashSet<short> Limited { get; private set; }
        public HashSet<short> SemiLimited { get; private set; }

        public Card_Limits()
        {
            Forbidden = new HashSet<short>();
            Limited = new HashSet<short>();
            SemiLimited = new HashSet<short>();
        }

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

        private void ReadCardIds(BinaryReader reader, HashSet<short> cardIds)
        {
            cardIds.Clear();

            short count = reader.ReadInt16();
            for (int i = 0; i < count; i++)
            {
                cardIds.Add(reader.ReadInt16());
            }
        }

        private void WriteCardIds(BinaryWriter writer, HashSet<short> cardIds)
        {
            writer.Write((short)cardIds.Count);
            foreach (ushort cardId in cardIds)
            {
                writer.Write(cardId);
            }
        }
    }
}