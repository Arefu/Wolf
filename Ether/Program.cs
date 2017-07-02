using System;
using System.IO;
using System.Windows.Forms;
using Celtic_Guardian;

namespace Ether
{
    internal class Program
    {
        [STAThread]
        private static void Main()
        {
            Console.Title = "Ether";
            int Choice;
            Console.WriteLine("Welcome To Ether." + Environment.NewLine);
            PrintCommandList();
            do
            {
                Console.Write("Choice: ");
                int.TryParse(Console.ReadLine(), out Choice);
                string Result;
                switch (Choice)
                {
                    case 1:
                        Console.Write("Number To Translate: ");
                        Console.WriteLine(143.ToString("X"));
                        Result = Utilities.DecToHex(Console.ReadLine());
                        Console.WriteLine($"Result: {Result}");
                        break;
                    case 2:
                        Console.Write("Hex To Translate: ");
                        Result = Utilities.HexToDec(Console.ReadLine()).ToString();
                        Console.WriteLine($"Result: {Result}");
                        break;
                    case 3:
                        using (var Ofd = new OpenFileDialog())
                        {
                            Ofd.Title = "Open File To Get Decimal Size For...";
                            Ofd.Multiselect = false;
                            if (Ofd.ShowDialog() != DialogResult.OK) return;
                            Result = Ofd.FileName;
                            Console.WriteLine($"Result: {new FileInfo(Result).Length} Bytes.");
                        }
                        break;
                    case 4:
                        using (var Ofd = new OpenFileDialog())
                        {
                            Ofd.Title = "Open File To Get Hexadecimal Size For...";
                            Ofd.Multiselect = false;
                            if (Ofd.ShowDialog() != DialogResult.OK) return;
                            Result = Ofd.FileName;
                            Console.WriteLine(
                                $"Result: {Utilities.DecToHex(new FileInfo(Result).Length.ToString())} Bytes.");
                        }
                        break;
                }
            } while (Choice != 9);
        }

        private static void PrintCommandList()
        {
            Console.WriteLine("------NUMBER TRANSLATION------");
            Console.WriteLine("1: Translate Decimal To Hexadecimal.");
            Console.WriteLine("2: Translate Hexadecimal To Decimal.");

            Console.WriteLine("");

            Console.WriteLine("------FILE INFORMATION------");
            Console.WriteLine("3: Get Dec File Size");
            Console.WriteLine("4: Get Hex File Size.");

            Console.WriteLine("");

            Console.WriteLine("9: Quit");
        }
    }
}