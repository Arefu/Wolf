using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;
using Celtic_Guardian;

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
            if (!File.Exists($"{Args[0]}\\.zib")) //To Stop Packing Random Files.
                Utilities.Log("Not an unpacked ZIB!", Utilities.Event.Error, true, 1);

            if (Args.Any(Arg => Arg == "-autovortex"))
            {
                //Look For Vortex && YGO_DATA
            }

            long DataStartOffset = 0x0; //Photos Should Start At This Point
            int OffsetReadSize = 0x0, SizeReadSize = 0x0, FileNameReadSize = 0x0; //These Should Add Up To 64.

            var ExtractedFolderName = Path.GetFileName(new FileInfo(Args[0]).Directory.Name).Replace("Unpacked_", string.Empty);
            switch (ExtractedFolderName)
            {
                case "cardcropHD400.jpg.zib":
                    OffsetReadSize = 8; //Write 8 Bytes, Fill 00 In Front
                    SizeReadSize = 8; //Write 8 Bytes, Fill 00 In Front
                    FileNameReadSize = 48; //Write 9> Bytes, Fill 00 In Back.
                    DataStartOffset = 0x69F10; //Probably Won't Be Needed. But Have It Here Incase.
                    break;

                case "cardcropHD401.jpg.zib":
                    OffsetReadSize = 8;
                    SizeReadSize = 8;
                    FileNameReadSize = 48;
                    DataStartOffset = 0xC810;
                    break;

                case "busts.zib":
                    OffsetReadSize = 4;
                    SizeReadSize = 4;
                    FileNameReadSize = 56;
                    DataStartOffset = 0x2390;
                    break;

                case "decks.zib":
                    OffsetReadSize = 4;
                    SizeReadSize = 4;
                    FileNameReadSize = 56;
                    DataStartOffset = 0x8650;
                    break;

                case "packs.zib":
                    OffsetReadSize = 4;
                    SizeReadSize = 4;
                    FileNameReadSize = 56;
                    DataStartOffset = 0x750;
                    break;

                default:
                    Utilities.Log("Unkown ZIB File! What!?!", Utilities.Event.Error, true, 1);
                    break;
            }

            //File.Delete($"{Args[0]}\\.zib"); //Clean Up.

            foreach (var Item in Directory.GetFiles(Args[0]))
            {
                var FileInformation = new FileInfo(Item);
                var DataStart = Utilities.DirSize(new DirectoryInfo(Args[0]));

                Data.Add(new PackData("","",""));
                //Get Size
                //Get Name
                //Figure Out How To Store LATER in File
            }
        }
    }
}
