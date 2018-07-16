using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Yu_Gi_Oh.File_Handling.Bin_Files;
using Yu_Gi_Oh.File_Handling.Main_Files;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Pack_Files;
using Yu_Gi_Oh.File_Handling.Utility;
using Yu_Gi_Oh.File_Handling.ZIB_Files;

namespace Yu_Gi_Oh.File_Handling.LOTD_Files
{
    public class LOTD_File
    {
        private static readonly Dictionary<File_Types, Type> fileTypeLookup = new Dictionary<File_Types, Type>();
        private static readonly Dictionary<Type, File_Types> fileTypeLookupReverse = new Dictionary<Type, File_Types>();
        private LOTD_Directory _directory;
        private string _name;
        public File_Data CachedData;

        static LOTD_File()
        {
            AddFileType(File_Types.BattlePackBin, typeof(Battle_Pack));
            AddFileType(File_Types.PackDataBin, typeof(Shop_Pack));
            AddFileType(File_Types.HowToPlayBin, typeof(How_To_Play));
            AddFileType(File_Types.SkuDataBin, typeof(SKU_Data));
            AddFileType(File_Types.ArenaDataBin, typeof(Arena_Data));
            AddFileType(File_Types.CharDataBin, typeof(Char_Data));
            AddFileType(File_Types.DeckDataBin, typeof(Deck_Data));
            AddFileType(File_Types.DuelDataBin, typeof(Duel_Data));
            AddFileType(File_Types.PackDefDataBin, typeof(Pack_Def_Data));
            AddFileType(File_Types.ScriptDataBin, typeof(Script_Data));
            AddFileType(File_Types.CardLimits, typeof(Card_Limits));
            AddFileType(File_Types.RelatedCardsBin, typeof(Related_Card_Data));
            AddFileType(File_Types.CreditsDat, typeof(Credits));
            AddFileType(File_Types.StringBnd, typeof(Strings_BND));
            AddFileType(File_Types.Dfymoo, typeof(Dfymoo));
            AddFileType(File_Types.Zib, typeof(ZIB_Data));
        }

        public string FilePathOnDisk { get; set; }
        public LOTD_Archive Archive { get; set; }
        public long ArchiveLength { get; set; }
        public long ArchiveOffset { get; set; }

        public bool IsArchiveFile => ArchiveLength > 0;
        public bool IsFileOnDisk => !string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk);
        public File_Types FileType => GetFileTypeFromExtension(Name, Extension);
        public string Extension => Path.GetExtension(Name);
        public string FullName => Directory == null || Directory.IsRoot ? Name : Path.Combine(Directory.FullName, Name);

        public LOTD_Directory Directory
        {
            get => _directory;
            set
            {
                if (_directory == value) return;
                _directory?.Files.Remove(Name);

                _directory = value;

                if (_directory != null)
                {
                    _directory?.Files.Add(Name, this);
                    Archive = _directory.Archive;
                }
                else
                {
                    Archive = null;
                }
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                if (_name == value) return;
                Directory?.Files.Remove(_name);
                _name = value;
                Directory?.Files.Add(_name, this);
            }
        }

        public LOTD_File GetLocalizedFile(Localized_Text.Language language)
        {
            if (GetLanguageFromFileName(Name) == language) return this;

            var fileName = GetFileNameWithLanguage(Name, language);
            return !string.IsNullOrEmpty(fileName)
                ? Archive.Root.FindFile(Path.Combine(Directory.FullName, fileName))
                : null;
        }

        public File_Data GetData()
        {
            return GetData(false);
        }

        public File_Data GetData(bool cache)
        {
            if (CachedData != null) return CachedData;

            var fileData = CreateFileData(FileType);
            fileData.File = this;
            if (cache) CachedData = fileData;
            return fileData;
        }

        public void Dump(string OutputDirectory)
        {
            Dump(new Dump_Settings(OutputDirectory));
        }

        public void Dump(Dump_Settings Settings)
        {
            LoadData(false).Dump(Settings);
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
            if (CachedData != null) return CachedData;

            var fileData = CreateFileData(FileType);
            fileData.File = this;
            if (!fileData.Load()) return null;
            if (cache) CachedData = fileData;
            return fileData;
        }

        internal static File_Data CreateFileData(File_Types Type)
        {
            var type = GetFileType(Type);
            if (type == null) type = typeof(Raw_File);
            return Activator.CreateInstance(type) as File_Data;
        }

        public static Type GetFileType(File_Types fileType)
        {
            fileTypeLookup.TryGetValue(fileType, out var type);
            return type;
        }

        public static File_Types GetFileType(Type type)
        {
            fileTypeLookupReverse.TryGetValue(type, out var fileType);
            return fileType;
        }

        public static string GetFileNameWithLanguage(string fileName, Localized_Text.Language language)
        {
            var existingLanguage = GetLanguageFromFileName(fileName);
            if (existingLanguage == Localized_Text.Language.Unknown) return null;
            var languageChar = 'E';
            switch (language)
            {
                case Localized_Text.Language.English:
                    languageChar = 'E';
                    break;

                case Localized_Text.Language.French:
                    languageChar = 'F';
                    break;

                case Localized_Text.Language.German:
                    languageChar = 'G';
                    break;

                case Localized_Text.Language.Italian:
                    languageChar = 'I';
                    break;

                case Localized_Text.Language.Spanish:
                    languageChar = 'S';
                    break;
                case Localized_Text.Language.Unknown:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(language), language, null);
            }

            var result = new StringBuilder(fileName) {[fileName.LastIndexOf('_') + 1] = languageChar};
            return result.ToString();
        }

        public static Localized_Text.Language GetLanguageFromFileName(string fileName)
        {
            if (fileName.LastIndexOf('_') < 0 || fileName.Length <= fileName.LastIndexOf('_') + 1)
                return Localized_Text.Language.Unknown;
            if (fileName.LastIndexOf('.') != -1 && fileName.LastIndexOf('.') != fileName.LastIndexOf('_') + 2)
                return Localized_Text.Language.Unknown;
            switch (char.ToUpper(fileName[fileName.LastIndexOf('_') + 1]))
            {
                case 'E': return Localized_Text.Language.English;
                case 'F': return Localized_Text.Language.French;
                case 'G': return Localized_Text.Language.German;
                case 'I': return Localized_Text.Language.Italian;
                case 'S': return Localized_Text.Language.Spanish;
                default: return Localized_Text.Language.Unknown;
            }
        }

        private static void AddFileType(File_Types fileType, Type type)
        {
            fileTypeLookup.Add(fileType, type);
            fileTypeLookupReverse.Add(type, fileType);
        }

        internal static File_Types GetFileTypeFromExtension(string name, string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return File_Types.Unknown;

            name = name.ToLower();

            switch (extension.ToLower())
            {
                case ".bin":
                    switch (name)
                    {
                        default:
                            if (name.Contains("battlepack"))
                                return File_Types.BattlePackBin;
                            else if (name.StartsWith("packdata_"))
                                return File_Types.PackDataBin;
                            else if (name.StartsWith("howtoplay_"))
                                return File_Types.HowToPlayBin;
                            else if (name.StartsWith("skudata_"))
                                return File_Types.SkuDataBin;
                            else if (name.StartsWith("arenadata_"))
                                return File_Types.ArenaDataBin;
                            else if (name.StartsWith("chardata_"))
                                return File_Types.CharDataBin;
                            else if (name.StartsWith("deckdata_"))
                                return File_Types.DeckDataBin;
                            else if (name.StartsWith("dueldata_"))
                                return File_Types.DuelDataBin;
                            else if (name.StartsWith("packdefdata_"))
                                return File_Types.PackDefDataBin;
                            else if (name.StartsWith("scriptdata_"))
                                return File_Types.ScriptDataBin;
                            else if (name.StartsWith("pd_limits"))
                                return File_Types.CardLimits;
                            else if (name.StartsWith("tagdata"))
                                return File_Types.RelatedCardsBin;
                            break;
                    }

                    break;

                case ".dat":
                    switch (name)
                    {
                        case "credits.dat": return File_Types.CreditsDat;
                        default:
                            return File_Types.Unknown;
                    }
                case ".bnd": return File_Types.StringBnd;
                case ".dfymoo": return File_Types.Dfymoo;
                case ".zib": return File_Types.Zib;
                default:
                    return File_Types.Unknown;
            }

            return File_Types.Unknown;
        }

        public byte[] LoadBuffer()
        {
            var fileData = CreateFileData(FileType);
            fileData.File = this;
            return fileData.LoadBuffer();
        }

        public void UnloadData()
        {
            CachedData.Unload();
            CachedData = null;
        }
    }
}