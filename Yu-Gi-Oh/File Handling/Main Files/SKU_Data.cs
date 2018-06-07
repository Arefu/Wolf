using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Main_Files
{
    public class SKU_Data : File_Data
    {
        private static readonly Encoding keyEncoding = Encoding.ASCII;
        private static readonly Encoding valueEncoding = Encoding.Unicode;

        public SKU_Data()
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
                var series = (Duel_Series) reader.ReadInt32();
                var keyOffset = reader.ReadInt64();
                var valueOffset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + keyOffset;
                var key = reader.ReadNullTerminatedString(keyEncoding);

                reader.BaseStream.Position = fileStartPos + valueOffset;
                var value = reader.ReadNullTerminatedString(valueEncoding);

                reader.BaseStream.Position = tempOffset;

                if (!Items.TryGetValue(id, out var item))
                {
                    item = new Item(id, series);
                    Items.Add(item.Id, item);
                }

                item.Key.SetText(language, key);
                item.Value.SetText(language, value);
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            const int firstChunkItemSize = 24;
            var fileStartPos = writer.BaseStream.Position;

            writer.Write((ulong) Items.Count);

            var offsetsOffset = writer.BaseStream.Position;
            writer.Write(new byte[Items.Count * firstChunkItemSize]);

            var index = 0;
            foreach (var item in Items.Values)
            {
                var keyLen = GetStringSize(item.Key.GetText(language), keyEncoding);
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + index * firstChunkItemSize;
                writer.Write(item.Id);
                writer.Write((int) item.Series);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + keyLen);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(item.Key.GetText(language), keyEncoding);
                writer.WriteNullTerminatedString(item.Value.GetText(language), valueEncoding);

                index++;
            }
        }


        public class Item
        {
            public Item(int id, Duel_Series series)
            {
                Id = id;
                Series = series;
                Key = new Localized_Text();
                Value = new Localized_Text();
            }

            public int Id { get; set; }
            public Duel_Series Series { get; set; }
            public Localized_Text Key { get; set; }
            public Localized_Text Value { get; set; }

            public override string ToString()
            {
                return "id: " + Id + " series: " + Series + " key: '" + Key + "' value: '" + Value + "'";
            }
        }
    }
}