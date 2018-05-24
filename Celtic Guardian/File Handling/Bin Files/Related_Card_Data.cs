using System.Collections.Generic;
using System.IO;

namespace Celtic_Guardian.Bin_Files
{
    public class Related_Card_Data : File_Data
    {
        public List<List<Item>> Items { get; private set; }

        public Related_Card_Data()
        {
            Items = new List<List<Item>>();
        }

        public override void Load(BinaryReader reader, long length)
        {
            Clear();
            int numCards = Constants.NumCards;

            long dataStart = reader.BaseStream.Position + (numCards * 8);

            for (int i = 0; i < numCards; i++)
            {
                uint shortoffset = reader.ReadUInt32();
                uint tagCount = reader.ReadUInt32();

                long tempOffset = reader.BaseStream.Position;

                long start = dataStart + (shortoffset * 4);
                reader.BaseStream.Position = start;

                List<Item> items = new List<Item>();
                for (int j = 0; j < tagCount; j++)
                {
                    items.Add(new Item(reader.ReadUInt16(), reader.ReadUInt16()));
                }
                Items.Add(items);

                reader.BaseStream.Position = tempOffset;
            }
        }

        public override void Save(BinaryWriter writer)
        {
            uint shortOffset = 0;
            for (int i = 0; i < Items.Count; i++)
            {
                writer.Write(shortOffset);
                writer.Write(Items[i].Count);
                shortOffset += (uint)Items[i].Count + 1;
            }

            for (int i = 0; i < Items.Count; i++)
            {
                foreach (Item item in Items[i])
                {
                    writer.Write(item.CardId);
                    writer.Write(item.TagIndex);
                }
                writer.Write(0);
            }
        }

        public override void Clear()
        {
            Items.Clear();
        }

        public class Item
        {
            public ushort CardId { get; set; }
            public ushort TagIndex { get; set; }

            public Item(ushort cardId, ushort tagIndex)
            {
                CardId = cardId;
                TagIndex = tagIndex;
            }

        }
    }
}