using Celtic_Guardian.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Celtic_Guardian.LOTD_Files
{
    public class LOTD_Archive
    {
        //**********
        //Properties
        //**********
        public string InstallDirectory { get; private set; }
        public BinaryReader Reader { get; private set; }
        public LOTD_Directory Root { get; private set; }
        public bool WriteAccess { get; set; }

        //***********
        //Constructor
        //***********
        public LOTD_Archive()
        {
            InstallDirectory = Utilities.GetInstallDir();
        }
        public LOTD_Archive(string installDirectory)
        {
            InstallDirectory = installDirectory;
        }

        //*****************
        //Functions
        //*****************
        public void Load()
        {
            if (string.IsNullOrEmpty(InstallDirectory))
            {
                throw new Exception("Invalid directory: " + InstallDirectory);
            }

            string tocPath = Path.Combine(InstallDirectory, "YGO_DATA.toc");
            string datPath = Path.Combine(InstallDirectory, "YGO_DATA.dat");

            if (!File.Exists(tocPath) || !File.Exists(datPath))
            {
                throw new Exception("Failed to find data files");
            }

            if (Reader != null)
            {
                Reader.Close();
                Reader = null;
            }

            Root = new LOTD_Directory();
            Root.Archive = this;
            Root.IsRoot = true;

            List<string> filePaths = new List<string>();

            try
            {
                long offset = 0;

                string[] lines = File.ReadAllLines(tocPath);
                for (int i = 0; i < lines.Length; i++)
                {
                    string line = lines[i];

                    if (!line.StartsWith("UT"))
                    {
                        int offsetStart = -1;
                        for (int charIndex = 0; charIndex < line.Length; charIndex++)
                        {
                            if (line[charIndex] != ' ')
                            {
                                offsetStart = charIndex;
                                break;
                            }
                        }

                        int offsetEnd = offsetStart == -1 ? -1 : line.IndexOf(' ', offsetStart);
                        int unknownStart = offsetEnd == -1 ? -1 : offsetEnd + 1;
                        int unknownEnd = unknownStart == -1 ? -1 : line.IndexOf(' ', unknownStart + 1);

                        bool validLine = unknownEnd >= 0;

                        if (validLine)
                        {
                            string lengthStr = line.Substring(offsetStart, offsetEnd - offsetStart);
                            string filePathLengthStr = line.Substring(unknownStart, unknownEnd - unknownStart);
                            string filePath = line.Substring(unknownEnd + 1);

                            long length;
                            int filePathLength;
                            if (long.TryParse(lengthStr, NumberStyles.HexNumber, null, out length) && int.TryParse(filePathLengthStr, NumberStyles.HexNumber, null, out filePathLength) && filePathLength == filePath.Length)
                            {
                                Root.AddFile(filePath, offset, length);

                                offset += length;

                                // Add the offset for the data alignment
                                const int align = 4;
                                if (length % align != 0)
                                {
                                    offset += align - (length % align);
                                }

                                filePaths.Add(filePath);
                            }
                            else
                            {
                                validLine = false;
                            }
                        }

                        if (!validLine)
                        {
                            throw new Exception("Failed to parse line in toc file " + line);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error when reading .toc file: " + e);
            }

            try
            {
                if (WriteAccess)
                {
                    Reader = new BinaryReader(File.Open(datPath, FileMode.Open, FileAccess.ReadWrite));
                }
                else
                {
                    Reader = new BinaryReader(File.OpenRead(datPath));
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error when opening .dat file: " + e);
            }

            // Validate all file paths
            foreach (string filePath in filePaths)
            {
                LOTD_File file = Root.FindFile(filePath);
                if (file == null)
                {
                    throw new Exception("Archive loader is broken. File path not found in archive structure: '" + filePath + "'");
                }
            }
        }

        public void Save()
        {
            Save(InstallDirectory);
        }

        public void Save(bool createBackup)
        {
            Save(InstallDirectory, createBackup);
        }

        public void Save(string outputDir)
        {
            Save(outputDir, true);
        }

        public void Save(string outputDir, bool createBackup)
        {
            if (string.IsNullOrEmpty(outputDir) || !Directory.Exists(outputDir))
            {
                throw new Exception("Invalid directory. Make sure the directory exists. '" + outputDir + "'");
            }

            throw new NotImplementedException();
        }

        public void Dump(string outputDir)
        {
            Dump(new Dump_Settings(outputDir));
        }

        public void Dump(Dump_Settings settings)
        {
            Root.Dump(settings);
        }

        public List<T> LoadFiles<T>() where T : File_Data
        {
            File_Types targetFileType = LOTD_File.GetFileType(typeof(T));

            List<T> result = new List<T>();

            List<LOTD_File> files = Root.GetAllFiles();
            foreach (LOTD_File file in files)
            {
                if (file.FileType == File_Types.Zib)
                {
                    Miscellaneous_Files.ZIB_Data zibData = file.LoadData<Miscellaneous_Files.ZIB_Data>();
                    foreach (Miscellaneous_Files.ZIB_File zibFile in zibData.Files.Values)
                    {
                        if (zibFile.FileType == targetFileType)
                        {
                            T data = zibFile.LoadData<T>();
                            if (data != null)
                            {
                                result.Add(data);
                            }
                        }
                    }
                }
                else if (file.FileType == targetFileType)
                {
                    T data = file.LoadData<T>();
                    if (data != null)
                    {
                        result.Add(data);
                    }
                }
            }

            return result;
        }

        public Dictionary<Localized_Text.Language, byte[]> LoadLocalizedBuffer(string search, bool startsWithElseContains)
        {
            Dictionary<Localized_Text.Language, byte[]> result = new Dictionary<Localized_Text.Language, byte[]>();

            search = search.ToLower();

            List<LOTD_File> files = Root.GetAllFiles();
            foreach (LOTD_File file in files)
            {
                if (file.FileType == File_Types.Zib)
                {
                    Miscellaneous_Files.ZIB_Data zibData = file.LoadData<Miscellaneous_Files.ZIB_Data>();
                    foreach (Miscellaneous_Files.ZIB_File zibFile in zibData.Files.Values)
                    {
                        Localized_Text.Language language = LOTD_File.GetLanguageFromFileName(zibFile.FileName);
                        if (language != Localized_Text.Language.Unknown)
                        {
                            if ((startsWithElseContains && zibFile.FileName.ToLower().StartsWith(search)) ||
                                (!startsWithElseContains && zibFile.FileName.ToLower().Contains(search)))
                            {
                                result.Add(language, zibFile.LoadBuffer());
                            }
                        }
                    }
                }
                else
                {
                    Localized_Text.Language language = LOTD_File.GetLanguageFromFileName(file.Name);
                    if (language != Localized_Text.Language.Unknown)
                    {
                        if ((startsWithElseContains && file.Name.ToLower().StartsWith(search)) ||
                            (!startsWithElseContains && file.Name.ToLower().Contains(search)))
                        {
                            result.Add(language, file.LoadBuffer());
                        }
                    }
                }
            }

            return result;
        }

        public T LoadLocalizedFile<T>() where T : File_Data, new()
        {
            File_Types targetFileType = LOTD_File.GetFileType(typeof(T));

            T result = new T();
            if (!result.IsLocalized)
            {
                throw new InvalidOperationException("Attempted to load a file with localization which has none");
            }

            List<LOTD_File> files = Root.GetAllFiles();
            foreach (LOTD_File file in files)
            {
                if (file.FileType == File_Types.Zib)
                {
                    Miscellaneous_Files.ZIB_Data zibData = file.LoadData<Miscellaneous_Files.ZIB_Data>();
                    foreach (Miscellaneous_Files.ZIB_File zibFile in zibData.Files.Values)
                    {
                        if (zibFile.FileType == targetFileType)
                        {
                            result.File = null;
                            result.ZibFile = zibFile;
                            result.Load();
                        }
                    }
                }
                else if (file.FileType == targetFileType)
                {
                    result.File = file;
                    result.ZibFile = null;
                    result.Load();
                }
            }

            return result;
        }

        internal void RunTests()
        {
            TestFileType<Miscellaneous_Files.Credits>();
            TestFileType<Miscellaneous_Files.Strings_BND>();
            TestFileType<Pack_Files.Battle_Pack>();
            TestFileType<Pack_Files.Shop_Pack>();
            TestFileType<Miscellaneous_Files.How_To_Play>();
            TestFileType<Main_Files.SKU_Data>();
            TestFileType<Main_Files.Arena_Data>();
            TestFileType<Main_Files.Char_Data>();
            TestFileType<Main_Files.Deck_Data>();
            TestFileType<Main_Files.Duel_Data>();
            TestFileType<Main_Files.Pack_Def_Data>();
            TestFileType<Main_Files.Script_Data>();
            TestFileType<Bin_Files.Card_Limits>();
            TestFileType<Bin_Files.Related_Card_Data>();
            TestFileType<Miscellaneous_Files.Dfymoo>();
        }

        private List<T> TestFileType<T>() where T : File_Data
        {
            List<T> result = LoadFiles<T>();
            foreach (T data in result)
            {
                ValidateData(data);
            }
            return result;
        }

        private void ValidateData(File_Data file)
        {
            // Add some padding as in real data we wont be starting at offset 0
            byte[] padding = Enumerable.Repeat((byte)0xFF, 100).ToArray();

            // Validate our Save function
            byte[] buffer = file.ZibFile != null ? file.ZibFile.LoadBuffer() : file.File.LoadBuffer();

            byte[] paddedBuffer = new byte[buffer.Length + padding.Length];
            Buffer.BlockCopy(padding, 0, paddedBuffer, 0, padding.Length);
            Buffer.BlockCopy(buffer, 0, paddedBuffer, padding.Length, buffer.Length);

            using (MemoryStream ms = new MemoryStream())
            using (BinaryWriter bw = new BinaryWriter(ms))
            {
                bw.Write(padding);

                file.Save(bw);
                byte[] buffer2 = ms.ToArray();
                if (file.FileType == File_Types.Dfymoo)
                {
                    System.Diagnostics.Debug.Assert(Encoding.ASCII.GetString(paddedBuffer).TrimEnd('\r', '\n') == Encoding.ASCII.GetString(buffer2).TrimEnd('\r', '\n'));
                }
                else
                {
                    for (int i = 0; i < paddedBuffer.Length; i++)
                    {
                        System.Diagnostics.Debug.Assert(paddedBuffer[i] == buffer2[i]);
                    }
                    System.Diagnostics.Debug.Assert(paddedBuffer.SequenceEqual(buffer2));
                }
            }
        }

        public static string GetInstallDirectory()
        {
            string installDir = null;
            try
            {
                using (var root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var key = root.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 480650"))
                    {
                        if (key != null)
                        {
                            installDir = key.GetValue("InstallLocation").ToString();
                        }
                    }
                }
            }
            catch
            {
            }
            return installDir;
        }
    }
}