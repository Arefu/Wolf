using System;
using System.IO;
using System.Text;

namespace Yu_Gi_Oh.File_Handling.Utility
{
    internal static class Extensions
    {
        public static byte[] ReadBytes(this BinaryReader reader, long count)
        {
            return reader.ReadBytes((int) count);
        }

        public static string ReadNullTerminatedString(this BinaryReader reader, Encoding encoding)
        {
            var stringBuilder = new StringBuilder();
            var streamReader = new StreamReader(reader.BaseStream, encoding);

            var startOffset = reader.BaseStream.Position;

            int intChar;
            while ((intChar = streamReader.Read()) != -1)
            {
                var c = (char) intChar;
                if (c == '\0') break;
                stringBuilder.Append(c);
            }

            var result = stringBuilder.ToString();

            reader.BaseStream.Position = startOffset + encoding.GetByteCount(result + '\0');

            return result;
        }

        public static void WriteNullTerminatedString(this BinaryWriter writer, string str, Encoding encoding)
        {
            writer.Write(encoding.GetBytes(str == null ? string.Empty : str + '\0'));
        }

        public static byte[] GetBytes(this Encoding encoding, string str, int bufferLen, int maxStringLen)
        {
            if (str == null) str = string.Empty;
            if (maxStringLen >= 0 && str.Length > maxStringLen) str = str.Substring(0, maxStringLen);

            var buffer = new byte[bufferLen];
            var tempBuffer = encoding.GetBytes(str);
            Buffer.BlockCopy(tempBuffer, 0, buffer, 0, Math.Min(bufferLen, tempBuffer.Length));
            return buffer;
        }

        public static void WriteOffset(this BinaryWriter writer, long relativeTo, int offset)
        {
            writer.Write((int) (offset - relativeTo));
        }

        public static void WriteOffset(this BinaryWriter writer, long relativeTo, uint offset)
        {
            writer.Write((uint) (offset - relativeTo));
        }

        public static void WriteOffset(this BinaryWriter writer, long relativeTo, long offset)
        {
            writer.Write(offset - relativeTo);
        }
    }
}