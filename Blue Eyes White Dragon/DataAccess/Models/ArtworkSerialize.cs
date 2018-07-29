using System.Collections.Generic;

namespace Blue_Eyes_White_Dragon.DataAccess.Models
{
    public class ArtworkSerialize
    {
        public int CardId { get; set; }
        public string GameImageFilePath { get; set; }
        public string GameImageMonsterName { get; set; }
        public string ReplacementImageFilePath { get; set; }
        public string ReplacementImageMonsterName { get; set; }
        public bool IsPendulum { get; set; }
        public bool IsMatched { get; set; }
        public List<string> AlternateReplacementImages { get; set; } = new List<string>();
        public string ZibFilename { get; set; }
    }
}
