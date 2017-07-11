namespace Celtic_Guardian
{
    /// <summary>
    ///     Item1: Start Offset
    ///     Item2: File Size
    ///     Item3: File Name
    /// </summary>
    public class ZibData
    {
        public ZibData(string Item1, string Item2, string Item3)
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