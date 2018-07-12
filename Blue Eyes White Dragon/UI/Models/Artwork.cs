using System.IO;

namespace Blue_Eyes_White_Dragon.UI.Models
{
    public class Artwork
    {
        /// <summary>
        /// GameImage is referred to as GI
        /// </summary>
        public FileInfo GameImageFile { get; set; }
        public string GameImageFilePath => GameImageFile?.FullName ?? "";
        public string GameImageFileName => GameImageFile?.Name ?? "";
        public string GameImageMonsterName { get; set; }
        public DirectoryInfo GameImagesDir { get; set; }
        public int GameImageHeight { get; set; }
        public int GameImageWidth { get; set; }


        /// <summary>
        /// ReplacementImage is reffered to as RI
        /// </summary>
        public FileInfo ReplacementImageFile { get; set; }
        public string ReplacementImageFilePath => ReplacementImageFile?.FullName ?? "";
        public string ReplacementImageFileName => ReplacementImageFile?.Name ?? "";
        public string ReplacementImageMonsterName { get; set; }
        public DirectoryInfo ReplacementImagesDir { get; set; }
        public int ReplacementImageHeight { get; set; }
        public int ReplacementImageWidth { get; set; }

        public override string ToString()
        {
            return $"GameImageFilePath:{GameImageFilePath} GameImageMonsterName:{GameImageMonsterName}" +
                   $"ReplacementImageFilePath:{ReplacementImageFilePath} ReplacementImageMonsterName:{ReplacementImageMonsterName}";
        }
    }
}

