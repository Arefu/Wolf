namespace Blue_Eyes_White_Dragon.UI_Models
{
    public class ArtworkModel
    {
        /// <summary>
        /// GameImage is referred to as GI
        /// </summary>
        public string GameImagePath { get; set; }
        public string GameImageMonsterName { get; set; }
        public string GameImageFileName { get; set; }
        public int GameImageHeight { get; set; } 
        public int GameImageWidth { get; set; }

        /// <summary>
        /// ReplacementImage is reffered to as RI
        /// </summary>
        public string ReplacementImagePath { get; set; }
        public string ReplacementImageMonsterName { get; set; }
        public string ReplacementImageFileName { get; set; }
        public int ReplacementImageHeight { get; set; }
        public int ReplacementImageWidth { get; set; }
    }
}
