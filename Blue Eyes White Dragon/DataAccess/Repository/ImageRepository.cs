using System;
using System.IO;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.Misc.Interface;
using PhotoSauce.MagicScaler;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class ImageRepository : IImageRepository
    {
        private readonly ILogger _logger;

        public ImageRepository(ILogger logger)
        {
            _logger = logger;
        }

        public void ConvertImage(DirectoryInfo destinationPath, FileInfo imageFile, string orgName,
            ProcessImageSettings settings, string zibFilename)
        {
            var outputFolder = new DirectoryInfo(Path.Combine(Constants.ResourceOutputFolderName, zibFilename));
            var filePath = Path.Combine(outputFolder.FullName, orgName);

            var imageLocation = imageFile.FullName;
            try
            {
                CreatePathIfNotExists(outputFolder);

                using (var outStream = new FileStream(filePath, FileMode.Create))
                {
                    MagicImageProcessor.ProcessImage(imageLocation, outStream, settings);
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
            }
        }

        private void CreatePathIfNotExists(DirectoryInfo path)
        {
            if (!path.Exists)
            {
                path.Create();
            }
        }
    }
}
