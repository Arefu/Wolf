using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Flareon
{
    internal class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            Console.Title = "Flareon";

            var ZibFolder = "";
            using (var Fbd = new FolderBrowserDialog())
            {
                Fbd.ShowNewFolderButton = false;
                Fbd.Description = "Select the ZIB folder you want packed.";
                Fbd.SelectedPath = Application.StartupPath;

                if (Fbd.ShowDialog() != DialogResult.OK)
                    Environment.Exit(1);
                else
                    ZibFolder = Fbd.SelectedPath;
            }

            ZibFolder = ZibFolder.Replace("Unpacked_", "");

            if (File.Exists($"{ZibFolder}.zib"))
                File.Delete($"{ZibFolder}.zib");

        }
    }
}
