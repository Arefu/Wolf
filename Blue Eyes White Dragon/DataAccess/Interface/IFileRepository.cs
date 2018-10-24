using System.Collections.Generic;
using System.IO;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface IFileRepository
    {
        FileInfo GetImageFile(string filename, DirectoryInfo imagesLocation);
        void CalculateHeightAndWidth(IEnumerable<Artwork> artworks);
        List<string> GetSupportedFileTypes();
        bool FolderExist(string path);
    }
}