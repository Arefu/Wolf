using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Miscellaneous_Files
{
    public class Strings_BND : File_Data
    {
        private readonly Encoding encoding = Encoding.BigEndianUnicode;

        public Strings_BND()
        {
            Strings = new List<Localized_Text>();
        }

        public List<Localized_Text> Strings { get; }

        public override bool IsLocalized => true;

        public override void Load(BinaryReader reader, long length, Localized_Text.Language language)
        {
            var fileStartPos = reader.BaseStream.Position;

            var count = Endian.ConvertUInt32(reader.ReadUInt32());
            for (var i = 0; i < count; i++)
            {
                var offset = Endian.ConvertUInt32(reader.ReadUInt32());
                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = fileStartPos + offset;

                var text = Strings.Count > i ? Strings[i] : null;
                if (text == null)
                {
                    text = new Localized_Text();
                    Strings.Add(text);
                }

                text.SetText(language, reader.ReadNullTerminatedString(encoding));

                reader.BaseStream.Position = tempOffset;
            }
        }

        public override void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            var fileStartPos = writer.BaseStream.Position;

            writer.Write(Endian.ConvertUInt32((uint) Strings.Count));
            writer.Write(new byte[Strings.Count * 4]);

            for (var i = 0; i < Strings.Count; i++)
            {
                var writerOffset = writer.BaseStream.Position;

                writer.BaseStream.Position = fileStartPos + (4 + i * 4);
                writer.Write(Endian.ConvertUInt32((uint) (writerOffset - fileStartPos)));

                writer.BaseStream.Position = writerOffset;
                writer.WriteNullTerminatedString(Strings[i].GetText(language), encoding);
            }
        }
    }
}