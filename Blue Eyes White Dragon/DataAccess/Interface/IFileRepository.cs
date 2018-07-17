using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface IFileRepository
    {
        FileInfo ErrorImage { get; set; }

        DirectoryInfo LoadCardDir(string locationSetting);
        List<FileInfo> FindFiles(DirectoryInfo gameImagesLocation);
        Dictionary<string, string> FindFilesAndNameAsDictionary(DirectoryInfo gameImagesLocation);
        FileInfo FindImageFile(Card replacementCard, DirectoryInfo imagesLocation);
        string SaveArtworkMatchToFile(List<Artwork> artworkList);
        bool GetPngDimension(string fileName, out int width, out int height);
        bool GetJpegDimension(string fileName, out int width, out int height);
        List<Artwork> LoadArtworkMatchFromFile(string path);
    }
}