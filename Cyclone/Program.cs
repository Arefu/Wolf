using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Cyclone
{
    internal class Program
    {
        public static List<PackData> Data = new List<PackData>();

        [STAThread]
        private static void Main()
        {
            Console.Title = "Cyclone";

            var ZibFolder = "";
            using (var Fbd = new FolderBrowserDialog())
            {
                Fbd.ShowNewFolderButton = false;
                Fbd.Description = "Select the ZIB folder.";
                Fbd.SelectedPath = Application.StartupPath;

                if (Fbd.ShowDialog() != DialogResult.OK)
                    Environment.Exit(1);
                else
                    ZibFolder = Fbd.SelectedPath;
            }

            var FileNamesToReadInOrder = new List<string>();
            if (File.Exists($"{ZibFolder}\\Index.zib"))
                FileNamesToReadInOrder = File.ReadAllLines($"{ZibFolder}\\Index.zib").ToList();

            using (var Writer = new BinaryWriter(File.Open(ZibFolder.Replace("Unpacked", ""), FileMode.OpenOrCreate, FileAccess.Write)))
            {
                var CurrentOffset = (Directory.GetFiles(ZibFolder).Length - 1) * 64 + 16; //First should be Number of Files * 64 + 16.
                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var OffSet = CurrentOffset;
                    var CurrentFileSize = new FileInfo($"{ZibFolder}\\{FileToPack}").Length.ToString();
                    CurrentOffset += Convert.ToInt32(CurrentFileSize);

                    Writer.Write(0x00);
                    Writer.Write(SwapBytes(OffSet));
                    Writer.Write(0x00);
                    Writer.Write(SwapBytes((int)new FileInfo($"{ZibFolder}\\{FileToPack}").Length));

                    Writer.Write(Encoding.ASCII.GetBytes(FileToPack));
                    Writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 });

                }
                foreach (var FileToPack in FileNamesToReadInOrder)
                    Writer.Write(File.ReadAllBytes($"{ZibFolder}\\{FileToPack}"));
            }
        }

        public static int SwapBytes(int Bytes)
        {
            Bytes = (Bytes >> 16) | (Bytes << 16);
            return (int)((Bytes & 0xFF00FF00) >> 8) | ((Bytes & 0x00FF00FF) << 8);
        }
    }
}