using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Microsoft.Win32;

namespace Celtic_Guardian
{
    public static class Endian
    {
        public static bool IsLittleEndian => BitConverter.IsLittleEndian;

        public static short ConvertInt16(short value)
        {
            return IPAddress.HostToNetworkOrder(value);
        }

        public static ushort ConvertUInt16(ushort value)
        {
            return (ushort) IPAddress.HostToNetworkOrder((short) value);
        }

        public static int ConvertInt32(int value)
        {
            return IPAddress.HostToNetworkOrder(value);
        }

        public static uint ConvertUInt32(uint value)
        {
            return (uint) IPAddress.HostToNetworkOrder((int) value);
        }

        public static long ConvertInt64(long value)
        {
            return IPAddress.HostToNetworkOrder(value);
        }

        public static ulong ConvertUInt64(ulong value)
        {
            return (ulong) IPAddress.HostToNetworkOrder((long) value);
        }

        public static float ConvertSingle(float value)
        {
            var buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            return BitConverter.ToSingle(buffer, 0);
        }

        public static double ConvertDouble(double value)
        {
            var buffer = BitConverter.GetBytes(value);
            Array.Reverse(buffer);
            return BitConverter.ToDouble(buffer, 0);
        }

        public static short ToInt16(byte[] value, int startIndex)
        {
            return ToInt16(value, startIndex, IsLittleEndian);
        }

        public static short ToInt16(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToInt16(value, startIndex);
            return convert ? ConvertInt16(result) : result;
        }

        public static ushort ToUInt16(byte[] value, int startIndex)
        {
            return ToUInt16(value, startIndex, IsLittleEndian);
        }

        public static ushort ToUInt16(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToUInt16(value, startIndex);
            return convert ? ConvertUInt16(result) : result;
        }

        public static int ToInt32(byte[] value, int startIndex)
        {
            return ToInt32(value, startIndex, IsLittleEndian);
        }

        public static int ToInt32(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToInt32(value, startIndex);
            return convert ? ConvertInt32(result) : result;
        }

        public static uint ToUInt32(byte[] value, int startIndex)
        {
            return ToUInt32(value, startIndex, IsLittleEndian);
        }

        public static uint ToUInt32(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToUInt32(value, startIndex);
            return convert ? ConvertUInt32(result) : result;
        }

        public static long ToInt64(byte[] value, int startIndex)
        {
            return ToInt64(value, startIndex, IsLittleEndian);
        }

        public static long ToInt64(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToInt64(value, startIndex);
            return convert ? ConvertInt64(result) : result;
        }

        public static ulong ToUInt64(byte[] value, int startIndex)
        {
            return ToUInt64(value, startIndex, IsLittleEndian);
        }

        public static ulong ToUInt64(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToUInt64(value, startIndex);
            return convert ? ConvertUInt64(result) : result;
        }

        public static float ToSingle(byte[] value, int startIndex)
        {
            return ToSingle(value, startIndex, IsLittleEndian);
        }

        public static float ToSingle(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToSingle(value, startIndex);
            return convert ? ConvertSingle(result) : result;
        }

        public static double ToDouble(byte[] value, int startIndex)
        {
            return ToDouble(value, startIndex, IsLittleEndian);
        }

        public static double ToDouble(byte[] value, int startIndex, bool convert)
        {
            var result = BitConverter.ToDouble(value, startIndex);
            return convert ? ConvertDouble(result) : result;
        }

        public static byte[] GetBytes(short value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(short value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }

        public static byte[] GetBytes(ushort value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(ushort value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }

        public static byte[] GetBytes(int value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(int value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }

        public static byte[] GetBytes(uint value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(uint value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }

        public static byte[] GetBytes(long value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(long value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }

        public static byte[] GetBytes(ulong value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(ulong value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }

        public static byte[] GetBytes(float value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(float value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }

        public static byte[] GetBytes(double value)
        {
            return GetBytes(value, IsLittleEndian);
        }

        public static byte[] GetBytes(double value, bool convert)
        {
            var result = BitConverter.GetBytes(value);
            if (convert) Array.Reverse(result);
            return result;
        }
    }

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

        public enum Language
        {
            Unknown,

            English,
            French,
            German,
            Italian,
            Spanish
        }

        public static uint ConvertUInt32(uint value)
        {
            return (uint) IPAddress.HostToNetworkOrder((int) value);
        }

        public static long ConvertInt64(long value)
        {
            return IPAddress.HostToNetworkOrder(value);
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

        public static int HexToDec(string HexValue, bool CheckAlignment = false)
        {
            var Number = int.Parse(HexValue, NumberStyles.HexNumber);
            if (CheckAlignment)
                while (Number % 4 != 0)
                    Number = Number + 1;

            return Number;
        }

        public static int ConvertToLittleEndian(byte[] Number, int Index)
        {
            return (Number[Index + 3] << 24)
                   | (Number[Index + 2] << 16)
                   | (Number[Index + 1] << 8)
                   | Number[Index];
        }

        public static int HexToDec(byte[] Data, bool CheckAlignment = false)
        {
            return HexToDec(BitConverter.ToString(Data).Replace("-", ""), CheckAlignment);
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


        public static List<FileLineInfo> ParseTocFile(bool ReturnAllInfo)
        {
            StreamReader Reader;
            var LocalVarFiles = new List<FileLineInfo>();
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
                LocalVarFiles.Add(new FileLineInfo(LineData)); //Add To List For Manip.
            }


            return LocalVarFiles;
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

        public class FileLineInfo
        {
            public FileLineInfo(IReadOnlyList<string> LineInfo)
            {
                Size = HexToDec(LineInfo[0]);
                FileNameSize = HexToDec(LineInfo[1]);
                FileName = LineInfo[2];
            }

            public long Size { get; set; }
            public long FileNameSize { get; set; }
            public string FileName { get; set; }
        }
    }
}