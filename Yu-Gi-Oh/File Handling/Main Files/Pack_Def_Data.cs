using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Main_Files
{
    public class Pack_Def_Data : File_Data
    {
        private static readonly Encoding encoding1 = Encoding.ASCII;
        private static readonly Encoding encoding2 = Encoding.Unicode;

        public Pack_Def_Data()
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
                var price = reader.ReadInt32();
                var type = (PackType) reader.ReadInt32();
                var codeNameOffset = reader.ReadInt64();
                var nameOffset = reader.ReadInt64();
                var unkStrOffset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + codeNameOffset;
                var codeName = reader.ReadNullTerminatedString(encoding1);

                reader.BaseStream.Position = fileStartPos + nameOffset;
                var name = reader.ReadNullTerminatedString(encoding2);

                reader.BaseStream.Position = fileStartPos + unkStrOffset;
                var unkStr = reader.ReadNullTerminatedString(encoding2);

                reader.BaseStream.Position = tempOffset;

                if (!Items.TryGetValue(id, out var item))
                {
                    item = new Item(id, series, price, type);
                    Items.Add(item.Id, item);
                }

                item.CodeName.SetText(language, codeName);
                item.Name.SetText(language, name);
                item.UnkStr.SetText(language, unkStr);
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            const int firstChunkItemSize = 40;
            var fileStartPos = writer.BaseStream.Position;

            writer.Write((ulong) Items.Count);

            var offsetsOffset = writer.BaseStream.Position;
            writer.Write(new byte[Items.Count * firstChunkItemSize]);

            var index = 0;
            foreach (var item in Items.Values)
            {
                var codeNameLen = GetStringSize(item.CodeName.GetText(language), encoding1);
                var nameLen = GetStringSize(item.Name.GetText(language), encoding2);
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + index * firstChunkItemSize;
                writer.Write(item.Id);
                writer.Write((int) item.Series);
                writer.Write(item.Price);
                writer.Write((int) item.Type);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + codeNameLen);
                writer.WriteOffset(fileStartPos, tempOffset + codeNameLen + nameLen);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(item.CodeName.GetText(language), encoding1);
                writer.WriteNullTerminatedString(item.Name.GetText(language), encoding2);
                writer.WriteNullTerminatedString(item.UnkStr.GetText(language), encoding2);

                index++;
            }
        }

        public class Item
        {
            public Item(int id, Duel_Series pack, int price, PackType type)
            {
                Id = id;
                Series = pack;
                Price = price;
                Type = type;
                CodeName = new Localized_Text();
                Name = new Localized_Text();
                UnkStr = new Localized_Text();
            }

            public int Id { get; set; }

            public Duel_Series Series { get; set; }

            public int Price { get; set; }

            public PackType Type { get; set; }

            public Localized_Text CodeName { get; set; }

            public Localized_Text Name { get; set; }

            public Localized_Text UnkStr { get; set; }

            public override string ToString()
            {
                return "id: " + Id + " series: " + Series + " price: " + Price + " type: " + Type +
                       " codeName: '" + CodeName + "' name: '" + Name + "' unkStr: '" + UnkStr + "'";
            }
        }
    }
}