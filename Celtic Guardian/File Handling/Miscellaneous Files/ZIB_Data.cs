using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Celtic_Guardian.Miscellaneous_Files
{
    public class ZIB_Data : File_Data
    {
        private const int align = 16;// 16 byte alignment on file data
        private Encoding stringEncoding = Encoding.ASCII;
        public Dictionary<string, ZIB_File> Files { get; private set; }

        public ZIB_Data()
        {
            Files = new Dictionary<string, ZIB_File>();
        }

        public override void Load(BinaryReader reader, long length)
        {
            bool longOffsets = IsLongOffsetFile(reader);

            bool firstFile = true;
            while (true)
            {
                long fileOffset = ReadOffsetLength(reader, longOffsets);
                long fileLength = ReadOffsetLength(reader, longOffsets);

                if (fileOffset == 0 && fileLength == 0)
                {
                    break;
                }

                if (firstFile && !longOffsets)
                {
                    // Files which use 4 bytes for offsets/len seem to have an incorrect offset for the first file.
                    // These files also have 8 bytes of 00 padding between the last file info and the actual content.
                    // - It could be possible that the native client always reads 8 bytes for offset/len and determines
                    //   if it should be 4 byte offset? Then that somehow impacts the first file offset as part of the calculation.
                    fileOffset--;
                }
                firstFile = false;

                string fileName = ReadString(reader, 64 - (longOffsets ? 16 : 8));
                Files.Add(fileName, new ZIB_File(this, fileName, fileOffset, fileLength));
            }
        }

        public override void Save(BinaryWriter writer)
        {
            bool longOffsets = IsLongOffsetFile();

            long writerOffsetsStart = writer.BaseStream.Position;
            Dictionary<ZIB_File , long> offsetOffsets = new Dictionary<ZIB_File, long>();

            List<ZIB_File> orderedFiles = new List<ZIB_File>();
            foreach (ZIB_File file in Files.Values)
            {
                if (file.IsValid)
                {
                    orderedFiles.Add(file);
                }
            }
            orderedFiles = orderedFiles.OrderBy(x => x.FileName).ToList();

            byte[] tempBuffer = new byte[64];
            foreach (ZIB_File file in orderedFiles)
            {
                offsetOffsets.Add(file, writer.BaseStream.Position);
                writer.Write(tempBuffer);
            }

            writer.Write((long)0);
            writer.Write((long)0);

            bool firstFile = true;
            foreach (ZIB_File file in orderedFiles)
            {
                long writerOffset = writer.BaseStream.Position;

                byte[] fileData = file.Load();
                long dataOffset = writerOffset;
                long dataLength = fileData.Length;

                if (firstFile && !longOffsets)
                {
                    dataOffset++;
                }
                firstFile = false;

                writer.BaseStream.Position = offsetOffsets[file];
                WriteOffsetLength(writer, longOffsets, dataOffset);
                WriteOffsetLength(writer, longOffsets, dataLength);
                WriteString(writer, longOffsets, file.FileName);

                writer.BaseStream.Position = writerOffset;
                writer.Write(fileData);

                if (fileData.Length % align != 0)
                {
                    writer.Write(new byte[align - (fileData.Length % align)]);
                }
            }
        }

        private string ReadString(BinaryReader reader, int length)
        {
            return stringEncoding.GetString(reader.ReadBytes(length)).TrimEnd('\0');
        }

        private long ReadOffsetLength(BinaryReader reader, bool longOffsets)
        {
            if (longOffsets)
            {
                return Endian.ConvertInt64(reader.ReadInt64());
            }
            else
            {
                return Endian.ConvertUInt32(reader.ReadUInt32());
            }
        }

        private void WriteString(BinaryWriter writer, bool longOffsets, string value)
        {
            byte[] buffer = stringEncoding.GetBytes(value);
            int padding = (64 - (longOffsets ? 16 : 8)) - buffer.Length;
            if (padding < 0)
            {
                throw new Exception("File name too long " + value);
            }
            writer.Write(buffer);
            writer.Write(new byte[padding]);
        }

        private void WriteOffsetLength(BinaryWriter writer, bool longOffsets, long value)
        {
            if (longOffsets)
            {
                writer.Write(Endian.ConvertInt64(value));
            }
            else
            {
                writer.Write(Endian.ConvertUInt32((uint)value));
            }
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
            {
                Dump(settings.OutputDirectory, File.Archive.Reader);
            }
            else
            {
                base.Dump(settings);
            }
        }

        public void Dump(string outputDir, BinaryReader reader)
        {
            if (outputDir == null)
            {
                outputDir = string.Empty;
            }
            if (File != null)
            {
                outputDir = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(File.FullName));
            }
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
            {
                Directory.CreateDirectory(outputDir);
            }

            foreach (ZIB_File file in Files.Values)
            {
                System.IO.File.WriteAllBytes(Path.Combine(outputDir, file.FileName), file.Load(reader));
            }
        }
    }
}