using Celtic_Guardian;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Wolf
{
    public static class ContextMenuFunctions
    {
        public static void ExtractFile(ListViewItem Item, string ExportPath = "")
        {
            var FileToExport = Form1.Data.First(File => File.Item3.Contains(Item.Text));
            var DirectoryInfo = new FileInfo(FileToExport.Item3).Directory;
            if (DirectoryInfo != null)
                Directory.CreateDirectory(DirectoryInfo.FullName);

            Directory.CreateDirectory("Exported");
            
            var BytesToRead = 0L;
            foreach (var File in Form1.Data)
            {
                if (File.Item3 == FileToExport.Item3)
                    break;

                var AligneSize = Utilities.IsAligned(File.Item1);
                BytesToRead = AligneSize + BytesToRead;

                if (File.Item3 == FileToExport.Item3)
                    break;
            }
            using (var BReader = new BinaryReader(File.Open($"{Utilities.GetInstallDir()}\\YGO_DATA.dat", FileMode.Open, FileAccess.Read)))
            {
                if (ExportPath == "")
                    ExportPath = $"Exported\\{FileToExport.Item3}";

                using (var Writer = new BinaryWriter(File.Open(ExportPath, FileMode.OpenOrCreate, FileAccess.Write)))
                {
                    Task.Run(() =>
                    {
                        BReader.BaseStream.Position = BytesToRead;
                        Writer.Write(BReader.ReadBytes(FileToExport.Item1));
                        Writer.Close();
                        Writer.Dispose();
                        BReader.Dispose();
                    });
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

        public static void ExecUtil(ListViewItem Item)
        {
            var ProgramPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            ExtractFile(Item,$"{ProgramPath}\\{Item.Text}");
            if (Item.Text.ToLower().EndsWith(".zib"))
            {
                MessageBox.Show($"{ProgramPath}\\{Item.Text}");
                var ProcStartInfo = new ProcessStartInfo
                {
                    Arguments = $"{Item.Text}",
                    FileName = $"{ProgramPath}\\Relinquished.exe"
                };
                new Process {StartInfo = ProcStartInfo}.Start();
                File.Delete($"{ProgramPath}\\{Item.Text}");
            }
            if (Item.Text.ToLower().EndsWith("credits.dat") || Item.Text.ToLower() == "credits.dat")
            { }
                //Extract, Run Lith
        }
    }
}