using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Relinquished
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Console.Title = "Relinquished";
            var ZibFileName = "";
            using (var FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "Open Yu-Gi-Oh ZIB File...";
                FileDialog.Filter = "Yu-Gi-Oh! Wolf ZIB File |*.zib";
                if (FileDialog.ShowDialog() != DialogResult.OK) return;

                ZibFileName = new FileInfo(FileDialog.FileName).Name;

                if (Directory.Exists($"{ZibFileName} Unpacked") || File.Exists($"{ZibFileName} Unpacked/Index.zib"))
                    return;

                Directory.CreateDirectory($"{ZibFileName} Unpacked");
                File.Create($"{ZibFileName} Unpacked/Index.zib").Close();
                File.SetAttributes($"{ZibFileName} Unpacked/Index.zib",
                    File.GetAttributes($"{ZibFileName} Unpacked/Index.zib") | FileAttributes.Hidden);

                using (var IndexWriter = new StreamWriter(File.Open($"{ZibFileName} Unpacked/Index.zib", FileMode.Open,
                    FileAccess.Write)))
                {
                    using (var Reader =
                        new BinaryReader(File.Open(FileDialog.FileName, FileMode.Open, FileAccess.Read)))
                    {
                        var DataStartOffset = long.MaxValue;
                        var ReadSize = 4;
                        var ZibFileSize = Reader.BaseStream.Length;
                        while (Reader.BaseStream.Position + 64 <= DataStartOffset)
                        {
                            // offset is aligned to 4 bytes
                            var CurrentStartOffset = (BitConverter.ToUInt32(Reader.ReadBytes(ReadSize).Reverse().ToArray(), 0) / 4) *4;

                            if (CurrentStartOffset == 0)
                            {
                                if (ReadSize == 4)
                                {
                                    // switch to 64 bit header format & try again
                                    Reader.BaseStream.Seek(0, SeekOrigin.Begin);
                                    ReadSize = 8;
                                    continue;
                                }
                                else
                                {
                                    throw new Exception("Not valid ZIB File");
                                }
                            }

                            var CurrentFileSize = (int)BitConverter.ToUInt32(Reader.ReadBytes(ReadSize).Reverse().ToArray(), 0);
                            var CurrentFileName = Utilities.GetText(Reader.ReadBytes(64 - ReadSize * 2));

                            // sanity check
                            if (CurrentStartOffset + CurrentFileSize > ZibFileSize) throw new Exception("Not valid ZIB File");

                            // update first known file offset
                            if (CurrentStartOffset < DataStartOffset) DataStartOffset = CurrentStartOffset;

                            Utilities.Log($"Exporting {CurrentFileName} ({CurrentFileSize} Bytes)",
                                Utilities.Event.Information);

                            var SnapBack = Reader.BaseStream.Position;
                            Reader.BaseStream.Position = CurrentStartOffset;
                            using (var Writer = new BinaryWriter(File.Open($"{ZibFileName} Unpacked/" + CurrentFileName,
                                FileMode.Create, FileAccess.Write)))
                            {
                                Writer.Write(Reader.ReadBytes(CurrentFileSize));
                                IndexWriter.Write(CurrentFileName + "\n");
                            }

                            Reader.BaseStream.Position = SnapBack;
                        }
                    }
                }
            }
        }
    }
}