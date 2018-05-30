using System.Collections.Generic;
using System.IO;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Pack_Files
{
    public class Battle_Pack : File_Data
    {
        public Battle_Pack()
        {
            Categories = new List<CardCollection>();
        }

        public List<CardCollection> Categories { get; }

        public override void Load(BinaryReader reader, long length)
        {
            Categories.Clear();
            var fileStartPos = reader.BaseStream.Position;

            var numCategories = reader.ReadInt64();
            for (var i = 0; i < numCategories; i++)
            {
                var cardListOffset = reader.ReadInt64();

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + cardListOffset;
                var cardCount = reader.ReadInt16();
                var cardCollection = new CardCollection();
                for (var j = 0; j < cardCount; j++) cardCollection.Add(reader.ReadInt16());
                Categories.Add(cardCollection);

                reader.BaseStream.Position = tempOffset;
            }
        }

        public override void Save(BinaryWriter writer)
        {
            var fileStartPos = writer.BaseStream.Position;

            writer.Write((long) Categories.Count);

            var offsetsOffset = writer.BaseStream.Position;
            writer.Write(new byte[Categories.Count * 8]);

            for (var i = 0; i < Categories.Count; i++)
            {
                var tempOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = offsetsOffset + i * 8;
                writer.WriteOffset(fileStartPos, tempOffset);
                writer.BaseStream.Position = tempOffset;

                writer.Write((short) Categories[i].CardIds.Count);
                foreach (var cardId in Categories[i].CardIds) writer.Write(cardId);
            }
        }
    }
}