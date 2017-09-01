using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Relinquished
{
    internal class Program
    {
        public static List<FileData> Data = new List<FileData>();

        [STAThread]
        private static void Main()
        {
            Console.Title = "Relinquished";
            using (var FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "Open Yu-Gi-Oh ZIB File...";
                FileDialog.Filter = "Yu-Gi-Oh! LOTD ZIB File |*.zib";
                if (FileDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    var ZibFileName = new FileInfo(FileDialog.FileName).Name;

                    if (Directory.Exists($"{ZibFileName} Unpacked") || File.Exists($"{ZibFileName} Unpacked/.zib"))
                        return;

                    Directory.CreateDirectory($"{ZibFileName} Unpacked");
                    File.Create($"{ZibFileName} Unpacked/.zib");
                    File.SetAttributes($"{ZibFileName} Unpacked/.zib",
                        File.GetAttributes($"{ZibFileName} Unpacked/.zib") | FileAttributes.Hidden);

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
                    }
                    using (var Reader =
                        new BinaryReader(File.Open(FileDialog.FileName, FileMode.Open, FileAccess.Read)))
                    {
                        var AmountofFiles = 0;
                        while (Reader.BaseStream.Position + 64 <= DataStartOffset)
                        {
                            var CurrentChunk = Reader.ReadBytes(64); //40 In HEX is 64 in DEC
                            var CurrentStartOffset = Utilities.HexToDec(CurrentChunk.Take(OffsetReadSize).ToArray());
                            CurrentChunk = CurrentChunk.Skip(OffsetReadSize).ToArray();
                            var CurrentFileSize = Utilities.HexToDec(CurrentChunk.Take(SizeReadSize).ToArray(), true);
                            CurrentChunk = CurrentChunk.Skip(SizeReadSize).ToArray();
                            var CurrentFileName = Utilities.GetText(CurrentChunk.Take(FileNameReadSize).ToArray());

                            //Start Offset Is WRONG In ZIB For Some Reason, or maybe I am...
                            if (CurrentFileName == "adriangecko_neutral.png")
                                CurrentStartOffset = 0x2390;

                            Utilities.Log($"Exporting {CurrentFileName} ({CurrentFileSize} Bytes)",
                                Utilities.Event.Information);

                            var SnapBack = Reader.BaseStream.Position;
                            Reader.BaseStream.Position = CurrentStartOffset;
                            using (var Writer = new BinaryWriter(File.Open($"{ZibFileName} Unpacked/" + CurrentFileName,
                                FileMode.Create, FileAccess.Write)))
                            {
                                Writer.Write(Reader.ReadBytes(CurrentFileSize));
                                Writer.Close();
                            }
                            Reader.BaseStream.Position = SnapBack;
                            AmountofFiles++;
                        }
                        Console.WriteLine(AmountofFiles);
                    }
                }
                catch (Exception Ex)
                {
                    Utilities.Log($"Exception Caught: {Ex.Message}", Utilities.Event.Error, true, 1);
                }
            }
        }
    }
}