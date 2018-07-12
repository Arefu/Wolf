using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class FileRepository : IFileRepository
    {
        public DirectoryInfo LoadCardDir(string locationSetting)
        {
            return new DirectoryInfo(locationSetting);
        }

        public List<FileInfo> FindFiles(DirectoryInfo gameImagesLocation)
        {
            return Directory
                .EnumerateFiles(gameImagesLocation.FullName)
                .Where(IsFileTypeSupported)
                .Select(x => new FileInfo(x))
                .ToList();
        }

        public Dictionary<string, string> FindFilesAndNameAsDictionary(DirectoryInfo gameImagesLocation)
        {
            return Directory
                .EnumerateFiles(gameImagesLocation.FullName)
                .Where(IsFileTypeSupported)
                .Select(x => new FileInfo(x))
                .ToDictionary(key => Path.GetFileNameWithoutExtension(key.Name), y => y.FullName);
        }

        public FileInfo FindImageFile(Card card, DirectoryInfo imagesLocation)
        {
            FileInfo imageFile = null;
            List<FileInfo> imagesWithCardId = new List<FileInfo>();

            foreach (var supportedImageType in GetSupportedFileTypes())
            {
                var cardName = card.Id.ToString();
                var path = Path.Combine(imagesLocation.FullName, cardName);
                var filePath = Path.ChangeExtension(path, supportedImageType);
                var tempFile = new FileInfo(filePath);

                if (tempFile.Exists)
                {
                    imagesWithCardId.Add(tempFile);
                }
            }

            if (imagesWithCardId.Count > 1)
            {
                imageFile = imagesWithCardId
                    .FirstOrDefault(x => x.Extension == Constants.SupportedImageTypes.jpg.ToString());
                Debug.WriteLine($"imagesWithCardId.Count found for {card.Name} picking: {imageFile?.Name}");
                //TODO what to do when a jpg and a png of the card exists in the folder?
            }
            else if (imagesWithCardId.Count == 0)
            {
                Debug.WriteLine($"imageFile is null for card with name: {card.Name} and id: {card.Id}");
                return new FileInfo(Constants.ErrorImageLocation);
            }
            else
            {
                imageFile = imagesWithCardId.First();
            }

            return imageFile;
        }

        private bool IsFileTypeSupported(string path)
        {
            var ext = Path.GetExtension(path);
            //Removes the dot
            var fileType = ext?.ToLower().Substring(1);
            return GetSupportedFileTypes().Contains(fileType);
        }

        private List<string> GetSupportedFileTypes()
        {
            return Enum.GetNames(typeof(Constants.SupportedImageTypes)).ToList();
        }
    }
}
