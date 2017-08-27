using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace Flatiron
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Console.Title = "Onomatopaira";

            using (var FileDialog = new OpenFileDialog())
            {
                FileDialog.Title = "Open Yu-Gi-Oh Sound Bank File...";
                FileDialog.Filter = "WWise BNK File |*.bnk";
                FileDialog.Multiselect = true;
                if (FileDialog.ShowDialog() != DialogResult.OK) return;

                foreach (var FileToExtract in FileDialog.FileNames)
                {
                }
            }
        }
    }
}
