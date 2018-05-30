using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Main_Files
{
    public class Arena_Data : File_Data
    {
        private static readonly Encoding keyEncoding = Encoding.ASCII;
        private static readonly Encoding valueEncoding = Encoding.Unicode;
        private static readonly Encoding value2Encoding = Encoding.Unicode;

        public Arena_Data()
        {
            Items = new Dictionary<int, Item>();
        }

        public Dictionary<int, Item> Items { get; }
        public override bool IsLocalized => true;

        public override void Load(BinaryReader reader, long length, Localized_Text.Language language)
        {
            var fileStartPos = reader.BaseStream.Position;

            var count = (uint) reader.ReadUInt64();
            for (uint i = 0; i < count; i++)
            {
                var id = reader.ReadInt32();
                var keyOffset = reader.ReadInt64();
                var valueOffset = reader.ReadInt64();
                var value2Offset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + keyOffset;
                var key = reader.ReadNullTerminatedString(keyEncoding);

                reader.BaseStream.Position = fileStartPos + valueOffset;
                var value = reader.ReadNullTerminatedString(valueEncoding);

                reader.BaseStream.Position = fileStartPos + value2Offset;
                var value2 = reader.ReadNullTerminatedString(valueEncoding);

                reader.BaseStream.Position = tempOffset;

                if (!Items.TryGetValue(id, out var item))
                {
                    item = new Item(id);
                    Items.Add(item.Id, item);
                }

                item.Key.SetText(language, key);
                item.Value.SetText(language, value);
                item.Value2.SetText(language, value2);
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            const int firstChunkItemSize = 28; // Size of each item in the first chunk
            var fileStartPos = writer.BaseStream.Position;

            writer.Write((ulong) Items.Count);

            var offsetsOffset = writer.BaseStream.Position;
            writer.Write(new byte[Items.Count * firstChunkItemSize]);

            var index = 0;
            foreach (var item in Items.Values)
            {
                var keyLen = GetStringSize(item.Key.GetText(language), keyEncoding);
                var valueLen = GetStringSize(item.Value.GetText(language), valueEncoding);
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + index * firstChunkItemSize;
                writer.Write(item.Id);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + keyLen);
                writer.WriteOffset(fileStartPos, tempOffset + keyLen + valueLen);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(item.Key.GetText(language), keyEncoding);
                writer.WriteNullTerminatedString(item.Value.GetText(language), valueEncoding);
                writer.WriteNullTerminatedString(item.Value2.GetText(language), value2Encoding);

                index++;
            }
        }

        public class Item
        {
            public Item(int id)
            {
                Id = id;
                Key = new Localized_Text();
                Value = new Localized_Text();
                Value2 = new Localized_Text();
            }

            public int Id { get; set; }
            public Localized_Text Key { get; set; }
            public Localized_Text Value { get; set; }
            public Localized_Text Value2 { get; set; }

            public override string ToString()
            {
                return "id: " + Id + " key: '" + Key + "' value: '" + Value + "' value2: '" + Value2 + "'";
            }
        }
    }
}