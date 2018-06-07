using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.Miscellaneous_Files
{
    internal class How_To_Play : File_Data
    {
        public enum EntryType : byte
        {
            MainEntry = 0,
            SubEntry = 1,
            SubEntryItem = 2,
            Unknown = 3,
            End = 4,
            Description = 0xFF
        }

        private const int align = 8;
        private readonly Encoding stringEncoding = Encoding.BigEndianUnicode;

        public How_To_Play()
        {
            Entries = new List<Entry>();
        }

        public List<Entry> Entries { get; }

        public override void Load(BinaryReader reader, long length)
        {
            Entries.Clear();
            var dataOffset = reader.BaseStream.Position;

            var count = Endian.ConvertUInt32(reader.ReadUInt32());

            var startChunkLen = count * 8 + 4;
            if (startChunkLen % align != 0) startChunkLen += align - startChunkLen % align;

            var stringOffset = dataOffset + startChunkLen;

            for (var i = 0; i < count; i++)
            {
                var placeholderBytes = reader.ReadUInt32();
                Debug.Assert(placeholderBytes == 0, "Unexpected placeholder data in howtoplay bin file");

                var type = (EntryType) reader.ReadByte();
                var imageId = reader.ReadByte();
                var len = Endian.ConvertUInt16(reader.ReadUInt16());

                var tempOffset = reader.BaseStream.Position;

                reader.BaseStream.Position = stringOffset;
                var str = Encoding.BigEndianUnicode.GetString(reader.ReadBytes(len * 2));
                reader.BaseStream.Position = tempOffset;

                var entry = new Entry(str, type, imageId);
                Entries.Add(entry);

                stringOffset += len * 2 + 2;
            }
        }

        public override void Save(BinaryWriter writer)
        {
            var count = (uint) Entries.Count;

            var injectEndEntry = false;
            if (Entries.Count == 0 || Entries[Entries.Count - 1].Type != EntryType.End)
            {
                count++;
                injectEndEntry = true;
            }

            writer.Write(Endian.ConvertUInt32(count));

            foreach (var entry in Entries)
            {
                writer.Write((uint) 0);
                writer.Write((byte) entry.Type);
                writer.Write(entry.ImageId);
                writer.Write(Endian.ConvertUInt16((ushort) (entry.Text?.Length ?? 0)));
            }

            if (injectEndEntry)
            {
                writer.Write((uint) 0);
                writer.Write((byte) EntryType.End);
                writer.Write((byte) 0);
                writer.Write((ushort) 0);
            }

            var startChunkLen = count * 8 + 4;
            if (startChunkLen % align != 0) writer.Write(new byte[align - startChunkLen % align]);

            foreach (var entry in Entries) writer.WriteNullTerminatedString(entry.Text, stringEncoding);
            if (injectEndEntry) writer.Write((ushort) 0);
        }

        public class Entry
        {
            public Entry(string text, EntryType type, byte imageId)
            {
                Text = text;
                Type = type;
                ImageId = imageId;
            }

            public string Text { get; set; }
            public EntryType Type { get; set; }

            public byte ImageId { get; set; }
        }
    }
}