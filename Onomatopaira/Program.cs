using Celtic_Guardian;
using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Onomatopaira
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Console.Title = "Onomatopaira";

            using (var OFD = new OpenFileDialog())
            {
                OFD.Title = "Open Yu-Gi-Oh TOC File...";
                OFD.Filter = "Yu-Gi-Oh! LOTD TOC File |*.toc";
                OFD.ShowDialog();

                using (var Reader = new StreamReader(OFD.FileName))
                {
                    BinaryReader DatReader = new BinaryReader(File.Open(OFD.FileName.Replace(".toc", ".dat"), FileMode.Open));
                    Reader.ReadLine(); //Dispose First Line.
                    while (!Reader.EndOfStream)
                    {
                        var Line = Reader.ReadLine();
                        if (Line == null) continue;
                        Line = Line.TrimStart(' '); //Trim Starting Spaces.
                        Line = Regex.Replace(Line, @"  +", " ", RegexOptions.Compiled); //Remove All Extra Spaces.
                        var LineData = Line.Split(' '); //Split Into Chunks.

                        //Create Item's Folder.
                        new FileInfo("YGO_DATA/" + LineData[2]).Directory?.Create();

                        //Check Alignment
                        var ExtraBytes = Utilities.HexToDec(LineData[0]);
                        if (Utilities.HexToDec(LineData[0]) % 4 != 0)
                            while (ExtraBytes % 4 != 0)
                                ExtraBytes = ExtraBytes + 1;

                        //Write File
                        using (var FileWriter = new BinaryWriter(File.Open("YGO_DATA/" + LineData[2], FileMode.Create, FileAccess.Write)))
                        {
                            FileWriter.Write(DatReader.ReadBytes(Utilities.HexToDec(LineData[0])));
                            FileWriter.Flush();
                        }

                        //Advance Stream
                        DatReader.BaseStream.Position += ExtraBytes - Utilities.HexToDec(LineData[0]);
                    }
                }
            }
        }
    }
}