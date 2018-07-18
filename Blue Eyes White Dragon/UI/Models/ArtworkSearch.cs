using System.IO;
using Blue_Eyes_White_Dragon.Business;

namespace Blue_Eyes_White_Dragon.UI.Models
{
    public class ArtworkSearch
    {
        public string CardName { get; set; }
        public FileInfo ImageFile { get; set; }
        public string ImageFilePath => ImageFile?.FullName ?? Constants.StringError;
    }
}