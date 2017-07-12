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
            var ExtractedFolderName = Path.GetFileName(new FileInfo(Args[0]).Directory.Name).Replace("Unpacked_", string.Empty);

            //File.Delete($"{Args[0]}\\.zib"); //Clean Up.
            Utilities.CreateDummyFile(ExtractedFolderName, Utilities.DirSize(new DirectoryInfo(Args[0])));

            foreach (var Item in Directory.GetFiles(Args[0]).Reverse()) //Reverse To Fill Images In, Then Offset Information. (HAS TO BE BETTER WAY D:)
            {
                var FileInformation = new FileInfo(Item);
                var FileSize = Utilities.DecToHex(FileInformation.Length.ToString());
                var FileName = FileInformation.Name;
                var FileOffset = 0x0; //HEXify, and Figure out
                var Reader = new BinaryReader();
                




            }
        }
    }
}
