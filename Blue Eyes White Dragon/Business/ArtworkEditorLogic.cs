using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Blue_Eyes_White_Dragon.Utility;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Business
{
    public class ArtworkEditorLogic : IArtworkEditorLogic
    {
        private readonly IArtworkManager _artworkManager;
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        private readonly IGameFileRepository _gameFileRepo;
        private readonly ILogger _logger;

        public ArtworkEditorLogic(IArtworkManager artworkManager, IFileRepository fileRepo, ICardRepository cardRepo, IGameFileRepository gameFileRepo, ILogger logger)
        {
            _artworkManager = artworkManager ?? throw new ArgumentNullException(nameof(artworkManager));
            _fileRepo = fileRepo ?? throw new ArgumentNullException(nameof(fileRepo));
            _cardRepo = cardRepo ?? throw new ArgumentNullException(nameof(cardRepo));
            _gameFileRepo = gameFileRepo ?? throw new ArgumentNullException(nameof(gameFileRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IEnumerable<Artwork> RunMatchAll(bool useIncludedPendulum)
        {
            var gameImagesLocation = _fileRepo.LoadCardDir(Constants.GameImagesLocation);
            var replacementImagesLocation = _fileRepo.LoadCardDir(Constants.ReplacementImagesLocation);

            var gameCards = _gameFileRepo.GetAllCards();
            var artworkListWithGameCards = _artworkManager.CreateArtworkModels(gameCards, gameImagesLocation, replacementImagesLocation);
            var artworkListWithReplacements = _artworkManager.UpdateArtworkModelsWithReplacement(artworkListWithGameCards, useIncludedPendulum);
            CalculateHeightAndWidth(artworkListWithReplacements);
            return SortArtwork(artworkListWithReplacements);
        }

        public void RunSaveMatch(IEnumerable<Artwork> artworks)
        {
            var path = _fileRepo.SaveArtworkMatchToFile(artworks);
            _logger.LogInformation(Localization.InformationSaveComplete(path));
        }

        public void SavePathSetting(string filePath)
        {
            Properties.Settings.Default.LastUsedLoadPath = filePath;
            Properties.Settings.Default.Save();
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
            return _fileRepo.LoadArtworkMatchFromFile(path);
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

    }
}
