using System;
using System.Windows.Forms;

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

                var ResultStatus = Injector.Inject("YuGiOh", Ofd.FileName);
                if (ResultStatus != InjectionStatus.Success)
                    MessageBox.Show("Error Status: " + ResultStatus, "Error During Inject!", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
            }
        }
    }
}