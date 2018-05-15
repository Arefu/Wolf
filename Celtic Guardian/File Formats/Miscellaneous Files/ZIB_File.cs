using System.IO;
using Celtic_Guardian.LOTD_Files;

namespace Celtic_Guardian.Miscellaneous_Files
{
    public class ZIB_File
    {
        //***********
        //Constructor
        //***********
        public ZIB_File(ZIB_Data owner, string fileName, long offset, long length)
        {
            Owner = owner;
            FileName = fileName;
            Offset = offset;
            Length = length;
        }

        //**********
        //Properties
        //**********
        public ZIB_Data Owner { get; set; }
        public string FileName { get; set; }
        public long Offset { get; set; }
        public long Length { get; set; }
        public string FilePathOnDisk { get; set; }

        //****************
        //Getters, Setters
        //****************
        public bool IsFileOnDisk => !string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk);
        public string Extension => Path.GetExtension(FileName);
        public FileTypes FileType => LOTD_File.GetFileTypeFromExtension(FileName, Extension);

        //***************
        //Load Prototypes
        //***************
        public byte[] Load()
        {
            return Load(Owner.File.Archive.Reader);
        }

        public byte[] Load(BinaryReader reader)
        {
            if (!string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk))
                return File.ReadAllBytes(FilePathOnDisk);
            if (Offset == 0 && Length == 0) return null;

            if (Owner.File != null && Owner.File.IsArchiveFile)
                reader.BaseStream.Position = Owner.File.ArchiveOffset + Offset;
            else
                reader.BaseStream.Position = Offset;
            return reader.ReadBytes((int) Length);
        }

        public ZIB_File GetLocalizedFile(Utilities.Language language)
        {
            if (LOTD_File.GetLanguageFromFileName(FileName) == language) return this;

            var fileName = LOTD_File.GetFileNameWithLanguage(FileName, language);
            if (!string.IsNullOrEmpty(fileName))
            {
                Owner.Files.TryGetValue(fileName, out var file);
                return file;
            }

            return null;
        }
    }
}