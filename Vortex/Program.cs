using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Vortex
{

    internal class Program
    {
        public static List<PackData> Data = new List<PackData>();
        public static List<FileNames> Files = new List<FileNames>();
        public static string[] FilesToPack;
        public static bool AutoCopy, AutoStart;
        public static StreamReader Reader;

        [STAThread]
        private static void Main(string[] Args)
        {
            Console.Title = "Vortex";
            var YgoFolder = "";
            using (var FBD = new FolderBrowserDialog())
            {
                FBD.ShowNewFolderButton = false;
                FBD.Description = "Select the YGO_DATA folder.";

                var Reply = FBD.ShowDialog();
                if (Reply != DialogResult.OK) Environment.Exit(1);
                else YgoFolder = FBD.SelectedPath;
            }

            if (File.Exists($"{YgoFolder}\\YGO_DATA.dat") && File.Exists($"{YgoFolder}\\YGO_DATA.toc"))
            {
                Utilities.Log("Pre-Exported Files Found! Cleaning Working Directory.", Utilities.Event.Warning);
                File.Delete($"{YgoFolder}\\YGO_DATA.dat");
                File.Delete($"{YgoFolder}\\YGO_DATA.toc");
            }
            if (Args.Any(Arg => Arg == "-autocopy"))
            {
                Utilities.Log("Auto-Copy Specified!", Utilities.Event.Information);
                AutoCopy = true;
            }
            if (Args.Any(Arg => Arg == "-autostart"))
            {
                Utilities.Log("Auto-Start Specified!", Utilities.Event.Information);
                AutoStart = true;
            }

            Files = ParseTocFile();
            FilesToPack = Directory.GetFiles($"{YgoFolder}", "*.*", SearchOption.AllDirectories);

            File.AppendAllText("YGO_DATA.toc", "UT\n");

            using (var Writer = new BinaryWriter(File.Open("YGO_DATA.dat", FileMode.Append, FileAccess.Write)))
            {
                foreach (var Item in Files)
                {
                    //Read Current File.

                    var CurrentFileName = FilesToPack
                        ?.First(File => File.Contains(Item.FileName))
                      ;
                    Utilities.Log($"Packing File: {CurrentFileName}.", Utilities.Event.Information);
                    var CurrentFileNameLength = Utilities.DecToHex(CurrentFileName.Length.ToString());
                    var CurrentFileSize =
                        Utilities.DecToHex(new FileInfo($".\\{CurrentFileName}").Length.ToString());

                    while (CurrentFileSize.Length != 12)
                        CurrentFileSize = CurrentFileSize.Insert(0, " ");
                    while (CurrentFileNameLength.Length != 2)
                        CurrentFileNameLength = CurrentFileNameLength.Insert(0, " ");

                    var Token = $".\\YGO_DATA\\{CurrentFileName}";
                    var Reader = new BinaryReader(File.Open(Token, FileMode.Open, FileAccess.Read));
                    var NewSize = new FileInfo(Token).Length;
                    while (NewSize % 4 != 0)
                        NewSize = NewSize + 1;

                    var BufferSize = NewSize - new FileInfo(Token).Length;
                    Writer.Write(Reader.ReadBytes((int)new FileInfo(Token).Length));

                    if (BufferSize > 0)
                        while (BufferSize != 0)
                        {
                            Writer.Write(new byte[] { 00 });
                            BufferSize = BufferSize - 1;
                        }

                    File.AppendAllText("YGO_DATA.toc",
                        $"{CurrentFileSize} {CurrentFileNameLength} {CurrentFileName}\n");
                }
            }
            Utilities.Log("Finished Packing Files.", Utilities.Event.Information);
            if (!AutoCopy) return;

            File.Copy("YGO_DATA.toc", $"{Utilities.GetInstallDir()}\\YGO_DATA.toc", true);
            File.Copy("YGO_DATA.dat", $"{Utilities.GetInstallDir()}\\YGO_DATA.dat", true);
            Utilities.Log("Finished Packing, And Moved Files.", Utilities.Event.Information);

            if (!AutoStart) return;

            Process.Start($"{Utilities.GetInstallDir()}\\YuGiOh.exe");
        }

        private static List<FileNames> ParseTocFile()
        {
            var Files = new List<FileNames>();
            try
            {
                var Reader = new StreamReader($"{Utilities.GetInstallDir()}\\YGO_DATA.TOC");
            }
            catch (Exception e)
            {
                using (var OFD = new OpenFileDialog())
                {
                    OFD.Title = "Select YuGiOh.exe";
                    OFD.Filter = "YuGiOh.exe | YuGiOh.exe";
                    var Result = OFD.ShowDialog();
                    if (Result != DialogResult.OK) Environment.Exit(1);
                    Reader = new StreamReader(File.Open($"{new FileInfo(OFD.FileName).DirectoryName}\\YGO_DATA.TOC",
                        FileMode.Open, FileAccess.Read));
                }
            }
            Reader.ReadLine(); //Dispose First Line.
            while (!Reader.EndOfStream)
            {
                var Line = Reader.ReadLine();
                if (Line == null) continue;

                Line = Line.TrimStart(' '); //Trim Starting Spaces.
                Line = Regex.Replace(Line, @"  +", " ", RegexOptions.Compiled); //Remove All Extra Spaces.
                var LineData = Line.Split(' '); //Split Into Chunks.
                Files.Add(new FileNames(LineData[2])); //Add To List For Manip.
            }

            return Files;
        }
    }
}