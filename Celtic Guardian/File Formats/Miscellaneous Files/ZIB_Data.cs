using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Celtic_Guardian.Miscellaneous_Files
{
    public class ZIB_Data : File_Data
    {
        private const int align = 16;
        private readonly Encoding stringEncoding = Encoding.ASCII;

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

        private long ReadOffsetLength(BinaryReader reader, bool longOffsets)
        {
            return longOffsets ? Endian.ConvertInt64(reader.ReadInt64()) : Endian.ConvertUInt32(reader.ReadUInt32());
        }

        private string ReadString(BinaryReader reader, int length)
        {
            return stringEncoding.GetString(reader.ReadBytes(length)).TrimEnd('\0');
        }

        //***************
        //Dump Prototypes
        //***************
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
            if (File != null) outputDir = Path.Combine(outputDir, Path.GetFileNameWithoutExtension(File.FullName));
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);

            foreach (var file in Files.Values)
                System.IO.File.WriteAllBytes(Path.Combine(outputDir, file.FileName), file.Load(reader));
        }

        //***************************
        //IsLongOffsetFile Prototypes
        //***************************
        private bool IsLongOffsetFile(BinaryReader reader)
        {
            return IsLongOffsetFile();
        }

        private bool IsLongOffsetFile()
        {
            return File != null && File.Name.Contains("cardcrop");
        }
    }
}