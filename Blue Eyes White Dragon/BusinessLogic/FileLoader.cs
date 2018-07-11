using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.UI_Models;

namespace Blue_Eyes_White_Dragon.BusinessLogic
{
    public static class FileLoader
    {
        public static DirectoryInfo LoadCardDir(string locationSetting)
        {
            var gameImagesLocationSetting = ConfigurationManager.AppSettings[locationSetting];
            return new DirectoryInfo(gameImagesLocationSetting);
        }

        public static List<FileInfo> FindFiles(DirectoryInfo gameImagesLocation)
        {
            return Directory
                .EnumerateFiles(gameImagesLocation.FullName)
                .Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("png"))
                .Select(x => new FileInfo(x))
                .ToList();
        }

        public static Dictionary<string, string> FindFilesAndNameAsDictionary(DirectoryInfo gameImagesLocation)
        {
            return Directory
                .EnumerateFiles(gameImagesLocation.FullName)
                .Where(file => file.ToLower().EndsWith("jpg") || file.ToLower().EndsWith("png"))
                .Select(x => new FileInfo(x))
                .ToDictionary(key => Path.GetFileNameWithoutExtension(key.Name), y => y.FullName);
        }
    }
}
