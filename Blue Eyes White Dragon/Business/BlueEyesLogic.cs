using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Blue_Eyes_White_Dragon.Business.Factory;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Repository;
using Blue_Eyes_White_Dragon.Presenter.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Blue_Eyes_White_Dragon.Utility;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Business
{
    public class BlueEyesLogic : IBlueEyesLogic
    {
        /// <summary>
        /// This is a reference to the actual UI. This way we can call UI methods from the logic layer
        /// when we have something new to present to the user
        /// </summary>
        private readonly IArtworkEditorPresenter _artworkEditorPresenter;
        private readonly IArtworkManager _artworkManager;
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        private readonly IGameFileRepository _gameFileRepo;
        private readonly ILogger _logger;
        private IEnumerable<Artwork> _artworkList;

        public BlueEyesLogic(IArtworkEditorPresenter artworkEditorPresenter)
        {
            _artworkEditorPresenter = artworkEditorPresenter;
            _logger = new ConsoleLogger(_artworkEditorPresenter);
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            _fileRepo = new FileRepository(_logger, currentDir);
            _cardRepo = new CardRepository(new CardDbContext(), _logger);

            var manager = new Manager();
            manager.Load();
            _gameFileRepo = new GameFileRepository(manager);
            _artworkManager = new ArtworkManager(_fileRepo, _cardRepo, _logger, new CardDbContextFactory());
        }

        public void RunMatchAll()
        {
            _artworkEditorPresenter.ClearObjectsFromObjectListView();

            var gameImagesLocation = _fileRepo.LoadCardDir(Constants.GameImagesLocation);
            var replacementImagesLocation = _fileRepo.LoadCardDir(Constants.ReplacementImagesLocation);

            var gameCards = _gameFileRepo.GetAllCards();
            var artworkListWithGameCards = _artworkManager.CreateArtworkModels(gameCards, gameImagesLocation, replacementImagesLocation);
            var artworkListWithReplacements = _artworkManager.UpdateArtworkModelsWithReplacement(artworkListWithGameCards);
            _fileRepo.CalculateHeightAndWidth(artworkListWithReplacements);
            _artworkList = SortArtwork(artworkListWithReplacements);

            _artworkEditorPresenter.AddObjectsToObjectListView(_artworkList);
        }

        public void RunSaveMatch()
        {
            var path = _fileRepo.SaveArtworkMatchToFile(_artworkList);
            _logger.LogInformation(Localization.InformationSaveComplete(path));
        }

        public void RunLoadMatch(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                _artworkEditorPresenter.ShowMessageBox(Localization.ErrorEnterPath);
                return;
            }

            var jsonFile = new FileInfo(path);
            if (!jsonFile.Exists)
            {
                _artworkEditorPresenter.ShowMessageBox(Localization.ErrorFileNotExist);
                return;
            }

            var artworkList = _fileRepo.LoadArtworkMatchFromFile(path);

            if (artworkList == null || artworkList.Count == 0)
            {
                _artworkEditorPresenter.ShowMessageBox(Localization.ErrorFileEmptyOrInvalid);
                return;
            }

            _artworkEditorPresenter.ClearObjectsFromObjectListView();

            _fileRepo.CalculateHeightAndWidth(artworkList);
            _artworkList = SortArtwork(artworkList);
            _artworkEditorPresenter.AddObjectsToObjectListView(_artworkList);
            _logger.LogInformation(Localization.InformationLoadComplete(path));
        }

        public void SavePathSetting(string filePath)
        {
            Properties.Settings.Default.LastUsedLoadPath = filePath;
            Properties.Settings.Default.Save();
        }

        public void RunCustomArtPicked(Artwork artwork, int rowIndex, ArtworkSearch pickedArtwork)
        {
            var pickedImageFile = pickedArtwork.ImageFile;
            SwapAlternateImages(artwork, artwork.ReplacementImageFile, pickedImageFile);

            artwork.ReplacementImageFile = pickedImageFile;

            _logger.LogInformation(Localization.InformationArtworkUpdated(artwork.GameImageMonsterName));
        }

        private void SwapAlternateImages(Artwork artwork, FileInfo orgImageFile, FileInfo newImageFile)
        {
            var altImages = artwork.AlternateReplacementImages;
            altImages.Remove(newImageFile);
            altImages.Add(orgImageFile);
            artwork.AlternateReplacementImages = altImages;
        }

        private IEnumerable<Artwork> SortArtwork(IEnumerable<Artwork> artworkList)
        {
            return artworkList.OrderBy(x => x.GameImageMonsterName).ToList();
        }

    }
}
