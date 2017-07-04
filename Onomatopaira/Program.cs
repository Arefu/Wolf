using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Celtic_Guardian;

namespace Onomatopaira
{
    internal class Program
    {
        public static List<FileData> Data = new List<FileData>();
        public static BinaryReader DatReader; //Read The DAT.
        public static BinaryWriter FileWriter; //Be The DAT!

        private static void Main(string[] Args)
        {
            Console.Title = "Onomatopaira";
            if (Args.Length <= 0)
                Utilities.Log("Please Drag The YGO_DATA.TOC File On To Me!", Utilities.Event.Information, true, 1);
            if (!Utilities.IsExt(Args[0], ".toc"))
                Utilities.Log("Please Make Sure You're Using The .TOC File!", Utilities.Event.Information, true, 1);
            if (!Directory.Exists("YGO_DATA"))
                Directory.CreateDirectory("YGO_DATA");

            using (var Reader = new StreamReader(Args[0]))
            {
                Reader.ReadLine(); //Dispose First Line.
                while (!Reader.EndOfStream)
                {
                    var Line = Reader.ReadLine();
                    if (Line == null) continue;
                    Line = Line.TrimStart(' '); //Trim Starting Spaces.
                    Line = Regex.Replace(Line, @"  +", " ", RegexOptions.Compiled); //Remove All Extra Spaces.
                    var LineData = Line.Split(' '); //Split Into Chunks.
                    Data.Add(new FileData(Utilities.HexToDec(LineData[0]), Utilities.HexToDec(LineData[1]),
                        LineData[2])); //Add To List For Manip.
                }
            }

            DatReader = new BinaryReader(File.Open(Args[0].Replace(".toc", ".dat"), FileMode.Open));

            foreach (var Item in Data)
            {
                var ExtraByte = Item.Item1;
                if (Item.Item1 % 4 != 0) //THIS IS BULLSHIT!
                {
                    Utilities.Log($"{Item.Item3} Not Aligned To 4 Bytes!", Utilities.Event.Warning);

                    while (ExtraByte % 4 != 0)
                    {
                        ExtraByte = ExtraByte + 1;
                    }
                }

                new FileInfo("YGO_DATA/" + Item.Item3).Directory?.Create();
                Utilities.Log($"Extracting YGO_DATA/{Item.Item3}, Expected Size: {Item.Item1}",
                    Utilities.Event.Information);
                FileWriter = new BinaryWriter(File.Open("YGO_DATA/" + Item.Item3, FileMode.Create,
                    FileAccess.Write));
                FileWriter.Write(DatReader.ReadBytes(Item.Item1));
                DatReader.BaseStream.Position += ExtraByte - Item.Item1;
                FileWriter.Close();

                Utilities.Log($"Done Extracting YGO_DATA/{Item.Item3}. It's Size Is: {new FileInfo("YGO_DATA/" + Item.Item3).Length}", Utilities.Event.Information);
            }

            Utilities.Log($"Finished Exporting {Data.Count} Files!", Utilities.Event.Alert);

            DatReader?.Close();
            DatReader?.Dispose();
        }
    }
}