using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Embargo
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Console.Title = "Embargo";

            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Select DLL To Inject";
                Ofd.Filter = "Language file (*.dll) | *.dll";
                if (Ofd.ShowDialog() == DialogResult.OK)
                    Process.Start("Steam://run/480650");
                else
                    Environment.Exit(1);


                Thread.Sleep(1000);
                Console.ReadLine();
                Utilities.Log($"Result: {Injector.Inject("YuGiOh", Ofd.FileName)}", Utilities.Event.Information);
                Thread.Sleep(1000);
            }
        }
    }
}