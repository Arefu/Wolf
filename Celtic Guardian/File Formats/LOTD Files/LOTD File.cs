using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Celtic_Guardian.Bin_Files;
using Celtic_Guardian.File_Formats.Miscellaneous_Files;
using Celtic_Guardian.Main_Files;
using Celtic_Guardian.Miscellaneous_Files;
using Celtic_Guardian.Pack_Files;

namespace Celtic_Guardian.LOTD_Files
{
    public class LOTD_File
    {
        //*******
        //Globals
        //*******
        private static readonly Dictionary<FileTypes, Type> fileTypeLookup = new Dictionary<FileTypes, Type>();
        private static readonly Dictionary<Type, FileTypes> fileTypeLookupReverse = new Dictionary<Type, FileTypes>();
        private LOTD_Directory _directory;
        private string _name;
        public File_Data CachedData;

        //***********
        //Constructor
        //***********
        static LOTD_File()
        {
            AddFileType(FileTypes.BattlePackBin, typeof(Battle_Pack));
            AddFileType(FileTypes.PackDataBin, typeof(Shop_Pack));
            AddFileType(FileTypes.HowToPlayBin, typeof(How_To_Play));
            AddFileType(FileTypes.SkuDataBin, typeof(SKU_Data));
            AddFileType(FileTypes.ArenaDataBin, typeof(Arena_Data));
            AddFileType(FileTypes.CharDataBin, typeof(Char_Data));
            AddFileType(FileTypes.DeckDataBin, typeof(Deck_Data));
            AddFileType(FileTypes.DuelDataBin, typeof(Duel_Data));
            AddFileType(FileTypes.PackDefDataBin, typeof(Pack_Def_Data));
            AddFileType(FileTypes.ScriptDataBin, typeof(Script_Data));
            AddFileType(FileTypes.CardLimits, typeof(Card_Limits));
            AddFileType(FileTypes.RelatedCardsBin, typeof(Related_Card_Data));
            AddFileType(FileTypes.CreditsDat, typeof(Credits));
            AddFileType(FileTypes.StringBnd, typeof(Strings_BND));
            AddFileType(FileTypes.Dfymoo, typeof(Dfymoo));
            AddFileType(FileTypes.Zib, typeof(ZIB_Data));
        }

        //**********
        //Properties
        //**********
        public string FilePathOnDisk { get; set; }
        public LOTD_Archive Archive { get; set; }
        public long ArchiveLength { get; set; }
        public long ArchiveOffset { get; set; }

        //*************************
        //Expanded Getters, Setters
        //*************************
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

        //****************
        //Getters, Setters
        //****************
        public bool IsArchiveFile => ArchiveLength > 0;
        public bool IsFileOnDisk => !string.IsNullOrEmpty(FilePathOnDisk) && File.Exists(FilePathOnDisk);
        public FileTypes FileType => GetFileTypeFromExtension(Name, Extension);
        public string Extension => Path.GetExtension(Name);
        public string FullName => Directory == null || Directory.IsRoot ? Name : Path.Combine(Directory.FullName, Name);

        //*****************
        //Utility Functions
        //*****************
        public LOTD_File GetLocalizedFile(Utilities.Language language)
        {
            if (GetLanguageFromFileName(Name) == language) return this;

            var fileName = GetFileNameWithLanguage(Name, language);
            if (!string.IsNullOrEmpty(fileName))
                return Archive.Root.FindFile(Path.Combine(Directory.FullName, fileName));

            return null;
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
            if (CachedData != null)
            {
                return CachedData;
            }

            var fileData = CreateFileData(FileType);
            fileData.File = this;
            if (!fileData.Load())
            {
                return null;
            }
            if (cache)
            {
                CachedData = fileData;
            }
            return fileData;
        }

        internal static File_Data CreateFileData(FileTypes Type)
        {
            var type = GetFileType(Type);
            if (type == null)
            {
                type = typeof(RawFile);
            }
            return Activator.CreateInstance(type) as File_Data;
        }

        public static Type GetFileType(FileTypes fileType)
        {
            fileTypeLookup.TryGetValue(fileType, out var type);
            return type;
        }

        public static FileTypes GetFileType(Type type)
        {
            fileTypeLookupReverse.TryGetValue(type, out var fileType);
            return fileType;
        }

        public static string GetFileNameWithLanguage(string fileName, Utilities.Language language)
        {
            var existingLanguage = GetLanguageFromFileName(fileName);
            if (existingLanguage != Utilities.Language.Unknown)
            {
                var languageChar = 'E';
                switch (language)
                {
                    case Utilities.Language.English:
                        languageChar = 'E';
                        break;
                    case Utilities.Language.French:
                        languageChar = 'F';
                        break;
                    case Utilities.Language.German:
                        languageChar = 'G';
                        break;
                    case Utilities.Language.Italian:
                        languageChar = 'I';
                        break;
                    case Utilities.Language.Spanish:
                        languageChar = 'S';
                        break;
                }

                var result = new StringBuilder(fileName);
                result[fileName.LastIndexOf('_') + 1] = languageChar;
                return result.ToString();
            }

            return null;
        }

        public static Utilities.Language GetLanguageFromFileName(string fileName)
        {
            if (fileName.LastIndexOf('_') < 0 || fileName.Length <= fileName.LastIndexOf('_') + 1)
                return Utilities.Language.Unknown;
            if (fileName.LastIndexOf('.') != -1 && fileName.LastIndexOf('.') != fileName.LastIndexOf('_') + 2)
                return Utilities.Language.Unknown;
            switch (char.ToUpper(fileName[fileName.LastIndexOf('_') + 1]))
            {
                case 'E': return Utilities.Language.English;
                case 'F': return Utilities.Language.French;
                case 'G': return Utilities.Language.German;
                case 'I': return Utilities.Language.Italian;
                case 'S': return Utilities.Language.Spanish;
                default: return Utilities.Language.Unknown;
            }
        }

        private static void AddFileType(FileTypes fileType, Type type)
        {
            fileTypeLookup.Add(fileType, type);
            fileTypeLookupReverse.Add(type, fileType);
        }

        internal static FileTypes GetFileTypeFromExtension(string name, string extension)
        {
            if (string.IsNullOrEmpty(extension))
                return FileTypes.Unknown;

            name = name.ToLower();

            switch (extension.ToLower())
            {
                case ".bin":
                    switch (name)
                    {
                        default:
                            if (name.Contains("battlepack"))
                                return FileTypes.BattlePackBin;
                            else if (name.StartsWith("packdata_"))
                                return FileTypes.PackDataBin;
                            else if (name.StartsWith("howtoplay_"))
                                return FileTypes.HowToPlayBin;
                            else if (name.StartsWith("skudata_"))
                                return FileTypes.SkuDataBin;
                            else if (name.StartsWith("arenadata_"))
                                return FileTypes.ArenaDataBin;
                            else if (name.StartsWith("chardata_"))
                                return FileTypes.CharDataBin;
                            else if (name.StartsWith("deckdata_"))
                                return FileTypes.DeckDataBin;
                            else if (name.StartsWith("dueldata_"))
                                return FileTypes.DuelDataBin;
                            else if (name.StartsWith("packdefdata_"))
                                return FileTypes.PackDefDataBin;
                            else if (name.StartsWith("scriptdata_"))
                                return FileTypes.ScriptDataBin;
                            else if (name.StartsWith("pd_limits"))
                                return FileTypes.CardLimits;
                            else if (name.StartsWith("tagdata"))
                                return FileTypes.RelatedCardsBin;
                            break;
                    }

                    break;
                case ".dat":
                    switch (name)
                    {
                        case "credits.dat": return FileTypes.CreditsDat;
                        default:
                            return FileTypes.Unknown;
                    }
                case ".bnd": return FileTypes.StringBnd;
                case ".dfymoo": return FileTypes.Dfymoo;
                case ".zib": return FileTypes.Zib;
                default:
                    return FileTypes.Unknown;
            }

            return FileTypes.Unknown;
        }

    }
}