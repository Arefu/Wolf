using System;
using System.Globalization;
using System.IO;
using System.Text;

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
            return int.Parse(HexValue, NumberStyles.HexNumber);
        }

        public static int GetIntFromByteArray(byte[] Data)
        {
            return HexToDec(BitConverter.ToString(Data).Replace("-", ""));
        }

        public static string GetRealTextFromByteArray(byte[] Data, bool RemovewhiteSpace = false)
        {
            var WorkingData = BitConverter.ToString(Data).Replace("-", "");
            var RawData = new byte[WorkingData.Length / 2];
            for (var I = 0; I < RawData.Length; I++)
                RawData[I] = Convert.ToByte(WorkingData.Substring(I * 2, 2), 16);
            if (RemovewhiteSpace)
            {
                var Formatted = Encoding.ASCII.GetString(RawData);
                Formatted = Formatted.Replace("\0", "");
                return Formatted;
            }
            return Encoding.ASCII.GetString(RawData);
        }

        public static string ByteArrayToString(byte[] Data, bool TrimDelim = true)
        {
            var Hex = BitConverter.ToString(Data);
            if (TrimDelim)
                return Hex.Replace("-", "");
            return Hex;
        }

        public static string GetText(byte[] Message, bool RemoveNull = true)
        {
            var StrContent = Encoding.ASCII.GetString(Message);

            if (RemoveNull)
                StrContent = StrContent.Replace("\0", string.Empty);

            return StrContent;
        }
    }
}