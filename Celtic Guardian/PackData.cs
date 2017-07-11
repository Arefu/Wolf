namespace Celtic_Guardian
{
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
}