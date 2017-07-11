using Celtic_Guardian;
using System;
using System.IO;
using System.Text;

namespace Lithe
{
    internal class Program
    {
        private static void Main(string[] Args)
        {
            Console.Title = "Lithe";
            if (Args.Length <= 0)
                Utilities.Log("Please Drag The \"credits.dat\" File On To Me!", Utilities.Event.Error, true, 1);
            if (!Args[0].ToLower().EndsWith("credits.dat") && !Args[0].ToLower().EndsWith("credits.txt"))
                Utilities.Log("\"credits.dat\" or \"credits.txt\" not specified!", Utilities.Event.Error, true, 1);

            var Credits = Args[0];

            var PackOrUnpack = Credits.EndsWith("credits.dat"); //False Pack, True Parse.

            if (PackOrUnpack)
            {
                using (var Reader = new BinaryReader(File.Open(Credits, FileMode.Open, FileAccess.Read)))
                {
                    var CreditsContent = Reader.ReadBytes((int)new FileInfo(Credits).Length);
                    File.WriteAllText("credits.txt",
                        Utilities.GetText(CreditsContent).Replace("?", String.Empty)); //Two Start Characters Are A Magic Byte Letting The Game Know To In-Line Images
                    Utilities.Log("Finished Parsing.", Utilities.Event.Information);
                }
            }
            else
            {
                using (var Writer = new BinaryWriter(File.Open("credits.dat", FileMode.Append, FileAccess.Write)))
                {
                    using (var Reader = new BinaryReader(File.Open("credits.txt", FileMode.Open, FileAccess.Read)))
                    {
                        var Content = Reader.ReadBytes((int)Reader.BaseStream.Length);
                        Writer.Write(new byte[] { 0xFF, 0xFE });
                        foreach (var Char in Encoding.ASCII.GetString(Content))
                            switch (Char)
                            {
                                case '\r':
                                    Writer.Write(new byte[] { 0x0D, 0x00 });
                                    break;

                                case '\n':
                                    Writer.Write(new byte[] { 0x0A, 0x00 });
                                    break;

                                default:
                                    Writer.Write(new byte[] { Convert.ToByte(Char), 0x00 });
                                    break;
                            }
                    }
                }
                Utilities.Log("Finished Encoding.", Utilities.Event.Information);
            }
        }
    }
}