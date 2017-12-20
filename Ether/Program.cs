using System;
using System.IO;
using System.Security.Cryptography;
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
                                $"Result: {Utilities.DecToHex(new FileInfo(Result).Length)} Bytes.");
                        }

                        break;

                    case 5:
                        if (!Directory.Exists($"{Utilities.GetInstallDir()}\\Backed Up Files"))
                            Directory.CreateDirectory($"{Utilities.GetInstallDir()}\\Backed Up Files");

                        File.Copy($"{Utilities.GetInstallDir()}\\YGO_DATA.TOC",
                            $"{Utilities.GetInstallDir()}\\Backed Up Files\\YGO_DATA.TOC", true);
                        File.Copy($"{Utilities.GetInstallDir()}\\YGO_DATA.DAT",
                            $"{Utilities.GetInstallDir()}\\Backed Up Files\\YGO_DATA.DAT", true);
                        break;

                    case 6:
                        Console.WriteLine(
                            $"YGO_DATA.TOC Hash: {GetHashOfFile(Utilities.GetInstallDir() + "\\YGO_DATA.TOC")}");
                        Console.WriteLine(
                            $"YGO_DATA.DAT Hash: {GetHashOfFile(Utilities.GetInstallDir() + "\\YGO_DATA.DAT")}");
                        break;

                    case 7:
                        if (!Directory.Exists($"{Utilities.GetInstallDir()}\\Backed Up Files"))
                            Utilities.Log("Back Ups Not Found! Restore Through Steam.", Utilities.Event.Error, true, 1);

                        File.Copy($"{Utilities.GetInstallDir()}\\Backed Up Files\\YGO_DATA.TOC",
                            $"{Utilities.GetInstallDir()}\\YGO_DATA.TOC", true);
                        File.Copy($"{Utilities.GetInstallDir()}\\Backed Up Files\\YGO_DATA.DAT",
                            $"{Utilities.GetInstallDir()}\\YGO_DATA.DAT", true);
                        break;

                    case 8:
                        PrintCommandList();
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

            Console.WriteLine("------ORIGINAL FILES------");
            Console.WriteLine("5: Back Up Original TOC and DAT.");
            Console.WriteLine("6: Get MD5 Of TOC and DAT.");

            Console.WriteLine("");

            Console.WriteLine("------OTHER FUNCTIONS------");
            Console.WriteLine("7: Restore Original TOC and DAT.");
            Console.WriteLine("8: Print Menu Again.");

            Console.WriteLine("");

            Console.WriteLine("9: Quit");

            Console.WriteLine("");
        }

        private static string GetHashOfFile(string FileName)
        {
            using (var Hash = MD5.Create())
            {
                using (var Stream = File.OpenRead(FileName))
                {
                    return BitConverter.ToString(Hash.ComputeHash(Stream)).Replace("-", string.Empty).ToLower();
                }
            }
        }
    }
}