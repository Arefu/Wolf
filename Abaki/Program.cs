using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

            var LocalizationFile = Args[0];

            using (var Writer = new BinaryWriter(File.Open(LocalizationFile.Replace("BND", "TXT"), FileMode.OpenOrCreate, FileAccess.Write)))
            {
                using (var Reader = new BinaryReader(File.Open(LocalizationFile, FileMode.Open, FileAccess.Read)))
                {
                    var AmountOfStrings = Utilities.HexToDec(Reader.ReadBytes(4));
                    do
                    {
                        OffsetList.Add(new Offsets(Utilities.HexToDec(Reader.ReadBytes(4))));
                    } while (OffsetList.Count != AmountOfStrings);

                    for (var Count = 0; Count < OffsetList.Count; Count++)
                    {
                        Reader.BaseStream.Position = OffsetList[Count].Offset;
                        Console.WriteLine($"Reading From: {Reader.BaseStream.Position}");
                        //Implement Parser
                    }

                }
            }
        }
    }
}