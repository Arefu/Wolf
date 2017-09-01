using System;
using System.IO;
using System.Text;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Lithe
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Console.Title = "Lithe";
            var Credits = "";
            using (var Ofd = new OpenFileDialog())
            {
                Ofd.Title = "Select Credits To Encode/Decode";
                Ofd.Filter = "Credits file (*.dat, *.txt) | *.dat; *.txt";
                if (Ofd.ShowDialog() == DialogResult.OK)
                    Credits = Ofd.FileName;
                else
                    Environment.Exit(1);
            }
            
            if (Utilities.IsExt(Credits,".dat"))
            {
                using (var Reader = new BinaryReader(File.Open(Credits, FileMode.Open, FileAccess.Read)))
                {
                    var CreditsContent = Reader.ReadBytes((int) new FileInfo(Credits).Length);
                    File.WriteAllText("credits.txt",
                        Utilities.GetText(CreditsContent)
                            .Replace("?",
                                string
                                    .Empty)); //Two Start Characters Are A Magic Byte Letting The Game Know To In-Line Images
                    Utilities.Log("Finished Parsing.", Utilities.Event.Information);
                }
            }
            else
            {
                using (var Writer = new BinaryWriter(File.Open("credits.dat", FileMode.Append, FileAccess.Write)))
                {
                    using (var Reader = new BinaryReader(File.Open("credits.txt", FileMode.OpenOrCreate, FileAccess.Read)))
                    {
                        var Content = Reader.ReadBytes((int) Reader.BaseStream.Length);
                        Writer.Write(new byte[] {0xFF, 0xFE});
                        foreach (var Char in Encoding.ASCII.GetString(Content))
                            switch (Char)
                            {
                                case '\r':
                                    Writer.Write(new byte[] {0x0D, 0x00});
                                    break;

                                case '\n':
                                    Writer.Write(new byte[] {0x0A, 0x00});
                                    break;

                                default:
                                    Writer.Write(new byte[] {Convert.ToByte(Char), 0x00});
                                    break;
                            }
                    }
                }
                Utilities.Log("Finished Encoding.", Utilities.Event.Information);
            }
        }
    }
}