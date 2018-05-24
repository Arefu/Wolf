using Celtic_Guardian.LOTD_Files;
using Celtic_Guardian.Utility;
using System.IO;

namespace Celtic_Guardian.Miscellaneous_Files
{
    public class ZIB_File
    {
        public ZIB_Data Owner { get; set; }
        public string FileName { get; set; }
        public long Offset { get; set; }
        public long Length { get; set; }

        public string FilePathOnDisk { get; set; }

        public bool IsFileOnDisk
        {
            get { return !string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk); }
        }

        public string Extension
        {
            get { return Path.GetExtension(FileName); }
        }

        public File_Types FileType
        {
            get { return LOTD_File.GetFileTypeFromExtension(FileName, Extension); }
        }

        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(FileName) || FileName.Length > 64 - 16)
                {
                    return false;
                }
                if (!string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk))
                {
                    return true;
                }
                return Offset != 0 && Length != 0;
            }
        }

        public ZIB_File(ZIB_Data owner, string fileName, long offset, long length)
        {
            Owner = owner;
            FileName = fileName;
            Offset = offset;
            Length = length;
        }

        public long CalculateLength()
        {
            if (!string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk))
            {
                return new FileInfo(FilePathOnDisk).Length;
            }
            return Length;
        }

        public byte[] Load()
        {
            return Load(Owner.File.Archive.Reader);
        }

        public byte[] Load(BinaryReader reader)
        {
            if (!string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk))
            {
                return File.ReadAllBytes(FilePathOnDisk);
            }
            if (Offset == 0 && Length == 0)
            {
                return null;
            }

            if (Owner.File != null && Owner.File.IsArchiveFile)
            {
                reader.BaseStream.Position = Owner.File.ArchiveOffset + Offset;
            }
            else
            {
                reader.BaseStream.Position = Offset;
            }
            return reader.ReadBytes((int)Length);
        }

        public byte[] LoadBuffer()
        {
            if (IsFileOnDisk)
            {
                if (!File.Exists(FilePathOnDisk))
                {
                    return null;
                }
                return File.ReadAllBytes(FilePathOnDisk);
            }
            else if (Owner != null && Owner.File != null && Offset > 0 && Length > 0)
            {
                Owner.File.Archive.Reader.BaseStream.Position = Owner.File.ArchiveOffset + Offset;
                return Owner.File.Archive.Reader.ReadBytes((int)Length);
            }

            return null;
        }

        public T LoadData<T>() where T : File_Data
        {
            return LoadData() as T;
        }

        public T LoadData<T>(bool cache) where T : File_Data
        {
            return LoadData(cache) as T;
        }

        public File_Data LoadData()
        {
            return LoadData(true);
        }

        public File_Data LoadData(bool cache)
        {
            File_Data fileData = LOTD_File.CreateFileData(LOTD_File.GetFileTypeFromExtension(FileName, Extension));

            if (IsFileOnDisk)
            {
                if (!File.Exists(FilePathOnDisk))
                {
                    return null;
                }
                fileData.Load(FilePathOnDisk);
                return fileData;
            }
            else if (Owner != null && Owner.File != null && Offset > 0 && Length > 0)
            {
                fileData.ZibFile = this;
                Owner.File.Archive.Reader.BaseStream.Position = Owner.File.ArchiveOffset + Offset;
                fileData.Load(Owner.File.Archive.Reader, Length);
                return fileData;
            }

            return null;
        }

        public ZIB_File GetLocalizedFile(Localized_Text.Language language)
        {
            if (LOTD_File.GetLanguageFromFileName(FileName) == language)
            {
                return this;
            }

            string fileName = LOTD_File.GetFileNameWithLanguage(FileName, language);
            if (!string.IsNullOrEmpty(fileName))
            {
                ZIB_File file;
                Owner.Files.TryGetValue(fileName, out file);
                return file;
            }

            return null;
        }
    }
}