using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Yu_Gi_Oh.File_Handling.Bin_Files;
using Yu_Gi_Oh.File_Handling.Main_Files;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Yu_Gi_Oh.File_Handling.Pack_Files;
using Yu_Gi_Oh.File_Handling.Utility;
using Yu_Gi_Oh.File_Handling.ZIB_Files;

namespace Yu_Gi_Oh.File_Handling.LOTD_Files
{
    public class LOTD_Archive
    {
        public static string InstallDir = "";

        public LOTD_Archive()
        {
            InstallDirectory = GetInstallDirectory();
        }

        public LOTD_Archive(string installDirectory)
        {
            InstallDirectory = installDirectory;
        }

        public LOTD_Archive(bool WriteAccess)
        {
            InstallDirectory = GetInstallDirectory();
            this.WriteAccess = WriteAccess;
        }

        public string InstallDirectory { get; }
        public BinaryReader Reader { get; private set; }
        public LOTD_Directory Root { get; private set; }
        public bool WriteAccess { get; set; }

        public void Load()
        {
            //if (string.IsNullOrEmpty(InstallDirectory)) throw new Exception("Invalid directory: " + InstallDirectory);
            

            var tocPath = Path.Combine(InstallDirectory, "YGO_DATA.toc");
            var datPath = Path.Combine(InstallDirectory, "YGO_DATA.dat");

            if (!File.Exists(tocPath) || !File.Exists(datPath)) throw new Exception("Failed to find data files");

            if (Reader != null)
            {
                Reader.Close();
                Reader = null;
            }

            Root = new LOTD_Directory
            {
                Archive = this,
                IsRoot = true
            };

            var filePaths = new List<string>();

            try
            {
                long offset = 0;

                var lines = File.ReadAllLines(tocPath);
                foreach (var line in lines)
                {
                    if (line.StartsWith("UT")) continue;

                    var offsetStart = -1;
                    for (var charIndex = 0; charIndex < line.Length; charIndex++)
                        if (line[charIndex] != ' ')
                        {
                            offsetStart = charIndex;
                            break;
                        }

                    var offsetEnd = offsetStart == -1 ? -1 : line.IndexOf(' ', offsetStart);
                    var unknownStart = offsetEnd == -1 ? -1 : offsetEnd + 1;
                    var unknownEnd = unknownStart == -1 ? -1 : line.IndexOf(' ', unknownStart + 1);

                    var validLine = unknownEnd >= 0;

                    if (validLine)
                    {
                        var lengthStr = line.Substring(offsetStart, offsetEnd - offsetStart);
                        var filePathLengthStr = line.Substring(unknownStart, unknownEnd - unknownStart);
                        var filePath = line.Substring(unknownEnd + 1);

                        if (long.TryParse(lengthStr, NumberStyles.HexNumber, null, out var length) &&
                            int.TryParse(filePathLengthStr, NumberStyles.HexNumber, null, out var filePathLength) &&
                            filePathLength == filePath.Length)
                        {
                            Root.AddFile(filePath, offset, length);

                            offset += length;

                            const int align = 4;
                            if (length % align != 0) offset += align - length % align;

                            filePaths.Add(filePath);
                        }
                        else
                        {
                            validLine = false;
                        }
                    }

                    if (!validLine) throw new Exception("Failed to parse line in toc file " + line);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Error when reading .toc file: " + e);
            }

            try
            {
                if (WriteAccess)
                    Reader = new BinaryReader(File.Open(datPath, FileMode.Open, FileAccess.ReadWrite));
                else
                    Reader = new BinaryReader(File.OpenRead(datPath));
            }
            catch (Exception e)
            {
                throw new Exception("Error when opening .dat file: " + e);
            }

            // Validate all file paths
            foreach (var filePath in filePaths)
            {
                var file = Root.FindFile(filePath);
                if (file == null)
                    throw new Exception("Archive loader is broken. File path not found in archive structure: '" +
                                        filePath + "'");
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
                throw new Exception("Invalid directory! CHECK CODE PLEASE!");

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
            var targetFileType = LOTD_File.GetFileType(typeof(T));

            var result = new List<T>();

            var files = Root.GetAllFiles();
            foreach (var file in files)
                if (file.FileType == File_Types.Zib)
                {
                    var zibData = file.LoadData<ZIB_Data>();
                    foreach (var zibFile in zibData.Files.Values)
                        if (zibFile.FileType == targetFileType)
                        {
                            var data = zibFile.LoadData<T>();
                            if (data != null) result.Add(data);
                        }
                }
                else if (file.FileType == targetFileType)
                {
                    var data = file.LoadData<T>();
                    if (data != null) result.Add(data);
                }

            return result;
        }

        public Dictionary<Localized_Text.Language, byte[]> LoadLocalizedBuffer(string search,
            bool startsWithElseContains)
        {
            var result = new Dictionary<Localized_Text.Language, byte[]>();

            search = search.ToLower();

            var files = Root.GetAllFiles();
            foreach (var file in files)
                if (file.FileType == File_Types.Zib)
                {
                    var zibData = file.LoadData<ZIB_Data>();
                    foreach (var zibFile in zibData.Files.Values)
                    {
                        var language = LOTD_File.GetLanguageFromFileName(zibFile.FileName);
                        if (language == Localized_Text.Language.Unknown) continue;

                        if (startsWithElseContains && zibFile.FileName.ToLower().StartsWith(search) ||
                            !startsWithElseContains && zibFile.FileName.ToLower().Contains(search))
                            result.Add(language, zibFile.LoadBuffer());
                    }
                }
                else
                {
                    var language = LOTD_File.GetLanguageFromFileName(file.Name);
                    if (language == Localized_Text.Language.Unknown) continue;

                    if (startsWithElseContains && file.Name.ToLower().StartsWith(search) ||
                        !startsWithElseContains && file.Name.ToLower().Contains(search))
                        result.Add(language, file.LoadBuffer());
                }

            return result;
        }

        public T LoadLocalizedFile<T>() where T : File_Data, new()
        {
            var targetFileType = LOTD_File.GetFileType(typeof(T));

            var result = new T();
            if (!result.IsLocalized) throw new InvalidOperationException("No Localized File Ready! CHECK CODE PLEASE!");

            var files = Root.GetAllFiles();
            foreach (var file in files)
                if (file.FileType == File_Types.Zib)
                {
                    var zibData = file.LoadData<ZIB_Data>();
                    foreach (var zibFile in zibData.Files.Values)
                        if (zibFile.FileType == targetFileType)
                        {
                            result.File = null;
                            result.ZibFile = zibFile;
                            result.Load();
                        }
                }
                else if (file.FileType == targetFileType)
                {
                    result.File = file;
                    result.ZibFile = null;
                    result.Load();
                }

            return result;
        }

        internal void RunTests()
        {
            TestFileType<Credits>();
            TestFileType<Strings_BND>();
            TestFileType<Battle_Pack>();
            TestFileType<Shop_Pack>();
            TestFileType<How_To_Play>();
            TestFileType<SKU_Data>();
            TestFileType<Arena_Data>();
            TestFileType<Char_Data>();
            TestFileType<Deck_Data>();
            TestFileType<Duel_Data>();
            TestFileType<Pack_Def_Data>();
            TestFileType<Script_Data>();
            TestFileType<Card_Limits>();
            TestFileType<Related_Card_Data>();
            TestFileType<Dfymoo>();
        }

        private List<T> TestFileType<T>() where T : File_Data
        {
            var result = LoadFiles<T>();
            foreach (var data in result) ValidateData(data);
            return result;
        }

        private static void ValidateData(File_Data file)
        {
            var padding = Enumerable.Repeat((byte) 0xFF, 100).ToArray();
            var buffer = file.ZibFile != null ? file.ZibFile.LoadBuffer() : file.File.LoadBuffer();

            var paddedBuffer = new byte[buffer.Length + padding.Length];
            Buffer.BlockCopy(padding, 0, paddedBuffer, 0, padding.Length);
            Buffer.BlockCopy(buffer, 0, paddedBuffer, padding.Length, buffer.Length);

            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(padding);

                file.Save(bw);
                var buffer2 = ms.ToArray();
                if (file.FileType == File_Types.Dfymoo)
                {
                    Debug.Assert(Encoding.ASCII.GetString(paddedBuffer).TrimEnd('\r', '\n') ==
                                 Encoding.ASCII.GetString(buffer2).TrimEnd('\r', '\n'));
                }
                else
                {
                    for (var i = 0; i < paddedBuffer.Length; i++) Debug.Assert(paddedBuffer[i] == buffer2[i]);
                    Debug.Assert(paddedBuffer.SequenceEqual(buffer2));
                }
            }
        }

        public static string GetInstallDirectory()
        {
            try
            {
                using (var Root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var Key =Root.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 480650"))
                    {
                        if (Key?.GetValue("InstallLocation").ToString() != null)
                        {
                            return Key?.GetValue("InstallLocation").ToString();
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
            using (var Ofd = new FolderBrowserDialog())
            {
                Ofd.Description = "Please Navigate To Your Install Directory";

                if (Ofd.ShowDialog() == DialogResult.OK)
                {
                    InstallDir = Ofd.SelectedPath;
                    return InstallDir;
                }

                return null;
            }
        }
    }
}