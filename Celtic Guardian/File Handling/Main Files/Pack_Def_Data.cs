using Celtic_Guardian.Utility;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Celtic_Guardian.Main_Files
{
    public class Pack_Def_Data : File_Data
    {
        static Encoding encoding1 = Encoding.ASCII;
        static Encoding encoding2 = Encoding.Unicode;
        public Dictionary<int, Item> Items { get; private set; }

        public override bool IsLocalized
        {
            get { return true; }
        }

        public Pack_Def_Data()
        {
            Items = new Dictionary<int, Item>();
        }

        public override void Load(BinaryReader reader, long length, Localized_Text.Language language)
        {
            long fileStartPos = reader.BaseStream.Position;

            uint count = (uint)reader.ReadUInt64();
            for (uint i = 0; i < count; i++)
            {
                int id = reader.ReadInt32();
                Duel_Series series = (Duel_Series)reader.ReadInt32();
                int price = reader.ReadInt32();
                PackType type = (PackType)reader.ReadInt32();
                long codeNameOffset = reader.ReadInt64();
                long nameOffset = reader.ReadInt64();
                long unkStrOffset = reader.ReadInt64();

                long tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + codeNameOffset;
                string codeName = reader.ReadNullTerminatedString(encoding1);

                reader.BaseStream.Position = fileStartPos + nameOffset;
                string name = reader.ReadNullTerminatedString(encoding2);

                reader.BaseStream.Position = fileStartPos + unkStrOffset;
                string unkStr = reader.ReadNullTerminatedString(encoding2);

                reader.BaseStream.Position = tempOffset;

                Item item;
                if (!Items.TryGetValue(id, out item))
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
            int firstChunkItemSize = 40;// Size of each item in the first chunk
            long fileStartPos = writer.BaseStream.Position;

            writer.Write((ulong)Items.Count);

            long offsetsOffset = writer.BaseStream.Position;
            writer.Write(new byte[Items.Count * firstChunkItemSize]);

            int index = 0;
            foreach (Item item in Items.Values)
            {
                int codeNameLen = GetStringSize(item.CodeName.GetText(language), encoding1);
                int nameLen = GetStringSize(item.Name.GetText(language), encoding2);
                long tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + (index * firstChunkItemSize);
                writer.Write(item.Id);
                writer.Write((int)item.Series);
                writer.Write(item.Price);
                writer.Write((int)item.Type);
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
            public int Id { get; set; }

            /// <summary>
            /// The series this pack belongs to (Yu-Gi-Oh!, GX, 5D's, ZEXAL, ARC-V)
            /// </summary>
            public Duel_Series Series { get; set; }

            /// <summary>
            /// The price of this pack.
            /// </summary>
            public int Price { get; set; }

            /// <summary>
            /// Pack type - 82 (regular) / 66 (battle packs)
            /// </summary>
            public PackType Type { get; set; }

            /// <summary>
            /// The short form code name of the pack (this maps to /packs/reward_wrap_XXXX.png - unclear if you can add new ones).
            /// </summary>
            public Localized_Text CodeName { get; set; }

            /// <summary>
            /// The name for the pack (this is the character for regular packs, battle pack name for battle packs)
            /// </summary>
            public Localized_Text Name { get; set; }

            /// <summary>
            /// Unknown - always an empty string
            /// </summary>
            public Localized_Text UnkStr { get; set; }

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

            public override string ToString()
            {
                return "id: " + Id + " series: " + Series + " price: " + Price + " type: " + Type +
                   " codeName: '" + CodeName + "' name: '" + Name + "' unkStr: '" + UnkStr + "'";
            }
        }        
    }

    public enum PackType
    {
        Shop = 82,
        Battle = 66
    }
    }
}