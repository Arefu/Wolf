using System;

namespace Celtic_Guardian
{
    /// <summary>
    ///     Item1: Start Offset / File Size.
    ///     Item2: File Size / FIle Name Length.
    ///     Item3: File Name / File Name.
    /// </summary>
    public class FileData
    {
        public FileData(int Item1, int Item2, string Item3)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
            this.Item3 = Item3;
        }

        public int Item1 { get; set; }
        public int Item2 { get; set; }
        public string Item3 { get; set; }
    }

    /// <summary>
    ///     Item1: File Size. HEX
    ///     Item2: FIle Name Length. HEX
    ///     Item3: File Name. STR
    /// </summary>
    public class PackData
    {
        public PackData(string Item1, string Item2, string Item3)
        {
            this.Item1 = Item1;
            this.Item2 = Item2;
            this.Item3 = Item3;
        }

        public string Item1 { get; set; }
        public string Item2 { get; set; }
        public string Item3 { get; set; }
    }

    public class FileNames
    {
        public FileNames(string FileName)
        {
            this.FileName = FileName;
        }

        public string FileName { get; set; }
    }

    public class Offsets
    {
        public Offsets(int Offset)
        {
            this.Offset = Offset;
        }

        public int Offset { get; set; }
    }
}