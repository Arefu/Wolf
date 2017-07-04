using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Vortex
{
    internal class Program
    {
        public static List<PackData> Data = new List<PackData>();
        public static List<FileNames> Files = new List<FileNames>();
        public static string[] FilesToPack;
        private static void Main(string[] Args)
        {
            Console.Title = "Vortex";
            if (Args.Length <= 0)
                Utilities.Log("Please Make Sure You Point Me Towards The YGO_DATA Directory.", Utilities.Event.Error, true, 1);
            if (!Directory.Exists($"{Args[0]}\\YGO_DATA"))
                Utilities.Log("Can't See YGO_DATA Directory!", Utilities.Event.Error, true, 1);

            Files = Utilities.GetFileNamesFromToc();
            FilesToPack = Directory.GetFiles($"{Args[0]}\\YGO_DATA", "*.*", SearchOption.AllDirectories);

            File.AppendAllText("YGO_DATA.toc", $"UT\n");

            using (var Writer = new BinaryWriter(File.Open("YGO_DATA.dat", FileMode.Append, FileAccess.Write)))
            {
                foreach (var Item in Files)
                {
                    //Read Current File.
                    var CurrentFileName = FilesToPack?.First(File => File.Replace(".\\YGO_DATA\\", string.Empty) == Item.FileName).Replace(".\\YGO_DATA\\", string.Empty);
                    var CurrentFileNameLength = Utilities.DecToHex(CurrentFileName.Length.ToString());
                    var CurrentFileSize = Utilities.DecToHex(new FileInfo($".\\YGO_DATA\\{CurrentFileName}").Length.ToString());

                    while (CurrentFileSize.Length != 12)
                    {
                        CurrentFileSize = CurrentFileSize.Insert(0, " ");
                    }
                    while (CurrentFileNameLength.Length != 2)
                    {
                        CurrentFileNameLength = CurrentFileNameLength.Insert(0, " ");
                    }
                    var Token = $".\\YGO_DATA\\{CurrentFileName}";
                    var Reader = new BinaryReader(File.Open(Token, FileMode.Open, FileAccess.Read));
                    var NewSize = new FileInfo(Token).Length;
                    while (NewSize % 4 != 0)
                    {
                        NewSize = NewSize + 1;
                    }

                    var BufferSize = NewSize - new FileInfo(Token).Length;
                    Writer.Write(Reader.ReadBytes((int)new FileInfo(Token).Length));

                    if (BufferSize > 0)
                    {
                        while (BufferSize != 0)
                        {
                            Writer.Write(new byte[] { 00 });
                            BufferSize = BufferSize - 1;
                        }
                    }
                    File.AppendAllText("YGO_DATA.toc", $"{CurrentFileSize} {CurrentFileNameLength} {CurrentFileName}\n");
                }
            }
        }
    }
}