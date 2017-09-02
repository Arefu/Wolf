using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Abaki
{
    internal class Program
    {
        public static List<Offsets> OffsetList = new List<Offsets>();

        [STAThread]
        private static void Main(string[] Args)
        {
            Console.Title = "Abaki";
            var BndFile = "";

            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Select File To Encode/Decode";
                Ofd.Filter = "Language file (*.bnd, *.txt) | *.bnd; *.txt";
                if (Ofd.ShowDialog() == DialogResult.OK)
                    BndFile = Ofd.FileName;
                else
                    Environment.Exit(1);
            }

            if (Utilities.IsExt(BndFile, ".bnd"))
            {
                using (var Reader = new BinaryReader(File.Open(BndFile, FileMode.Open, FileAccess.Read)))
                {
                    Reader.ReadBytes(4); //Firt 4 Bytes is Number of Strings
                    int Counter = 0;
                    do
                    {
                        Reader.ReadBytes(4);
                        Counter++;
                    } while (Counter < 1138); //First String Starts Here.
                    Console.WriteLine(Reader.BaseStream.Position);
                    Console.ReadLine();
                }
            }
        }
    }
}