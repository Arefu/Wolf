using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

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

            using (var Writer = new BinaryWriter(File.Open(ZibFolder.Replace("Unpacked", ""), FileMode.CreateNew, FileAccess.Write)))
            {
                var CurrentOffset = (Directory.GetFiles(ZibFolder).Length - 1) * 64 + 16; //First should be Number of Files * 64 + 16.
                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var OffSet = CurrentOffset;
                    var CurrentFileSize = Utilities.DecToHex(new FileInfo($"{ZibFolder}\\{FileToPack}").Length.ToString());
                    CurrentOffset += Convert.ToInt32(Utilities.HexToDec(CurrentFileSize));

                    var CurrentSafeFileName = new FileInfo(FileToPack).Name; //ONLY the file name not the whole dir.
                    Writer.Write(new byte[] { 0x00, 0x00, 0x00, 0x00 }); //Write 4 Byte Buffer
                    var OffsetSize = Utilities.DecToHex(OffSet.ToString()).Length;
                    while (OffsetSize < 6)
                    {
                        Writer.Write((byte)0x00);
                        OffsetSize++;
                    }
                    Writer.Write(OffSet); //Array Reverse :/

                    //Write Start Offset filled to 4 bytes
                    //write 4 byte buffer 
                    //write File size to 4 bytes
                    //Write 4 File name.
                    //Write till total == 64 bytes
                    return;
                }
            }
        }
    }
}