using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Relinquished
{
    internal class Program
    {
        public static List<FileData> Data = new List<FileData>();

        private static void Main(string[] Args)
        {
            Console.Title = "Relinquished";
            if (Args.Length <= 0)
                Utilities.Log("Please Drag A ZIB File On To Me!", Utilities.Event.Error, true, 1);
            if (!Utilities.IsExt(Args[0], ".zib"))
                Utilities.Log("Please Make Sure You're Using A ZIB File!", Utilities.Event.Error, true, 1);

            var ZibFileName = new FileInfo(Args[0]).Name;

            if (!Directory.Exists($"Unpacked_{ZibFileName}") || !File.Exists($"Unpacked_{ZibFileName}\\.zib"))
            {
                Directory.CreateDirectory($"Unpacked_{ZibFileName}");
                File.Create($"Unpacked_{ZibFileName}\\.zib");
            }

            long DataStartOffset = 0x0;
            int OffsetReadSize = 0x0, SizeReadSize = 0x0, FileNameReadSize = 0x0; //These Should Add Up To 64.

            switch (ZibFileName)
            {
                case "cardcropHD400.jpg.zib":
                    OffsetReadSize = 8;
                    SizeReadSize = 8;
                    FileNameReadSize = 48;
                    DataStartOffset = 0x69F10;
                    break;

                case "cardcropHD401.jpg.zib":
                    OffsetReadSize = 8;
                    SizeReadSize = 8;
                    FileNameReadSize = 48;
                    DataStartOffset = 0xC810;
                    break;

                case "busts.zib":
                    OffsetReadSize = 4;
                    SizeReadSize = 4;
                    FileNameReadSize = 56;
                    DataStartOffset = 0x2390;
                    break;

                case "decks.zib":
                    OffsetReadSize = 4;
                    SizeReadSize = 4;
                    FileNameReadSize = 56;
                    DataStartOffset = 0x8650;
                    break;

                case "packs.zib":
                    OffsetReadSize = 4;
                    SizeReadSize = 4;
                    FileNameReadSize = 56;
                    DataStartOffset = 0x750;
                    break;

                default:
                    Utilities.Log("Unkown ZIB File! What!?!", Utilities.Event.Error, true, 1);
                    break;
            }
            
            using (var Reader = new BinaryReader(File.Open(Args[0], FileMode.Open, FileAccess.Read)))
            {
                while (Reader.BaseStream.Position + 64 <= DataStartOffset)
                {
                    var CurrentChunk = Reader.ReadBytes(64); //40 In HEX is 64 in DEC
                    var CurrentStartOffset = CurrentChunk.Take(OffsetReadSize).ToArray();
                    CurrentChunk = CurrentChunk.Skip(OffsetReadSize).ToArray();
                    var CurrentFileSize = CurrentChunk.Take(SizeReadSize).ToArray();
                    CurrentChunk = CurrentChunk.Skip(SizeReadSize).ToArray();
                    var CurrentFileName = CurrentChunk.Take(FileNameReadSize).ToArray();

                    Utilities.Log(
                        $"Found {Utilities.GetText(CurrentFileName)} At: {Utilities.HexToDec(CurrentStartOffset)} With Size: {Utilities.HexToDec(CurrentFileSize)}",
                        Utilities.Event.Information);

                    var RealOffset = Utilities.HexToDec(CurrentStartOffset);
                    var RealSize = Utilities.HexToDec(CurrentFileSize);
                    var RealName = Utilities.GetText(CurrentFileName);

                    if (RealName == "adriangecko_neutral.png") //Start Offset Is WRONG In ZIB For Some Reason.
                        RealOffset = RealOffset - 1;

                    Data.Add(new FileData(RealOffset, RealSize, RealName));
                }
                foreach (var Item in Data)
                {
                    Reader.BaseStream.Position = Item.Item1;
                    Utilities.Log($"Exporting {Item.Item3}", Utilities.Event.Information);
                    var Writer = new BinaryWriter(File.Open($"Unpacked_{ZibFileName}/" + Item.Item3, FileMode.Create,
                        FileAccess.Write));
                    Writer.Write(Reader.ReadBytes(Item.Item2));
                    Writer.Close();
                }
            }
        }
    }

}