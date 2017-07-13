using Celtic_Guardian;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cyclone
{
    internal class Program
    {
        public static List<PackData> Data = new List<PackData>();
        private static void Main(string[] Args)
        {
            Console.Title = "Cyclone";
            if (Args.Length <= 0)
                Utilities.Log("Please Make Sure You Point Me Towards An Unpacked ZIB File.", Utilities.Event.Error, true, 1);
            //if (!File.Exists($"{Args[0]}\\.zib"))
            //   Utilities.Log("Not an unpacked ZIB!", Utilities.Event.Error, true, 1);

            if (Args.Any(Arg => Arg == "-autovortex"))
            {
                //Look For Vortex && YGO_DATA
            }
            var ExtractedFolderName = Args[0].Replace("Unpacked_", string.Empty);

            //File.Delete($"{Args[0]}\\.zib");
            var Size = Utilities.DirSize(new DirectoryInfo(Args[0])) + Directory.GetFiles(Args[0]).Length * 0x40;
            long DataStart = Directory.GetFiles(Args[0]).Length * 0x40 - 0x30;


            if (File.Exists(ExtractedFolderName))
                File.Delete(ExtractedFolderName);

            Utilities.CreateDummyFile(ExtractedFolderName, Size);

            //Convert Stuff From String To RAW BYTES, E.G. The Data In The String
            //Reimplement switch to determine what ZIB
            //Memes...
            var Writer = new BinaryWriter(File.Open(ExtractedFolderName, FileMode.Append, FileAccess.Write));
            foreach (var Item in Directory.GetFiles(Args[0]))
            {
                var ItemInfo = new FileInfo(Item);
                var StartOffset = Utilities.DecToHex(DataStart.ToString());
                var StartSize = Utilities.DecToHex(ItemInfo.Length.ToString());
                var StartName = String.Join("", ItemInfo.Name.Select(Letter => $"{Convert.ToInt32(Letter):X}"));

                var NewSize = ItemInfo.Length;
                while (NewSize % 4 != 0)
                    NewSize = NewSize + 1;

                NewSize = NewSize - ItemInfo.Length;


                StartOffset = StartOffset.PadLeft(8, '0');
                StartSize = StartSize.PadLeft(8, '0');
                StartName = StartName.PadRight(4, '0');
                
                Writer.Write(System.Text.Encoding.ASCII.GetBytes(StartOffset));

                if (NewSize > 0)
                    while (NewSize != 0)
                    {
                        Writer.Write(new byte[] { 00 });
                        NewSize = NewSize - 1;
                    }
                DataStart = DataStart + ItemInfo.Length + NewSize;
                return;
                ;
            }
            Console.ReadLine();
        }
    }
}
