using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vortex
{
    internal class Program
    {
        public static List<PackData> Data = new List<PackData>();
        private static void Main(string[] Args)
        {
            Console.Title = "Vortex";
            if(Args.Length <= 0)
                Utilities.Log("Please Make Sure You Point Me Towards The YGO_DATA Directory.", Utilities.Event.Error, true, 1);
            if(!Directory.Exists($"{Args[0]}\\YGO_DATA"))
                Utilities.Log("Can't See YGO_DATA Directory!",Utilities.Event.Error,true,1);

            foreach (var Item in Directory.GetFiles($"{Args[0]}\\YGO_DATA", "*.*", SearchOption.AllDirectories))
            {
                var Item1 = Utilities.DecToHex(new FileInfo(Item).Length.ToString()); //Size
                var Item2 = Utilities.DecToHex(Item.Split(new [] { "YGO_DATA\\" },StringSplitOptions.None)[1].Length.ToString()); //File Name Length
                var Item3 = Item.Split(new[] {"YGO_DATA\\"}, StringSplitOptions.None)[1]; //File Name

                using (var Writer = new BinaryWriter(File.Open("YGO_DATA.dat", FileMode.Append, FileAccess.Write)))
                {
                    using (var Reader = new BinaryReader(File.Open(Item,FileMode.Open,FileAccess.Read)))
                    {
                        var Chunk  = Reader.ReadBytes((int)new FileInfo(Item).Length);
                        Writer.Write(Chunk); //GET ORIGINAL SIZE BEFORE WRITING AS THE %4 == 0 FILLER CODE BREAKS THIS! / TRIM TRAILING 00??
                        if (Utilities.HexToDec(Item1) % 4 != 0)
                        {
                            var TempSize = Utilities.HexToDec(Item1);
                            while (TempSize % 4 != 0)
                            {
                                Writer.Write(new byte[] {00});
                                TempSize++;
                            }
                        }
                    }
                }

                Data.Add(new PackData(Item1,Item2,Item3));
            }
            File.AppendAllText("YGO_DATA.toc", $"UT{Environment.NewLine}");
            foreach (var Item in Data)
            {
                while (Item.Item1.Length != 12)
                {
                    Item.Item1 = Item.Item1.Insert(0, " ");
                }
                while (Item.Item2.Length != 2)
                {
                    Item.Item2 = Item.Item2.Insert(0, " ");
                }
                File.AppendAllText("YGO_DATA.toc",$"{Item.Item1} {Item.Item2} {Item.Item3}\n");
            }
            File.AppendAllText("YGO_DATA.toc", $"{Environment.NewLine}");
        }
    }
}
