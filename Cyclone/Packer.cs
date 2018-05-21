using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Celtic_Guardian;

namespace Cyclone
{
    internal class Packer
    {
        internal static void Pack(string ZibFolder)
        {
            if (!File.Exists($"{ZibFolder}\\Index.zib"))
                throw new Exception("Index.ZIB Not Found; Not Unpacked Correctly!");

            var FileNamesToReadInOrder = File.ReadAllLines($"{ZibFolder}\\Index.zib").ToList();

            using (var Writer = new BinaryWriter(File.Open(ZibFolder.Replace(" Unpacked", string.Empty),
                FileMode.OpenOrCreate, FileAccess.Write)))
            {
                var CurrentOffset =
                    (uint) (Directory.GetFiles(ZibFolder).Length - 1) * 64 +
                    16; //First should be Number of Files * 64 + 16.
                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var CurrentFileSize = new FileInfo($"{ZibFolder}\\{FileToPack}").Length;

                    //File OffSet.
                    Writer.Write(new byte[4]);
                    Writer.Write(Utilities.SwapBytes(CurrentOffset));

                    //File Size.
                    Writer.Write(new byte[4]);
                    Writer.Write(Utilities.SwapBytes((uint) new FileInfo($"{ZibFolder}\\{FileToPack}").Length));

                    //File Name.
                    Writer.Write(Encoding.ASCII.GetBytes(FileToPack));
                    Writer.Write(FileToPack.Length == 9 ? new byte[39] : new byte[40]);

                    CurrentOffset += Convert.ToUInt32(16 * ((CurrentFileSize + 15) / 16));
                }

                Writer.Write(new byte[16]);

                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var FileData = File.ReadAllBytes($"{ZibFolder}\\{FileToPack}");
                    var FileDataPadding = 16 * ((FileData.Length + 15) / 16);
                    Writer.Write(FileData);
                    Writer.Write(new byte[FileDataPadding - FileData.Length]);
                    Utilities.Log($"Packing {FileToPack} ({new FileInfo(ZibFolder + "\\" + FileToPack).Length} Bytes)",
                        Utilities.Event.Information);
                }
            }
        }

        internal static void Pack(string ZibFolder, uint CurrentOffset)
        {
            var FileNamesToReadInOrder = new List<string>();
            if (File.Exists($"{ZibFolder}\\Index.zib"))
                FileNamesToReadInOrder = File.ReadAllLines($"{ZibFolder}\\Index.zib").ToList();

            var Initial = true;
            using (var Writer = new BinaryWriter(File.Open(
                new FileInfo(ZibFolder).Name.Replace(" Unpacked", string.Empty), FileMode.OpenOrCreate,
                FileAccess.Write)))
            {
                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var OffSet = CurrentOffset;
                    var CurrentFileSize = new FileInfo($"{ZibFolder}\\{FileToPack}").Length;

                    if (Initial)
                    {
                        OffSet = OffSet + 1;
                        Initial = false;
                    }

                    Writer.Write(Utilities.SwapBytes(OffSet));
                    Writer.Write(Utilities.SwapBytes((uint) new FileInfo($"{ZibFolder}\\{FileToPack}").Length));
                    Writer.Write(Encoding.ASCII.GetBytes(FileToPack));

                    var Length = 4 + 4 + Encoding.ASCII.GetBytes(FileToPack).Length;
                    var LengthPadded = 64 * ((Length + 63) / 64);
                    Writer.Write(new byte[LengthPadded - Length]);

                    CurrentFileSize = 16 * ((CurrentFileSize + 15) / 16);
                    CurrentOffset += Convert.ToUInt32(CurrentFileSize);
                }

                Writer.Write(new byte[16]);

                foreach (var FileToPack in FileNamesToReadInOrder)
                {
                    var FileBytes = File.ReadAllBytes($"{ZibFolder}\\{FileToPack}");
                    var FileBytesWithPadding = 16 * ((FileBytes.Length + 15) / 16);
                    Writer.Write(FileBytes);
                    Writer.Write(new byte[FileBytesWithPadding - FileBytes.Length]);
                    Utilities.Log($"Packing {FileToPack} ({new FileInfo(ZibFolder + "\\" + FileToPack).Length} Bytes)",
                        Utilities.Event.Information);
                }
            }
        }
    }
}