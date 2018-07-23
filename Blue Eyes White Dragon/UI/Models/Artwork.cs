using System.Collections.Generic;
using System.IO;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.UI.Interface;
using Newtonsoft.Json;

namespace Blue_Eyes_White_Dragon.UI.Models
{
    public class Artwork : IUiModel
    {
        public int CardId { get; set; }
        
        public FileInfo GameImageFile { get; set; }
        public string GameImageFilePath => GameImageFile?.FullName ?? Constants.StringError;
        public string GameImageFileName => GameImageFile?.Name ?? Constants.StringError;
        public string GameImageMonsterName { get; set; }
        public int GameImageHeight { get; set; }
        public int GameImageWidth { get; set; }

        public FileInfo ReplacementImageFile { get; set; }
        public string ReplacementImageFilePath => ReplacementImageFile?.FullName ?? Constants.StringError;
        public string ReplacementImageFileName => ReplacementImageFile?.Name ?? Constants.StringError;
        public string ReplacementImageMonsterName { get; set; }
        public int ReplacementImageHeight { get; set; }
        public int ReplacementImageWidth { get; set; }

        public bool IsMatched { get; set; }
        public bool IsPendulum { get; set; }
        public List<FileInfo> AlternateReplacementImages { get; set; } = new List<FileInfo>();

        public override string ToString()
        {
            return $"GameImageFilePath:{GameImageFilePath} GameImageMonsterName:{GameImageMonsterName} " +
                   $"ReplacementImageFilePath:{ReplacementImageFilePath} ReplacementImageMonsterName:{ReplacementImageMonsterName}";
        }
    }
}

