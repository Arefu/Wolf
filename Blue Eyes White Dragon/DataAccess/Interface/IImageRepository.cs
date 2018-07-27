using System.IO;
using PhotoSauce.MagicScaler;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface IImageRepository
    {
        void ConvertImage(DirectoryInfo destinationPath, FileInfo imageFile, string orgName,
            ProcessImageSettings settings);
    }
}