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
            if (!File.Exists($"{Args[0]}\\.zib"))
                Utilities.Log("Not an unpacked ZIB!", Utilities.Event.Error, true, 1);

            if (Args.Any(Arg => Arg == "-autovortex"))
            {
                //Look For Vortex && YGO_DATA
            }
            var ExtractedFolderName = Args[0].Replace("Unpacked_", string.Empty);

            //File.Delete($"{Args[0]}\\.zib");
            var Writer = new BinaryWriter(File.Open(ExtractedFolderName, FileMode.Append, FileAccess.Write));
            foreach (var Item in Directory.GetFiles(Args[0]))
            {
                //Write Pack Logic.
            }
        }
    }
}
