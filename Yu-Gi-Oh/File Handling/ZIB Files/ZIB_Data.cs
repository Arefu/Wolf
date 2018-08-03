using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Yu_Gi_Oh.File_Handling.Utility;

namespace Yu_Gi_Oh.File_Handling.ZIB_Files
{
    public class ZIB_Data : File_Data
    {
        private const int Align = 16; // 16 byte alignment on file data
        private readonly Encoding _stringEncoding = Encoding.ASCII;

        public ZIB_Data()
        {
            Files = new Dictionary<string, ZIB_File>();
        }

        public Dictionary<string, ZIB_File> Files { get; }

        public override void Load(BinaryReader reader, long length)
        {
            var longOffsets = IsLongOffsetFile(reader);

            var firstFile = true;
            while (true)
            {
                var fileOffset = ReadOffsetLength(reader, longOffsets);
                var fileLength = ReadOffsetLength(reader, longOffsets);

                if (fileOffset == 0 && fileLength == 0) break;

                if (firstFile && !longOffsets) fileOffset--;
                firstFile = false;

                var fileName = ReadString(reader, 64 - (longOffsets ? 16 : 8));
                Files.Add(fileName, new ZIB_File(this, fileName, fileOffset, fileLength));
            }
        }

        public override void Save(BinaryWriter writer)
        {
            var longOffsets = IsLongOffsetFile();

            var writerOffsetsStart = writer.BaseStream.Position;
            var offsetOffsets = new Dictionary<ZIB_File, long>();

            var orderedFiles = new List<ZIB_File>();
            foreach (var file in Files.Values)
                if (file.IsValid)
                    orderedFiles.Add(file);
            orderedFiles = orderedFiles.OrderBy(x => x.FileName).ToList();

            var tempBuffer = new byte[64];
            foreach (var file in orderedFiles)
            {
                offsetOffsets.Add(file, writer.BaseStream.Position);
                writer.Write(tempBuffer);
            }

            writer.Write((long) 0);
            writer.Write((long) 0);

            var firstFile = true;
            foreach (var file in orderedFiles)
            {
                var writerOffset = writer.BaseStream.Position;

                var fileData = file.Load();
                var dataOffset = writerOffset;
                long dataLength = fileData.Length;

                if (firstFile && !longOffsets) dataOffset++;
                firstFile = false;

                writer.BaseStream.Position = offsetOffsets[file];
                WriteOffsetLength(writer, longOffsets, dataOffset);
                WriteOffsetLength(writer, longOffsets, dataLength);
                WriteString(writer, longOffsets, file.FileName);

                writer.BaseStream.Position = writerOffset;
                writer.Write(fileData);

                if (fileData.Length % Align != 0) writer.Write(new byte[Align - fileData.Length % Align]);
            }
        }

        private string ReadString(BinaryReader reader, int length)
        {
            return _stringEncoding.GetString(reader.ReadBytes(length)).TrimEnd('\0');
        }

        private static long ReadOffsetLength(BinaryReader reader, bool longOffsets)
        {
            return longOffsets ? Endian.ConvertInt64(reader.ReadInt64()) : Endian.ConvertUInt32(reader.ReadUInt32());
        }

        private void WriteString(BinaryWriter writer, bool longOffsets, string value)
        {
            var buffer = _stringEncoding.GetBytes(value);
            var padding = 64 - (longOffsets ? 16 : 8) - buffer.Length;
            if (padding < 0) throw new Exception("File name too long " + value);
            writer.Write(buffer);
            writer.Write(new byte[padding]);
        }

        private static void WriteOffsetLength(BinaryWriter writer, bool longOffsets, long value)
        {
            if (longOffsets)
                writer.Write(Endian.ConvertInt64(value));
            else
                writer.Write(Endian.ConvertUInt32((uint) value));
        }

        private bool IsLongOffsetFile(BinaryReader reader)
        {
            // TODO: Come up with a more generic method of calculating this based on the data
            return IsLongOffsetFile();
        }

        private bool IsLongOffsetFile()
        {
            return File != null && File.Name.Contains("cardcrop");
        }

        public override void Dump(string outputDir)
        {
            Dump(outputDir, File.Archive.Reader);
        }

        public override void Dump(Dump_Settings settings)
        {
            if (settings.Deep)
                Dump(settings.OutputDirectory, File.Archive.Reader);
            else
                base.Dump(settings);
        }

        public void Dump(string outputDir, BinaryReader reader)
        {
            if (outputDir == null) outputDir = string.Empty;
            if (File != null)
                outputDir = Path.Combine(outputDir,
                    Path.GetFileNameWithoutExtension(File.FullName) ?? throw new NullReferenceException());
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

            foreach (var file in Files.Values)
                System.IO.File.WriteAllBytes(Path.Combine(outputDir, file.FileName), file.Load(reader));
        }
    }
}