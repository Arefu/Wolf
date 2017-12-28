<<<<<<< Updated upstream
using System;
=======
﻿using System;
>>>>>>> Stashed changes
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Wolf
{
    public static class ContextMenuFunctions
    {
        public static void ExtractFile(ListViewItem Item, string ExportPath = "")
        {
            var FileToExport = Form1.Data.First(File => File.Item3.Contains(Item.Text));

            var BytesToRead = 0L;
            foreach (var File in Form1.Data)
            {
                if (File.Item3 == FileToExport.Item3)
                    break;

                while (File.Item1 % 4 != 0)
                    File.Item1 = File.Item1 + 1;

                BytesToRead = File.Item1 + BytesToRead;

                if (File.Item3 == FileToExport.Item3)
                    break;
            }

            using (var BReader = new BinaryReader(File.Open($"{Form1.InstallDir}\\YGO_DATA.dat", FileMode.Open, FileAccess.Read)))
            {
                if (ExportPath == "")
                    ExportPath = $"Exported/{FileToExport.Item3}";

                new FileInfo(ExportPath).Directory?.Create();
                using (var Writer = new BinaryWriter(File.Open(ExportPath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    BReader.BaseStream.Position = BytesToRead;
                    Writer.Write(BReader.ReadBytes(FileToExport.Item1));
                    BReader.Dispose();
                }
            }
        }

        public static void ViewImage(ListViewItem Item)
        {
            var RandFileName = new Random(DateTime.Now.Millisecond).Next();
            ExtractFile(Item, $"{Path.GetTempPath()}\\{RandFileName}");
            var Viewer = new ImageViewer($"{Path.GetTempPath()}\\{RandFileName}");
            Viewer.ShowDialog();
        }
    }
}