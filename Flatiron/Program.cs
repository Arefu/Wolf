using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
                if (FileDialog.ShowDialog() != DialogResult.OK) return;

            }
        }
    }

    internal struct Index
    {
        private int NotKnown;
        private int StartOffset;
        private uint FileSize;
    }

    internal struct Section
    {
        private char[] Sign = new char[];
        private uint FileSize;
    }
}
