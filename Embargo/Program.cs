using System;
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
                if (Ofd.ShowDialog() != DialogResult.OK)
                    Environment.Exit(1);

                Utilities.Log($"Result: {Injector.Inject("YuGiOh", Ofd.FileName)}", Utilities.Event.Information);
                Console.ReadLine();
            }
        }
    }
}