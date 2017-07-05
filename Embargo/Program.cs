using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Celtic_Guardian;

namespace Embargo
{
    internal class Program
    {
        private static void Main(string[] Args)
        {
            Console.Title = "Embargo";
            if (Args.Length <= 0)
                Utilities.Log("Please Specify A DLL To \"Inject\"!", Utilities.Event.Error, true, 1);
            if (!Utilities.IsExt(Args[0], ".dll"))
                Utilities.Log("This File Isn't A DLL...", Utilities.Event.Error, true, 1);

            var Result = Injector.Inject("notepad",Args[0]);

            Utilities.Log(Result.ToString(),Utilities.Event.Information);
        }
    }
}
