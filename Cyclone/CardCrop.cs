using System;
using System.IO;
using System.Linq;
using System.Text;
using Celtic_Guardian;

namespace Cyclone
{
    internal static class CardCrop
    {
        internal static void Extract(int OffsetBuffer, int SizeBuffer, int FileNameBuffer, int FileNameExtraBuffer, int FileLenghtCheck, string ZibFolder)
        {
            if (!File.Exists($"{ZibFolder}\\Index.zib"))
                throw new Exception("Index.ZIB Not Found; Not Unpacked Correctly!");

            var FileNamesToReadInOrder = File.ReadAllLines($"{ZibFolder}\\Index.zib").ToList();

            using (var Writer = new BinaryWriter(File.Open(ZibFolder.Replace(" Unpacked", string.Empty), FileMode.OpenOrCreate, FileAccess.Write)))
            {
                var CurrentOffset = (uint) (Directory.GetFiles(ZibFolder).Length - 1) * 64 + 16; //First should be Number of Files * 64 + 16.
                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var CurrentFileSize = new FileInfo($"{ZibFolder}\\{FileToPack}").Length;

                    //File OffSet.
                    Writer.Write(new byte[OffsetBuffer]);
                    Writer.Write(Utilities.SwapBytes(CurrentOffset));

                    //File Size.
                    Writer.Write(new byte[SizeBuffer]);
                    Writer.Write(Utilities.SwapBytes((uint) new FileInfo($"{ZibFolder}\\{FileToPack}").Length));

                    //File Name.
                    Writer.Write(Encoding.ASCII.GetBytes(FileToPack));
                    Writer.Write(FileToPack.Length == FileLenghtCheck ? new byte[FileNameBuffer] : new byte[FileNameExtraBuffer]);

                    CurrentOffset += Convert.ToUInt32(16 * ((CurrentFileSize + 15) / 16));
                }

                Writer.Write(new byte[16]);

                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var FileData = File.ReadAllBytes($"{ZibFolder}\\{FileToPack}");
                    var FileDataPadding = 16 * ((FileData.Length + 15) / 16);
                    Writer.Write(FileData);
                    Writer.Write(new byte[FileDataPadding - FileData.Length]);
                }
            }
        }
    }
}