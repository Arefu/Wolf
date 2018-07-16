using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Main_Files
{
    public class Deck_Data : File_Data
    {
        private static readonly Encoding deckFileNameEncoding = Encoding.ASCII;
        private static readonly Encoding deckNameEncoding = Encoding.Unicode;
        private static readonly Encoding deckDescriptionEncoding = Encoding.Unicode;
        private static readonly Encoding unkStr1Encoding = Encoding.Unicode;

        public Deck_Data()
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
                var id1 = reader.ReadInt32();
                var id2 = reader.ReadInt32();
                var series = (Duel_Series) reader.ReadInt32();
                var signatureCardId = reader.ReadInt32();
                var deckOwner = reader.ReadInt32();
                var unk1 = reader.ReadInt32();
                var deckFileNameOffset = reader.ReadInt64();
                var deckNameOffset = reader.ReadInt64();
                var deckDescriptionOffset = reader.ReadInt64();
                var unkStr1Offset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + deckFileNameOffset;
                var deckFileName = reader.ReadNullTerminatedString(deckFileNameEncoding);

                reader.BaseStream.Position = fileStartPos + deckNameOffset;
                var deckName = reader.ReadNullTerminatedString(deckNameEncoding);

                reader.BaseStream.Position = fileStartPos + deckDescriptionOffset;
                var deckDescription = reader.ReadNullTerminatedString(deckDescriptionEncoding);

                reader.BaseStream.Position = fileStartPos + unkStr1Offset;
                var unkStr1 = reader.ReadNullTerminatedString(unkStr1Encoding);

                reader.BaseStream.Position = tempOffset;

                if (!Items.TryGetValue(id1, out var item))
                {
                    item = new Item(id1, id2, series, signatureCardId, deckOwner, unk1);
                    Items.Add(item.Id1, item);
                }

                item.DeckFileName.SetText(language, deckFileName);
                item.DeckName.SetText(language, deckName);
                item.DeckDescription.SetText(language, deckDescription);
                item.UnkStr1.SetText(language, unkStr1);
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
                var deckFileNameLen = GetStringSize(item.DeckFileName.GetText(language), deckFileNameEncoding);
                var deckNameLen = GetStringSize(item.DeckName.GetText(language), deckNameEncoding);
                var deckDescriptionLen = GetStringSize(item.DeckDescription.GetText(language), deckDescriptionEncoding);
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + index * firstChunkItemSize;
                writer.Write(item.Id1);
                writer.Write(item.Id2);
                writer.Write((int) item.Series);
                writer.Write(item.SignatureCardId);
                writer.Write(item.DeckOwnerId);
                writer.Write(item.Unk1);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + deckFileNameLen);
                writer.WriteOffset(fileStartPos, tempOffset + deckFileNameLen + deckNameLen);
                writer.WriteOffset(fileStartPos, tempOffset + deckFileNameLen + deckNameLen + deckDescriptionLen);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(item.DeckFileName.GetText(language), deckFileNameEncoding);
                writer.WriteNullTerminatedString(item.DeckName.GetText(language), deckNameEncoding);
                writer.WriteNullTerminatedString(item.DeckDescription.GetText(language), deckDescriptionEncoding);
                writer.WriteNullTerminatedString(item.UnkStr1.GetText(language), unkStr1Encoding);

                index++;
            }
        }

        public override void Clear()
        {
            Items.Clear();
        }

        public class Item
        {
            public Item(int id1, int id2, Duel_Series series, int signatureCardId, int deckOwner, int unk1)
            {
                Id1 = id1;
                Id2 = id2;
                Series = series;
                SignatureCardId = signatureCardId;
                DeckOwnerId = deckOwner;
                Unk1 = unk1;
                DeckFileName = new Localized_Text();
                DeckName = new Localized_Text();
                DeckDescription = new Localized_Text();
                UnkStr1 = new Localized_Text();
            }

            public int Id1 { get; set; }
            public int Id2 { get; set; }
            public Duel_Series Series { get; set; }
            public int SignatureCardId { get; set; }

            public int DeckOwnerId { get; set; }

            public int Unk1 { get; set; }
            public Localized_Text DeckFileName { get; set; }
            public Localized_Text DeckName { get; set; }
            public Localized_Text DeckDescription { get; set; }
            public Localized_Text UnkStr1 { get; set; }

            public override string ToString()
            {
                return "id1: " + Id1 + " id2: " + Id1 + " signatureCard: " + SignatureCardId + " deckOwner: " +
                       DeckOwnerId + " unk1: " + Unk1 +
                       " deckFileName: '" + DeckFileName + "' deckName: '" + DeckName + "' unk4: '" + DeckDescription +
                       "' unkStr1: '" + UnkStr1 + "'";
            }

            public Char_Data.Item GetDeckOwner(Char_Data charData)
            {
                charData.Items.TryGetValue(DeckOwnerId, out var owner);
                return owner;
            }
        }
    }
}