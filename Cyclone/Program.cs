using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Celtic_Guardian;

namespace YGOPRODraft
{
    public class Cyclone
    {
        [STAThread]
        private static void Main()
        {
            //Load ZIB.
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

            if (!File.Exists($"{ZibFolder}\\Index.zib"))
                throw new Exception("Index.ZIB Not Found; Not Unpacked Correctly!");

            var FileNamesToReadInOrder = File.ReadAllLines($"{ZibFolder}\\Index.zib").ToList();

            using (var Writer = new BinaryWriter(File.Open(ZibFolder.Replace(" Unpacked", string.Empty), FileMode.OpenOrCreate, FileAccess.Write)))
            {
                var CurrentOffset = (uint)(Directory.GetFiles(ZibFolder).Length - 1) * 64 + 16; //First should be Number of Files * 64 + 16.
                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var CurrentFileSize = new FileInfo($"{ZibFolder}\\{FileToPack}").Length;

                    //File OffSet.
                    Writer.Write(new byte[4]);
                    Writer.Write(SwapBytes(CurrentOffset));

                    //File Size.
                    Writer.Write(new byte[4]);
                    Writer.Write(SwapBytes((uint)new FileInfo($"{ZibFolder}\\{FileToPack}").Length));

                    //File Name.
                    Writer.Write(Encoding.ASCII.GetBytes(FileToPack));
                    Writer.Write(FileToPack.Length == 9 ? new byte[39] : new byte[40]);

                    CurrentOffset += Convert.ToUInt32(16 * ((CurrentFileSize + 15) / 16));
                }

                ////Also add 16 bytes of padding here.
                Writer.Write(new byte[16]);

                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    byte[] bytes = File.ReadAllBytes($"{ZibFolder}\\{FileToPack}");
                    int numBytesWithPadding = 16 * ((bytes.Length + 15) / 16);
                    Writer.Write(bytes);

                    //Pad to multiples of 16 bytes, aligned.
                   var fill = new byte[numBytesWithPadding - bytes.Length];
                    Writer.Write(fill);
                    }

                //TODO: need to add padding to 16 bytes
            }
        }
        public static uint SwapBytes(uint x)
        {
            x = (x >> 16) | (x << 16);
            return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
        }
    }
}