using System;
using System.IO;
using System.Reflection;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.Misc.Interface;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly ILogger _logger;
        private readonly string _resourceLocation;

        public ResourceRepository(ILogger logger)
        {
            _logger = logger;
            var executionPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? throw new InvalidOperationException();
            _resourceLocation = Path.Combine(executionPath, Constants.ResourceLocation);
            if (_resourceLocation == null)
            {
                _logger.LogError(Localization.ErrorExecutableLocationNotFound);
            }
        }

        public DirectoryInfo GetPendulumPathFromResource()
        {
            return new DirectoryInfo(Path.Combine(_resourceLocation, Constants.ResourcePendulumLocation));
        }

        public FileInfo LoadErrorImageFromResource()
        {
            var errorImageLocation = Path.Combine(_resourceLocation, Constants.ErrorImageName);
            var errorImage = new FileInfo(errorImageLocation);
            if (errorImage.Exists)
            {
                return errorImage;
            }
            _logger.LogError(Localization.ErrorLoadingErrorImage);
            return null;
        }
    }
}
