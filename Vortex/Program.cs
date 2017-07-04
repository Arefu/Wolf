using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
            if (Args.Length <= 0)
                Utilities.Log("Please Make Sure You Point Me Towards The YGO_DATA Directory.", Utilities.Event.Error, true, 1);
            if (!Directory.Exists($"{Args[0]}\\YGO_DATA"))
                Utilities.Log("Can't See YGO_DATA Directory!", Utilities.Event.Error, true, 1);

            int Total = 0;
            foreach (var Item in Directory.GetFiles($"{Args[0]}\\YGO_DATA", "*.*", SearchOption.AllDirectories))
            {
                var Item1 = Utilities.DecToHex(new FileInfo(Item).Length.ToString()); //Size
                var Item2 = Utilities.DecToHex(Item.Split(new[] { "YGO_DATA\\" }, StringSplitOptions.None)[1].Length.ToString()); //File Name Length
                var Item3 = Item.Split(new[] { "YGO_DATA\\" }, StringSplitOptions.None)[1]; //File Name

                using (var Writer = new BinaryWriter(File.Open("YGO_DATA.dat", FileMode.Append, FileAccess.Write)))
                {
                    using (var Reader = new BinaryReader(File.Open(Item, FileMode.Open, FileAccess.Read)))
                    {
                        var FileSize = new FileInfo(Item).Length;
                        while (FileSize % 4 != 0)
                        {
                            FileSize = FileSize + 1;
                        }
                        var BufferSize = FileSize - new FileInfo(Item).Length;

                        var Chunk = Reader.ReadBytes((int)new FileInfo(Item).Length);
                        Writer.Write(Chunk);
                        Utilities.Log($"Packing {Item3}. It has the size: {Item1}.", Utilities.Event.Information);
                        if (BufferSize > 0)
                        {
                            Utilities.Log($"{Item3} Size Not Alligned To 4 Bytes.",Utilities.Event.Warning);
                            Total = Total + 1;
                            while (BufferSize != 0)
                            {
                                Writer.Write(new byte[] {00});
                                BufferSize = BufferSize - 1;
                            }
                        }
                    }
                }
                
                Data.Add(new PackData(Item1, Item2, Item3));
            }
            File.AppendAllText("YGO_DATA.toc", $"UT{Environment.NewLine}");
            //Read Original TOC File, Input Updated Values.
            //Game Reads TOC Information In "Order", It'll Either Crash Or Just Break A Few Things.
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
                if (Convert.ToInt32(Utilities.HexToDec(Item.Item2)) != Item.Item3.Length)
                    Utilities.Log($"{Item.Item3} File name length doesn't match!", Utilities.Event.Error);
                File.AppendAllText("YGO_DATA.toc", $"{Item.Item1} {Item.Item2} {Item.Item3}\n");
                Utilities.Log($"Wrote {Item.Item3} to TOC file.", Utilities.Event.Information);
            }
            File.AppendAllText("YGO_DATA.toc", $"{Environment.NewLine}");
            Console.WriteLine(Total);
        }
        
    }
}