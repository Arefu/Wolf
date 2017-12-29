using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Celtic_Guardian
{
    public static class Utilities
    {
        public enum Event
        {
            Warning = 0,
            Information = 1,
            Error = 2,
            Alert = 3,
            Meta = 4
        }

        public static string ByteArrayToString(byte[] Bytes)
        {
            return BitConverter.ToString(Bytes).Replace("-", "");
        }

        public static uint SwapBytes(uint Number)
        {
            Number = (Number >> 16) | (Number << 16);
            return ((Number & 0xFF00FF00) >> 8) | ((Number & 0x00FF00FF) << 8);
        }

        public static long DirSize(DirectoryInfo Directory)
        {
            var Size = Directory.GetFiles().Sum(File => File.Length);
            var SubDirs = Directory.GetDirectories();
            Size += SubDirs.Sum(SubDir => DirSize(SubDir));

            return Size;
        }

        public static void CreateDummyFile(string FileName, long Length)
        {
            using (var Stream = new FileStream(FileName, FileMode.Create, FileAccess.Write, FileShare.None))
            {
                Stream.SetLength(Length);
            }
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

                case Event.Meta:
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    Console.Write("[Meta Information]: ");
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

        public static List<string> ParseTocFile()
        {
            StreamReader Reader;
            var LocalVarFiles = new List<string>();
            try
            {
                Reader = new StreamReader($"{GetInstallDir()}\\YGO_DATA.TOC");
            }
            catch (Exception)
            {
                using (var Ofd = new OpenFileDialog())
                {
                    Ofd.Title = "Select YuGiOh.exe";
                    Ofd.Filter = "YuGiOh.exe | YuGiOh.exe";
                    var Result = Ofd.ShowDialog();
                    if (Result != DialogResult.OK) Environment.Exit(1);
                    Reader = new StreamReader(File.Open($"{new FileInfo(Ofd.FileName).DirectoryName}\\YGO_DATA.TOC",
                        FileMode.Open, FileAccess.Read));
                }
            }

            Reader.ReadLine(); //Dispose First Line.
            while (!Reader.EndOfStream)
            {
                var Line = Reader.ReadLine();
                if (Line == null) continue;

                Line = Line.TrimStart(' '); //Trim Starting Spaces.
                Line = Regex.Replace(Line, @"  +", " ", RegexOptions.Compiled); //Remove All Extra Spaces.
                var LineData = Line.Split(' '); //Split Into Chunks.
                LocalVarFiles.Add(LineData[2]); //Add To List For Manip.
            }

            return LocalVarFiles;
        }

        public static int HexToDec(string HexValue, bool CheckAlignment = false)
        {
            var Number = int.Parse(HexValue, NumberStyles.HexNumber);
            if (CheckAlignment)
                while (Number % 4 != 0)
                    Number = Number + 1;

            return Number;
        }

        public static int HexToDec(byte[] Data, bool CheckAlignment = false)
        {
            return HexToDec(BitConverter.ToString(Data).Replace("-", ""), CheckAlignment);
        }

        public static string DecToHex(string DecValue)
        {
            return int.Parse(DecValue).ToString("x");
        }

        public static string DecToHex(long DecValue)
        {
            return DecValue.ToString("x");
        }

        public static string GetText(byte[] Message, bool RemoveNull = true)
        {
            var StrContent = Encoding.ASCII.GetString(Message);

            if (RemoveNull)
                StrContent = StrContent.Replace("\0", string.Empty);

            return StrContent;
        }

        public static string GiveFileSize(long Value, int DecimalPlaces = 1)
        {
            var SizeSuffixes = new[] {"Bytes", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"};
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

        public static string GetInstallDir()
        {
            try
            {
                using (var Root = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
                {
                    using (var Key =
                        Root.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Steam App 480650"))
                    {
                        return Key?.GetValue("InstallLocation").ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception("Game Not Found");
            }
        }
    }
}