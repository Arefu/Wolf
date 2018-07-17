using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.Business;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Blue_Eyes_White_Dragon.Utility.Interface;
using Newtonsoft.Json;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class FileRepository : IFileRepository
    {
        private readonly ILogger _logger;
        public FileInfo ErrorImage { get; set; }
        private readonly string _directoryName;

        public FileRepository(ILogger logger, string directoryName)
        {
            _logger = logger;
            _directoryName = directoryName;
            //I'd rather do this on boot than the first time the image is needed, eg. with a lambda
            ErrorImage = LoadErrorImage();
        }

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
                _logger.LogInformation($"{imagesWithCardId.Count} images found for {card.Name} picking: {imageFile?.Name}");
                //TODO what to do when a jpg and a png of the card exists in the folder?
            }
            else if (imagesWithCardId.Count == 0)
            {
                _logger.LogInformation($"no image was found for the card: {card.Name} and id: {card.Id}");
                return ErrorImage;
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

        public FileInfo LoadErrorImage()
        {
            var errorImagePath = Path.Combine(Path.GetTempPath(), "error.bmp");
            var errorImage = new FileInfo(errorImagePath);
            if (!errorImage.Exists)
            {
                Properties.Resources.error.Save(errorImagePath);
            }
            return errorImage;
        }

        public bool GetJpegDimension(string fileName, out int width, out int height)
        {
            width = height = 0;
            bool found = false;
            bool eof = false;

            FileStream stream = new FileStream(fileName,FileMode.Open,FileAccess.Read);
            BinaryReader reader = new BinaryReader(stream);

            while (!found || eof)
            {
                // read 0xFF and the type
                reader.ReadByte();
                byte type = reader.ReadByte();

                // get length
                int len = 0;
                switch (type)
                {
                    // start and end of the image
                    case 0xD8:
                    case 0xD9:
                        len = 0;
                        break;

                    // restart interval
                    case 0xDD:
                        len = 2;
                        break;

                    // the next two bytes is the length
                    default:
                        int lenHi = reader.ReadByte();
                        int lenLo = reader.ReadByte();
                        len = (lenHi << 8 | lenLo) - 2;
                        break;
                }

                // EOF?
                if (type == 0xD9)
                    eof = true;

                // process the data
                if (len > 0)
                {
                    // read the data
                    byte[] data = reader.ReadBytes(len);

                    // this is what we are looking for
                    if (type == 0xC0)
                    {
                        height = data[1] << 8 | data[2];
                        width = data[3] << 8 | data[4];
                        found = true;
                    }
                }
            }
            reader.Close();
            stream.Close();
            return found;
        }

        public bool GetPngDimension(string fileName, out int width, out int height)
        {
            var buff = new byte[32];
            using (var d = File.OpenRead(fileName))
            {
                d.Read(buff, 0, 32);
            }
            const int wOff = 16;
            const int hOff = 20;
            width = BitConverter.ToInt32(new[] { buff[wOff + 3], buff[wOff + 2], buff[wOff + 1], buff[wOff + 0], }, 0);
            height = BitConverter.ToInt32(new[] { buff[hOff + 3], buff[hOff + 2], buff[hOff + 1], buff[hOff + 0], }, 0);
            return true;
        }

        public string SaveArtworkMatchToFile(List<Artwork> artworkList)
        {
            var fileName = Constants.ArtworkMatchFileName;
            var path = Path.Combine(_directoryName, fileName);

            string jsonArtwork = JsonConvert.SerializeObject(artworkList, Formatting.Indented);
            File.WriteAllText(path, jsonArtwork);
            return path;
        }

        public List<Artwork> LoadArtworkMatchFromFile(string path)
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                List<Artwork> files = (List<Artwork>) JsonConvert.DeserializeObject<List<Artwork>>(json);
                return files;
            }
        }
    }
}
