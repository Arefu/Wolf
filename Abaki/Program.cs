using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Abaki
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Console.Title = "Abaki";
            var BndFile = "";

            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Select File To Decode";
                Ofd.Filter = "Language file (*.bnd) | *.bnd";
                if (Ofd.ShowDialog() == DialogResult.OK)
                    BndFile = Ofd.FileName;
                else
                    Environment.Exit(1);
            }

            var StringIndex = new List<long>();
            using (var Reader = new BinaryReader(File.Open(BndFile, FileMode.Open, FileAccess.Read)))
            {
                var AmountOfStringsToRead = Utilities.HexToDec(Reader.ReadBytes(4));
                var Counter = 0;

                do
                {
                    StringIndex.Add(Utilities.HexToDec(Reader.ReadBytes(4)));
                    Counter++;
                } while (Counter < AmountOfStringsToRead);
            }
            UnpackStrings(ref StringIndex, BndFile);
        }

        private static void UnpackStrings(ref List<long> StringIndex, string BndFilePath)
        {
            using (var Reader = new BinaryReader(File.Open(BndFilePath, FileMode.Open, FileAccess.Read)))
            {
                Reader.ReadBytes(4); //Discard First 4 Bytes.
                for (var Count = 0; Count < StringIndex.Count; Count++)
                {
                    do
                    {
                        using (var Writer = new BinaryWriter(File.Open($"{new FileInfo(BndFilePath).DirectoryName}\\String_{Count}.txt", FileMode.OpenOrCreate, FileAccess.Write)))
                        {
                            try
                            {
                                Reader.BaseStream.Position = StringIndex[Count];
                                Writer.Write(
                                    Reader.ReadBytes((int) StringIndex[Count + 1] - (int) Reader.BaseStream.Position));
                                Count++;
                            }
                            catch
                            {
                                Reader.BaseStream.Position = StringIndex[Count];
                                Writer.Write(
                                    Reader.ReadBytes((int)Reader.BaseStream.Length - (int)Reader.BaseStream.Position));
                                Count++;
                            }
                        }
                    } while (Count < StringIndex.Count);
                }
            }
        }
    }
}