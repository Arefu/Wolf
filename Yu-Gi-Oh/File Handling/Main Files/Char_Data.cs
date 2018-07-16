using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Main_Files
{
    public class Char_Data : File_Data
    {
        private static readonly Encoding keyEncoding = Encoding.ASCII;
        private static readonly Encoding valueEncoding = Encoding.Unicode;
        private static readonly Encoding descriptionEncoding = Encoding.Unicode;

        public Char_Data()
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
                var challengeDeckId = reader.ReadInt32();
                var unk3 = reader.ReadInt32();
                var dlcId = reader.ReadInt32();
                var unk5 = reader.ReadInt32();
                var type = reader.ReadInt64();
                var keyOffset = reader.ReadInt64();
                var valueOffset = reader.ReadInt64();
                var descriptionOffset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + keyOffset;
                var codeName = reader.ReadNullTerminatedString(keyEncoding);

                reader.BaseStream.Position = fileStartPos + valueOffset;
                var name = reader.ReadNullTerminatedString(valueEncoding);

                reader.BaseStream.Position = fileStartPos + descriptionOffset;
                var bio = reader.ReadNullTerminatedString(valueEncoding);

                reader.BaseStream.Position = tempOffset;

                if (!Items.TryGetValue(id, out var item))
                {
                    item = new Item(id, series, challengeDeckId, unk3, dlcId, unk5, type);
                    Items.Add(item.Id, item);
                }

                item.CodeName.SetText(language, codeName);
                item.Name.SetText(language, name);
                item.Bio.SetText(language, bio);
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            const int firstChunkItemSize = 56; // Size of each item in the first chunk
            var fileStartPos = writer.BaseStream.Position;

            writer.Write((ulong) Items.Count);

            var offsetsOffset = writer.BaseStream.Position;
            writer.Write(new byte[Items.Count * firstChunkItemSize]);

            var index = 0;
            foreach (var item in Items.Values)
            {
                var keyLen = GetStringSize(item.CodeName.GetText(language), keyEncoding);
                var valueLen = GetStringSize(item.Name.GetText(language), valueEncoding);
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + index * firstChunkItemSize;
                writer.Write(item.Id);
                writer.Write((int) item.Series);
                writer.Write(item.ChallengeDeckId);
                writer.Write(item.Unk3);
                writer.Write(item.DlcId);
                writer.Write(item.Unk5);
                writer.Write(item.Type);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + keyLen);
                writer.WriteOffset(fileStartPos, tempOffset + keyLen + valueLen);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(item.CodeName.GetText(language), keyEncoding);
                writer.WriteNullTerminatedString(item.Name.GetText(language), valueEncoding);
                writer.WriteNullTerminatedString(item.Bio.GetText(language), descriptionEncoding);

                index++;
            }
        }

        public override void Clear()
        {
            Items.Clear();
        }

        public class Item
        {
            public Item(int id, Duel_Series series, int challengeDeckId, int unk3, int dlcId, int unk5, long type)
            {
                Id = id;
                Series = series;
                ChallengeDeckId = challengeDeckId;
                Unk3 = unk3;
                DlcId = dlcId;
                Unk5 = unk5;
                Type = type;
                CodeName = new Localized_Text();
                Name = new Localized_Text();
                Bio = new Localized_Text();
            }

            public int Id { get; set; }
            public Duel_Series Series { get; set; }
            public int ChallengeDeckId { get; set; }
            public int Unk3 { get; set; }
            public int DlcId { get; set; }
            public int Unk5 { get; set; }
            public long Type { get; set; }

            public Localized_Text CodeName { get; set; }
            public Localized_Text Name { get; set; }
            public Localized_Text Bio { get; set; }

            public override string ToString()
            {
                return "id: " + Id + " series: " + Series + " challengeDeckId: " + ChallengeDeckId + " unk3: " + Unk3 +
                       " dlcId: " + DlcId + " unk5: " + Unk5 +
                       " type: " + Type + " codeName: '" + CodeName + "' name: '" + Name + "' bio: '" + Bio + "'";
            }
        }
    }
}