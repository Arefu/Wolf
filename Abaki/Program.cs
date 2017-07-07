using System;
using System.Collections.Generic;
using System.IO;
using Celtic_Guardian;

namespace Abaki
{
    internal class Program
    {
        public static List<Offsets> OffsetList = new List<Offsets>();
        private static void Main(string[] Args)
        {
            Console.Title = "Abaki";
            if (Args.Length <= 0)
                Utilities.Log("Please Specify A BND To Parse!", Utilities.Event.Error, true, 1);
            if (!Utilities.IsExt(Args[0].ToLower(), ".bnd"))
                Utilities.Log("This File Isn't A BND.", Utilities.Event.Error, true, 1);

            var LocalizationFile = Args[0].ToLower();

            using (var Writer = new BinaryWriter(File.Open(LocalizationFile.Replace("bnd", "txt"), FileMode.OpenOrCreate, FileAccess.Write)))
            {
                using (var Reader = new BinaryReader(File.Open(LocalizationFile, FileMode.Open, FileAccess.Read)))
                {
                    Reader.ReadBytes(4); //First 4 Bytes Tell How Many Strings There Are, We Don't Care So We'll Discard Them.
                    while (Reader.BaseStream.Position != 0x11CC)  //Data Starts Here.
                    {
                        OffsetList.Add(new Offsets(Utilities.HexToDec(Reader.ReadBytes(4))));
                    }
                    for (var CurrentIndex = 0; CurrentIndex < OffsetList.Count; CurrentIndex++)
                    {
                        Reader.BaseStream.Position = OffsetList[CurrentIndex].Offset;
                        try
                        {
                            Writer.Write(Utilities.GetText(Reader.ReadBytes(OffsetList[CurrentIndex + 1].Offset -
                                                          (int)Reader.BaseStream.Position)) + "\r\n");
                        }
                        catch
                        {
                            Writer.Write(Utilities.GetText(Reader.ReadBytes((int)Reader.BaseStream.Length - OffsetList[CurrentIndex].Offset)) +
                                         "\r\n");
                        }
                    }
                }
            }
        }
    }
}