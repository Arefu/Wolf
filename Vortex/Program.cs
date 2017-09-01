using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Vortex
{
    internal class Program
    {
        public static List<FileNames> Files = new List<FileNames>();
        public static string[] FilesToPack;

        [STAThread]
        private static void Main()
        {
            Console.Title = "Vortex";

            if (File.Exists("YGO_DATA.dat"))
                File.Delete("YGO_DATA.dat");
            if (File.Exists("YGO_DATA.toc"))
                File.Delete("YGO_DATA.toc");

            var YgoFolder = "";
            using (var Fbd = new FolderBrowserDialog())
            {
                Fbd.ShowNewFolderButton = false;
                Fbd.Description = "Select the YGO_DATA folder.";
                Fbd.SelectedPath = Application.StartupPath;

                if (Fbd.ShowDialog() != DialogResult.OK)
                    Environment.Exit(1);
                else
                    YgoFolder = Fbd.SelectedPath;
            }

            if (!YgoFolder.Contains("YGO_DATA"))
                throw new Exception("YGO_DATA Folder Not Found!");

            Files = Utilities.ParseTocFile();
            FilesToPack = Directory.GetFiles($"{YgoFolder}", "*.*", SearchOption.AllDirectories);

            File.AppendAllText("YGO_DATA.toc", "UT\n");

            using (var Writer = new BinaryWriter(File.Open("YGO_DATA.dat", FileMode.Append, FileAccess.Write)))
            {
                foreach (var Item in Files)
                {
                    var CurrentFileName = FilesToPack?.First(File => File.Contains(Item.FileName));

                    Utilities.Log($"Packing File: {CurrentFileName}.", Utilities.Event.Information);
                    var CurrentFileNameLength = Utilities.DecToHex(CurrentFileName
                        .Split(new[] {"YGO_DATA"}, StringSplitOptions.None).Last().TrimStart('\\').Length.ToString());
                    var CurrentFileSize = Utilities.DecToHex(new FileInfo($"{CurrentFileName}").Length.ToString());

                    while (CurrentFileSize.Length != 12)
                        CurrentFileSize = CurrentFileSize.Insert(0, " ");
                    while (CurrentFileNameLength.Length != 2)
                        CurrentFileNameLength = CurrentFileNameLength.Insert(0, " ");

                    var Reader = new BinaryReader(File.Open(CurrentFileName, FileMode.Open, FileAccess.Read));
                    var NewSize = new FileInfo(CurrentFileName).Length;
                    while (NewSize % 4 != 0)
                        NewSize = NewSize + 1;

                    var BufferSize = NewSize - new FileInfo(CurrentFileName).Length;
                    Writer.Write(Reader.ReadBytes((int) new FileInfo(CurrentFileName).Length));

                    if (BufferSize > 0)
                        while (BufferSize != 0)
                        {
                            Writer.Write(new byte[] {00});
                            BufferSize = BufferSize - 1;
                        }

                    File.AppendAllText("YGO_DATA.toc",
                        $"{CurrentFileSize} {CurrentFileNameLength} {CurrentFileName.Split(new[] {"YGO_DATA\\"}, StringSplitOptions.None).Last()}\n");
                }
            }
            Utilities.Log("Finished Packing Files.", Utilities.Event.Information);
        }
    }
}