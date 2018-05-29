using System;
using System.IO;
using System.Text;
using Celtic_Guardian.File_Handling.LOTD_Files;
using Celtic_Guardian.File_Handling.Utility;
using Celtic_Guardian.File_Handling.ZIB_Files;

namespace Celtic_Guardian.File_Handling
{
    [Obsolete("This Class Shouldn't Be Used, It Should Only Be Inherited.")]
    public abstract class File_Data
    {
        public LOTD_File File { get; set; }

        public ZIB_File ZibFile { get; set; }

        public File_Types FileType => File?.FileType ?? (ZibFile?.FileType ?? File_Types.Unknown);

        public virtual bool IsLocalized => false;

        public byte[] LoadBuffer()
        {
            if (File.IsFileOnDisk)
                return LoadBuffer(File.FilePathOnDisk);

            if (File.IsArchiveFile)
                return LoadBuffer(File.Archive.Reader);

            throw new FileLoadException("Can't Load File Into Buffer! CHECK CODE PLEASE!");
        }

        public byte[] LoadBuffer(string path)
        {
            return System.IO.File.Exists(path) ? System.IO.File.ReadAllBytes(path) : throw new FileNotFoundException("Can't Load/Find File! CHECK CODE PLEASE!");
        }

        public byte[] LoadBuffer(BinaryReader reader)
        {
            File.Archive.Reader.BaseStream.Position = File.ArchiveOffset;
            return reader.ReadBytes((int) File.ArchiveLength);
        }

        public bool Load()
        {
            if (IsLocalized)
                return Load(GetLanguage());

            if (ZibFile != null)
            {
                if (ZibFile.IsFileOnDisk)
                {
                    if (!System.IO.File.Exists(ZibFile.FilePathOnDisk))
                        return false;

                    Load(ZibFile.FilePathOnDisk);
                    return true;
                }

                if (ZibFile.Owner?.File == null || ZibFile.Offset <= 0 || ZibFile.Length <= 0) return false;
                ZibFile.Owner.File.Archive.Reader.BaseStream.Position = ZibFile.Owner.File.ArchiveOffset + ZibFile.Offset;
                Load(ZibFile.Owner.File.Archive.Reader, ZibFile.Length);
                return true;
            }

            if (File == null) return false;

            if (File.IsFileOnDisk)
            {
                if (!System.IO.File.Exists(File.FilePathOnDisk))
                    return false;

                Load(File.FilePathOnDisk);
                return true;
            }

            if (!File.IsArchiveFile)
                return false;

            File.Archive.Reader.BaseStream.Position = File.ArchiveOffset;
            Load(File.Archive.Reader, File.ArchiveLength);
            return true;
        }

        public void Load(string path)
        {
            using (var reader = new BinaryReader(System.IO.File.OpenRead(path)))
            {
                Load(reader, reader.BaseStream.Length);
            }
        }

        public bool Load(Localized_Text.Language language)
        {
            if (ZibFile != null)
            {
                if (ZibFile.IsFileOnDisk)
                {
                    if (!System.IO.File.Exists(ZibFile.FilePathOnDisk)) return false;
                    Load(ZibFile.FilePathOnDisk, language);
                    return true;
                }

                if (ZibFile.Owner?.File == null || ZibFile.Offset <= 0 || ZibFile.Length <= 0) return false;

                ZibFile.Owner.File.Archive.Reader.BaseStream.Position = ZibFile.Owner.File.ArchiveOffset + ZibFile.Offset;
                Load(ZibFile.Owner.File.Archive.Reader, ZibFile.Length, language);
                return true;
            }

            if (File == null) return false;

            if (File.IsFileOnDisk)
            {
                if (!System.IO.File.Exists(File.FilePathOnDisk)) return false;
                Load(File.FilePathOnDisk, language);
                return true;
            }

            if (!File.IsArchiveFile) return false;

            File.Archive.Reader.BaseStream.Position = File.ArchiveOffset;
            Load(File.Archive.Reader, File.ArchiveLength);
            return true;
        }

        public void Load(string path, Localized_Text.Language language)
        {
            using (var reader = new BinaryReader(System.IO.File.OpenRead(path)))
            {
                Load(reader, reader.BaseStream.Length, language);
            }
        }

        public virtual void Load(BinaryReader reader, long length)
        {
            if (IsLocalized) Load(reader, length, GetLanguage());
        }

        public virtual void Load(BinaryReader reader, long length, Localized_Text.Language language)
        {
            throw new NotImplementedException("Localized Files Can't Be Loaded! CHECK CODE PLEASE!");
        }

        public void Save(string path)
        {
            using (var writer = new BinaryWriter(System.IO.File.Create(path)))
            {
                Save(writer);
            }
        }

        public virtual void Save(BinaryWriter writer)
        {
            if (IsLocalized) Save(writer, GetLanguage());
        }

        public void Save(string path, Localized_Text.Language language)
        {
            using (var writer = new BinaryWriter(System.IO.File.Create(path)))
            {
                Save(writer, language);
            }
        }

        public virtual void Save(BinaryWriter writer, Localized_Text.Language language)
        {
            throw new NotImplementedException("Localized Files Can't Be Saved! CHECK CODE PLEASE!");
        }

        public virtual void Dump(string outputDir)
        {
            Dump(new Dump_Settings(outputDir));
        }

        public virtual void Dump(Dump_Settings settings)
        {
            ShallowDump(settings);
        }

        private void ShallowDump(Dump_Settings settings)
        {
            if (File == null) return;
            var buffer = LoadBuffer();
            if (buffer == null) return;
            var outputDir = settings.OutputDirectory;
            outputDir = Path.Combine(outputDir ?? string.Empty, File.Directory.FullName);
            if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir)) Directory.CreateDirectory(outputDir);
            System.IO.File.WriteAllBytes(Path.Combine(outputDir, File.Name), buffer);
        }

        public virtual void Clear()
        {
        }

        public virtual void Unload()
        {
        }

        private Localized_Text.Language GetLanguage()
        {
            if (File != null) return LOTD_File.GetLanguageFromFileName(File.Name);
            return ZibFile != null ? LOTD_File.GetLanguageFromFileName(ZibFile.FileName) : Localized_Text.Language.Unknown;
        }

        protected int GetStringSize(string str, Encoding encoding)
        {
            return encoding.GetByteCount((str ?? string.Empty) + '\0');
        }

        public LOTD_File GetLocalizedFile(Localized_Text.Language language)
        {
            return File?.GetLocalizedFile(language);
        }

        public ZIB_File GetLocalizedZibFile(Localized_Text.Language language)
        {
            return ZibFile?.GetLocalizedFile(language);
        }
    }
}