using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using Blue_Eyes_White_Dragon.Business.Factory;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Repository;
using Blue_Eyes_White_Dragon.UI.Interface;
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
        private readonly IArtworkEditor _artworkEditorUi;
        private readonly IArtworkManager _artworkManager;
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        private readonly IGameFileRepository _gameFileRepo;
        private readonly ILogger _logger;
        private List<Artwork> _artworkList;

        public BlueEyesLogic(IArtworkEditor artworkEditorUi)
        {
            _artworkEditorUi = artworkEditorUi;
            _logger = new ConsoleLogger(_artworkEditorUi);
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
            var gameImagesLocation = _fileRepo.LoadCardDir(Constants.GameImagesLocation);
            var replacementImagesLocation = _fileRepo.LoadCardDir(Constants.ReplacementImagesLocation);

            var gameCards = _gameFileRepo.GetAllCards();
            var artworkListWithGameCards = _artworkManager.CreateArtworkModels(gameCards, gameImagesLocation, replacementImagesLocation);
            var artworkListWithReplacements = _artworkManager.UpdateArtworkModelsWithReplacement(artworkListWithGameCards);
            _artworkList = SortArtwork(artworkListWithReplacements);

            _artworkEditorUi.AddObjectsToObjectListView(_artworkList);
        }

        private List<Artwork> SortArtwork(List<Artwork> artworkList)
        {
            return artworkList.OrderBy(x => x.GameImageMonsterName).ToList();
        }

        public object GameImageGetter(object row)
        {
            var artworkRow = ((Artwork)row);
            var gameImagePath = artworkRow.GameImageFilePath;

            if (string.IsNullOrEmpty(gameImagePath))
            {
                throw new ArgumentNullException($"Can not load GameImageGetter for: {artworkRow}");
            }

            if (_artworkEditorUi.LargeImageListContains(gameImagePath)) return gameImagePath;

            Image image = Image.FromFile(gameImagePath);
            UpdateImageLists(gameImagePath, image);
            artworkRow.GameImageWidth = image.Width;
            artworkRow.GameImageHeight = image.Height;
            return gameImagePath;
        }

        public object ReplacementImageGetter(object row)
        {
            var artworkRow = ((Artwork)row);
            var replacementImagePath = artworkRow.ReplacementImageFilePath;

            if (string.IsNullOrEmpty(replacementImagePath))
            {
                throw new ArgumentNullException($"Can not load ReplacementImageGetter for: {artworkRow}");
            }

            if (_artworkEditorUi.LargeImageListContains(replacementImagePath)) return replacementImagePath;

            Image image = Image.FromFile(replacementImagePath);
            UpdateImageLists(replacementImagePath, image);
            artworkRow.ReplacementImageWidth = image.Width;
            artworkRow.ReplacementImageHeight = image.Height;
            return replacementImagePath;
        }

        private void UpdateImageLists(string imagePath, Image image)
        {
            _artworkEditorUi.SmallImageListAdd(imagePath, image);
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
                _artworkEditorUi.ShowMessageBox(Localization.ErrorEnterPath);
                return;
            }

            var jsonFile = new FileInfo(path);
            if (!jsonFile.Exists)
            {
                _artworkEditorUi.ShowMessageBox(Localization.ErrorFileNotExist);
                return;
            }

            var artworkList = _fileRepo.LoadArtworkMatchFromFile(path);

            if (artworkList == null || artworkList.Count == 0)
            {
                _artworkEditorUi.ShowMessageBox(Localization.ErrorFileEmptyOrInvalid);
                return;
            }

            CalculateHeightAndWidth(artworkList);
            _artworkList = SortArtwork(artworkList);
            _artworkEditorUi.AddObjectsToObjectListView(_artworkList);
            _logger.LogInformation(Localization.InformationLoadComplete(path));
        }

        private void CalculateHeightAndWidth(IEnumerable<Artwork> artworkList)
        {
            foreach (var artwork in artworkList)
            {
                var width = 0;
                var height = 0;

                var path = artwork.GameImageFilePath;
                if (!string.IsNullOrEmpty(path))
                {
                    UpdateArtworkHeightWidth(artwork, ref width, ref height, path);
                }

                path = artwork.ReplacementImageFilePath;
                if (!string.IsNullOrEmpty(path))
                {
                    UpdateArtworkHeightWidth(artwork, ref width, ref height, path);
                }
            }
        }

        private void UpdateArtworkHeightWidth(Artwork artwork, ref int width, ref int height, string path)
        {
            var jpg = Constants.Jpg;
            var png = Constants.Png;

            if (artwork.GameImageFile.Extension == jpg)
            {
                _fileRepo.GetJpegDimension(path, out width, out height);
            }
            else if (artwork.GameImageFile.Extension == png)
            {
                _fileRepo.GetPngDimension(path, out width, out height);
            }

            artwork.GameImageWidth = width;
            artwork.GameImageHeight = height;
        }
    }
}
