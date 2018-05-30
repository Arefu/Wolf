using System.Collections.Generic;
using System.IO;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Bin_Files
{
    public class Related_Card_Data : File_Data
    {
        public Related_Card_Data()
        {
            Items = new List<List<Item>>();
        }

        public List<List<Item>> Items { get; }

        public override void Load(BinaryReader reader, long length)
        {
            Clear();

            var dataStart = reader.BaseStream.Position + Constants.NumCards * 8;

            for (var i = 0; i < Constants.NumCards; i++)
            {
                var shortoffset = reader.ReadUInt32();
                var tagCount = reader.ReadUInt32();

                var tempOffset = reader.BaseStream.Position;

                var start = dataStart + shortoffset * 4;
                reader.BaseStream.Position = start;

                var items = new List<Item>();
                for (var j = 0; j < tagCount; j++) items.Add(new Item(reader.ReadUInt16(), reader.ReadUInt16()));
                Items.Add(items);

                reader.BaseStream.Position = tempOffset;
            }
        }

        public override void Save(BinaryWriter writer)
        {
            uint shortOffset = 0;
            foreach (var Item in Items)
            {
                writer.Write(shortOffset);
                writer.Write(Item.Count);
                shortOffset += (uint) Item.Count + 1;
            }

            foreach (var Item in Items)
            {
                foreach (var item in Item)
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
            public Item(ushort cardId, ushort tagIndex)
            {
                CardId = cardId;
                TagIndex = tagIndex;
            }

            public ushort CardId { get; set; }
            public ushort TagIndex { get; set; }
        }
    }
}