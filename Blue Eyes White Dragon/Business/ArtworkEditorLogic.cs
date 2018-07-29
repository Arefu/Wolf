using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.Misc.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Business
{
    public class ArtworkEditorLogic : IArtworkEditorLogic
    {
        private readonly IArtworkManager _artworkManager;
        private readonly IFileRepository _fileRepo;
        private readonly IGameFileRepository _gameFileRepo;
        private readonly ILogger _logger;
        private readonly ISettingRepository _settingRepo;
        private readonly ISaveAndLoadRepository _saveAndLoadRepo;
        private readonly IDbFactory _dbFactory;

        public ArtworkEditorLogic(IArtworkManager artworkManager, IFileRepository fileRepo,
            IGameFileRepository gameFileRepo, ILogger logger, ISettingRepository settingRepo, ISaveAndLoadRepository saveAndLoadRepo,
            IDbFactory dbFactory)
        {
            _artworkManager = artworkManager ?? throw new ArgumentNullException(nameof(artworkManager));
            _fileRepo = fileRepo ?? throw new ArgumentNullException(nameof(fileRepo));
            _gameFileRepo = gameFileRepo ?? throw new ArgumentNullException(nameof(gameFileRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _settingRepo = settingRepo ?? throw new ArgumentNullException(nameof(settingRepo));
            _saveAndLoadRepo = saveAndLoadRepo ?? throw new ArgumentNullException(nameof(saveAndLoadRepo));
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
        }

        public IEnumerable<Artwork> RunMatchAll(DirectoryInfo replacementImagesLocation, bool useIncludedPendulum)
        {
            var gameCards = _gameFileRepo.GetAllCards();
            var artworkListWithGameCards = _artworkManager.CreateArtworkModels(gameCards, replacementImagesLocation);
            var artworkListWithReplacements = _artworkManager.UpdateArtworkModelsWithReplacement(artworkListWithGameCards, useIncludedPendulum);
            CalculateHeightAndWidth(artworkListWithReplacements);
            return SortArtwork(artworkListWithReplacements);
        }

        public void RunSaveMatch(IEnumerable<Artwork> artworks)
        {
            var path = _saveAndLoadRepo.SaveArtworkMatchToFile(artworks);
            _logger.LogInformation(Localization.InformationSaveComplete(path));
        }

        public void RunConvertAll(IEnumerable<Artwork> artworks)
        {
            _artworkManager.ConvertAll(artworks);
        }

        public void SetDbLocation(string path)
        {
            _dbFactory.CardDbLocation = path;
        }

        public string LoadDbPath()
        {
            return _settingRepo.GetPathSetting(Constants.Setting.LastUsedCardDbPath);
        }

        public void SavePathSetting(string filePath, Constants.Setting setting)
        {
            _settingRepo.SavePathSetting(filePath, setting);
        }

        public void RunCustomArtPicked(Artwork artwork, ArtworkSearch pickedArtwork)
        {
            var pickedImageFile = pickedArtwork.ImageFile;
            SwapAlternateImages(artwork, artwork.ReplacementImageFile, pickedImageFile);

            artwork.ReplacementImageFile = pickedImageFile;

            _logger.LogInformation(Localization.InformationArtworkUpdated(artwork.GameImageMonsterName));
        }

        public IEnumerable<Artwork> LoadArtworkMatch(string path)
        {
            return _saveAndLoadRepo.LoadArtworkMatchFromFile(path);
        }

        public void CalculateHeightAndWidth(List<Artwork> artworkList)
        {
            _logger.LogInformation(Localization.InformationCalculatingImageDimensions);
            _fileRepo.CalculateHeightAndWidth(artworkList);
            _logger.LogInformation(Localization.InformationCalculationComplete);
        }

        private void SwapAlternateImages(Artwork artwork, FileInfo orgImageFile, FileInfo newImageFile)
        {
            var altImages = artwork.AlternateReplacementImages;
            altImages.Remove(newImageFile);
            altImages.Add(orgImageFile);
            artwork.AlternateReplacementImages = altImages;
        }

        public IEnumerable<Artwork> SortArtwork(IEnumerable<Artwork> artworkList)
        {
            return artworkList.OrderBy(x => x.GameImageMonsterName).ToList();
        }

        public string GetPathSetting(Constants.Setting setting)
        {
            return _settingRepo.GetPathSetting(setting);
        }
    }
}
