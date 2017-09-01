using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Onomatopaira
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] Arguments)
        {
            var ShouldGarbageCollect = Arguments.Any(Argument => Argument.ToLower() == "-gc");

            Console.Title = "Onomatopaira";

            using (var FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "Open Yu-Gi-Oh TOC File...";
                FileDialog.Filter = "Yu-Gi-Oh! LOTD TOC File |*.toc";
                if (FileDialog.ShowDialog() != DialogResult.OK) return;

                try
                {
                    using (var Reader = new StreamReader(FileDialog.FileName))
                    {
                        var DatReader =
                            new BinaryReader(File.Open(FileDialog.FileName.Replace(".toc", ".dat"), FileMode.Open));
                        Reader.ReadLine(); //Dispose First Line.

                        while (!Reader.EndOfStream)
                        {
                            var Line = Reader.ReadLine();
                            if (Line == null) continue;

                            Line = Line.TrimStart(' '); //Trim Starting Spaces.
                            Line = Regex.Replace(Line, @"  +", " ",
                                RegexOptions.Compiled); //Remove All Extra Spaces.
                            var LineData = Line.Split(' '); //Split Into Chunks.

                            Utilities.Log(
                                $"Extracting File: {new FileInfo(LineData[2]).Name} ({LineData[0]} Bytes)",
                                Utilities.Event.Information);

                            //Create Item's Folder.
                            new FileInfo("YGO_DATA/" + LineData[2]).Directory?.Create();

                            //Check Alignment
                            var ExtraBytes = Utilities.HexToDec(LineData[0]);
                            if (Utilities.HexToDec(LineData[0]) % 4 != 0)
                                while (ExtraBytes % 4 != 0)
                                    ExtraBytes = ExtraBytes + 1;

                            //Write File
                            using (var FileWriter =
                                new BinaryWriter(
                                    File.Open("YGO_DATA/" + LineData[2], FileMode.Create, FileAccess.Write)))
                            {
                                FileWriter.Write(DatReader.ReadBytes(Utilities.HexToDec(LineData[0])));
                                FileWriter.Flush();

                                if (ShouldGarbageCollect)
                                    GC.Collect(GC.MaxGeneration);
                            }

                            //Advance Stream
                            DatReader.BaseStream.Position += ExtraBytes - Utilities.HexToDec(LineData[0]);
                        }
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