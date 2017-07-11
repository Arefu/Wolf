using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celtic_Guardian;

namespace Cyclone
{
    internal class Program
    {
        private static void Main(string[] Args)
        {
            Console.Title = "Cyclone";
            if (Args.Length <= 0)
                Utilities.Log("Please Make Sure You Point Me Towards An Unpacked ZIB File.", Utilities.Event.Error, true, 1);
            if (!File.Exists($"{Args[0]}\\.zib")) //To Stop Packing Random Files.
                Utilities.Log("Not an unpacked ZIB!", Utilities.Event.Error, true, 1);

            if (Args.Any(Arg => Arg == "-autovortex"))
            {
                if (Args[0].Contains("Vortex") || Directory.GetFiles(Args[0]).Contains("Vortex"))
                {
                    //Vortex, Copy and Pack.
                }
                else
                {
                    Utilities.Log("Can't Find Vortex!",Utilities.Event.Error);
                }
            }
        }
    }
}
