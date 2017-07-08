using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Celtic_Guardian
{
    public static class Utilities
    {
        public enum Event
        {
            Warning = 0,
            Information = 1,
            Error = 2,
            Alert = 3
        }

        private static readonly string[] SizeSuffixes =
            {"bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};

        public static int IsAligned(int Number)
        {
            if (Number % 4 == 0) return Number;

            while (Number % 4 != 0)
                Number = Number + 1;

            return Number;
        }

        public static void Log(string Message, Event LogLevel, bool ShouldQuit = false, int ExitCode = 0)
        {
            switch (LogLevel)
            {
                case Event.Warning:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.Write("[Warning]: ");
                    Console.ResetColor();
                    break;

                case Event.Information:
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.Write("[Information]: ");
                    Console.ResetColor();
                    break;

                case Event.Error:
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.Write("[Error]: ");
                    Console.ResetColor();
                    break;

                case Event.Alert:
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write("[Information]: ");
                    Console.ResetColor();
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(LogLevel), LogLevel, null);
            }
            Console.WriteLine(Message);

            if (ShouldQuit)
                Environment.Exit(ExitCode);
        }

        public static bool IsExt(string File, string Extension)
        {
            return new FileInfo(File).Extension.ToLower() == Extension;
        }

        public static int HexToDec(string HexValue)
        {
            return Int32.Parse(HexValue, NumberStyles.HexNumber);
        }

        public static string DecToHex(string DecValue)
        {
            return Int32.Parse(DecValue).ToString("x");
        }

        public static int HexToDec(byte[] Data)
        {
            return HexToDec(BitConverter.ToString(Data).Replace("-", ""));
        }

        public static string GetRealTextFromByteArray(byte[] Data, bool RemovewhiteSpace = false)
        {
            var WorkingData = BitConverter.ToString(Data).Replace("-", "");
            var RawData = new byte[WorkingData.Length / 2];
            for (var I = 0; I < RawData.Length; I++)
                RawData[I] = Convert.ToByte(WorkingData.Substring(I * 2, 2), 16);
            if (!RemovewhiteSpace) return Encoding.ASCII.GetString(RawData);

            var Formatted = Encoding.ASCII.GetString(RawData);
            Formatted = Formatted.Replace("\0", "");
            return Formatted;
        }

        public static string ByteArrayToString(byte[] Data, bool TrimDelim = true)
        {
            var Hex = BitConverter.ToString(Data);
            return TrimDelim ? Hex.Replace("-", "") : Hex;
        }

        public static string GetText(byte[] Message, bool RemoveNull = true)
        {
            var StrContent = Encoding.ASCII.GetString(Message);

            if (RemoveNull)
                StrContent = StrContent.Replace("\0", String.Empty);

            return StrContent;
        }

        public static string GiveFileSize(long Value, int DecimalPlaces = 1)
        {
            if (Value < 0)
                return "-" + GiveFileSize(-Value);
            var I = 0;
            decimal DValue = Value;
            while (Math.Round(DValue, DecimalPlaces) >= 1000)
            {
                DValue /= 1024;
                I++;
            }
            return string.Format("{0:n" + DecimalPlaces + "} {1}", DValue, SizeSuffixes[I]);
        }

        public static bool IsImage(string FileName)
        {
            return FileName.ToLower().EndsWith("jpg") || FileName.ToLower().EndsWith("png");
        }

        public static string GetInstallDir()
        {
            string InstallDir;
            try
            {
                using (var Root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var Key =
                        Root.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 480650"))
                    {
                        InstallDir = Key?.GetValue("InstallLocation").ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw new FileNotFoundException("Can't Find Game");
            }
            return InstallDir;
        }

        public static string GetHashOfFile(string FileName)
        {
            using (var Hash = MD5.Create())
            {
                using (var Stream = File.OpenRead(FileName))
                {
                    return BitConverter.ToString(Hash.ComputeHash(Stream)).Replace("-", String.Empty).ToLower();
                }
            }
        }

        public static List<FileNames> GetFileNamesFromToc()
        {
            var Files = new List<FileNames>();

            using (var Reader = new StreamReader($"{GetInstallDir()}\\YGO_DATA.TOC"))
            {
                Reader.ReadLine(); //Dispose First Line.
                while (!Reader.EndOfStream)
                {
                    var Line = Reader.ReadLine();
                    if (Line == null) continue;

                    Line = Line.TrimStart(' '); //Trim Starting Spaces.
                    Line = Regex.Replace(Line, @"  +", " ", RegexOptions.Compiled); //Remove All Extra Spaces.
                    var LineData = Line.Split(' '); //Split Into Chunks.
                    Files.Add(new FileNames(LineData[2])); //Add To List For Manip.
                }
            }
            return Files;
        }
    }
}