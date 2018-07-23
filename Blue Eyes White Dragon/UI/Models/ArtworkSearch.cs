using System.IO;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.UI.Interface;

namespace Blue_Eyes_White_Dragon.UI.Models
{
    public class ArtworkSearch : IUiModel
    {
        public int CardId { get; set; }
        public string CardName { get; set; }
        public FileInfo ImageFile { get; set; }
        public string ImageFilePath => ImageFile?.FullName ?? Constants.StringError;
    }
}