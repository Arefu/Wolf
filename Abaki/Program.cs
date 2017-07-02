using System;
using System.IO;
using System.Text;
using Celtic_Guardian;

namespace Abaki
{
    internal class Program
    {
        private static void Main(string[] Args)
        {
            Console.Title = "Abaki";
            if (Args.Length <= 0)
                Utilities.Log("Please Drag A BIN File On To Me!", Utilities.Event.Information, true, 1);
            if (!Utilities.IsExt(Args[0], ".bin"))
                Utilities.Log("Please Make Sure You're Using A BIN File!", Utilities.Event.Information, true, 1);

            var BinFileName = new FileInfo(Args[0]).Name;

            if (!File.Exists(BinFileName + ".txt"))
                File.Create(BinFileName + ".txt");

            var Line = new StringBuilder();
            byte Break = 0x2E;
            using (var Reader = new BinaryReader(File.Open(Args[0], FileMode.Open, FileAccess.Read)))
            {
                while (Reader.BaseStream.Position != Reader.BaseStream.Length)
                {
                    var CurrentByte = Reader.ReadBytes(1);
                    if (CurrentByte[0] != Break)
                    {
                        Line.Append(Utilities.GetRealTextFromByteArray(CurrentByte, true));
                    }
                    else
                    {
                        Console.WriteLine(Line + Environment.NewLine);
                        Line.Clear();
                    }
                }
            }
        }
    }
}