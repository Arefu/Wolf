using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Main_Files
{
    public class Duel_Data : File_Data
    {
        private static readonly Encoding encoding1 = Encoding.ASCII;
        private static readonly Encoding encoding2 = Encoding.Unicode;

        public Duel_Data()
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
                var displayIndex = reader.ReadInt32();
                var playerCharId = reader.ReadInt32();
                var opponentCharId = reader.ReadInt32();
                var playerDeckId = reader.ReadInt32();
                var opponentDeckId = reader.ReadInt32();
                var arenaId = reader.ReadInt32();
                var unk8 = reader.ReadInt32();
                var dlcId = reader.ReadInt32();
                var codeNameOffset = reader.ReadInt64();
                var playerAlternateSkinOffset = reader.ReadInt64();
                var opponentAlternateSkinOffset = reader.ReadInt64();
                var nameOffset = reader.ReadInt64();
                var descriptionOffset = reader.ReadInt64();
                var tipOffset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + codeNameOffset;
                var codeName = reader.ReadNullTerminatedString(encoding1);

                reader.BaseStream.Position = fileStartPos + playerAlternateSkinOffset;
                var playerAlternateSkin = reader.ReadNullTerminatedString(encoding1);

                reader.BaseStream.Position = fileStartPos + opponentAlternateSkinOffset;
                var opponentAlternateSkin = reader.ReadNullTerminatedString(encoding1);

                reader.BaseStream.Position = fileStartPos + nameOffset;
                var name = reader.ReadNullTerminatedString(encoding2);

                reader.BaseStream.Position = fileStartPos + descriptionOffset;
                var description = reader.ReadNullTerminatedString(encoding2);

                reader.BaseStream.Position = fileStartPos + tipOffset;
                var tipStr = reader.ReadNullTerminatedString(encoding2);

                reader.BaseStream.Position = tempOffset;

                if (!Items.TryGetValue(id, out var item))
                {
                    item = new Item(id, series, displayIndex, playerCharId, opponentCharId, playerDeckId,
                        opponentDeckId, arenaId, unk8, dlcId);
                    Items.Add(item.Id, item);
                }

                item.CodeName.SetText(language, codeName);
                item.PlayerAlternateSkin.SetText(language, playerAlternateSkin);
                item.OpponentAlternateSkin.SetText(language, opponentAlternateSkin);
                item.Name.SetText(language, name);
                item.Description.SetText(language, description);
                item.Tip.SetText(language, tipStr);
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            const int firstChunkItemSize = 88;
            var fileStartPos = writer.BaseStream.Position;

            writer.Write((ulong) Items.Count);

            var offsetsOffset = writer.BaseStream.Position;
            writer.Write(new byte[Items.Count * firstChunkItemSize]);

            var index = 0;
            foreach (var item in Items.Values)
            {
                var codeNameLen = GetStringSize(item.CodeName.GetText(language), encoding1);
                var playerAlternateSkinLen = GetStringSize(item.PlayerAlternateSkin.GetText(language), encoding1);
                var opponentAlternateSkinLen = GetStringSize(item.OpponentAlternateSkin.GetText(language), encoding1);
                var nameLen = GetStringSize(item.Name.GetText(language), encoding2);
                var descriptionLen = GetStringSize(item.Description.GetText(language), encoding2);
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + index * firstChunkItemSize;
                writer.Write(item.Id);
                writer.Write((int) item.Series);
                writer.Write(item.DisplayIndex);
                writer.Write(item.PlayerCharId);
                writer.Write(item.OpponentCharId);
                writer.Write(item.PlayerDeckId);
                writer.Write(item.OpponentDeckId);
                writer.Write(item.ArenaId);
                writer.Write(item.Unk8);
                writer.Write(item.DlcId);
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.WriteOffset(fileStartPos, tempOffset + codeNameLen);
                writer.WriteOffset(fileStartPos, tempOffset + codeNameLen + playerAlternateSkinLen);
                writer.WriteOffset(fileStartPos,
                    tempOffset + codeNameLen + playerAlternateSkinLen + opponentAlternateSkinLen);
                writer.WriteOffset(fileStartPos,
                    tempOffset + codeNameLen + playerAlternateSkinLen + opponentAlternateSkinLen + nameLen);
                writer.WriteOffset(fileStartPos,
                    tempOffset + codeNameLen + playerAlternateSkinLen + opponentAlternateSkinLen + nameLen +
                    descriptionLen);
                writer.BaseStream.Position = tempOffset;

                writer.WriteNullTerminatedString(item.CodeName.GetText(language), encoding1);
                writer.WriteNullTerminatedString(item.PlayerAlternateSkin.GetText(language), encoding1);
                writer.WriteNullTerminatedString(item.OpponentAlternateSkin.GetText(language), encoding1);
                writer.WriteNullTerminatedString(item.Name.GetText(language), encoding2);
                writer.WriteNullTerminatedString(item.Description.GetText(language), encoding2);
                writer.WriteNullTerminatedString(item.Tip.GetText(language), encoding2);

                index++;
            }
        }

        public class Item
        {
            public Item(int id, Duel_Series series, int displayIndex, int playerCharId, int opponentCharId,
                int playerDeckId, int opponentDeckId, int duelArena, int unk8, int dlcId)
            {
                Id = id;
                Series = series;
                DisplayIndex = displayIndex;
                PlayerCharId = playerCharId;
                OpponentCharId = opponentCharId;
                PlayerDeckId = playerDeckId;
                OpponentDeckId = opponentDeckId;
                ArenaId = duelArena;
                Unk8 = unk8;
                DlcId = dlcId;
                CodeName = new Localized_Text();
                PlayerAlternateSkin = new Localized_Text();
                OpponentAlternateSkin = new Localized_Text();
                Name = new Localized_Text();
                Description = new Localized_Text();
                Tip = new Localized_Text();
            }

            public int Id { get; set; }

            public Duel_Series Series { get; set; }

            public int DisplayIndex { get; set; }

            public int PlayerCharId { get; set; }

            public int OpponentCharId { get; set; }

            public int PlayerDeckId { get; set; }

            public int OpponentDeckId { get; set; }

            public int ArenaId { get; set; }

            public int Unk8 { get; set; }
            public int DlcId { get; set; }

            public Localized_Text CodeName { get; set; }

            public Localized_Text PlayerAlternateSkin { get; set; }

            public Localized_Text OpponentAlternateSkin { get; set; }

            public Localized_Text Name { get; set; }

            public Localized_Text Description { get; set; }

            public Localized_Text Tip { get; set; }

            public override string ToString()
            {
                return "id: " + Id +
                       " series: " + Series + " displayIndex: " + DisplayIndex +
                       " playerCharId: " + PlayerCharId + " opponentCharId: " + OpponentCharId +
                       " playerDeckId: " + PlayerDeckId + " opponentDeckId: " + OpponentDeckId +
                       " arenaId: " + ArenaId + " unk8: " + Unk8 + " dlcId: " + DlcId +
                       " codeName: '" + CodeName + "' playerAlternateSkin: '" + PlayerAlternateSkin +
                       "' opponentAlternateSkin: '" + OpponentAlternateSkin + "' name: '" + Name +
                       "' unkStr5: '" + Description + "' tip: '" + Tip + "'";
            }
        }
    }
}