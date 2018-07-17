using System;
using System.Drawing;
using System.IO;
using Blue_Eyes_White_Dragon.Business.Factory;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Repository;
using Blue_Eyes_White_Dragon.UI;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;
using Blue_Eyes_White_Dragon.Utility;
using Blue_Eyes_White_Dragon.Utility.Interface;
using BrightIdeasSoftware;

namespace Blue_Eyes_White_Dragon.Business
{
    public class BlueEyesLogic
    {
        /// <summary>
        /// This is a reference to the actual UI. This way we can call UI methods from the logic layer
        /// when we have something new to present to the user
        /// </summary>
        private readonly ArtworkEditor _cardArtEditorUi;
        private readonly ArtworkManager _artworkManager;
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        private readonly IGameFileRepository _gameFileRepo;
        private readonly ILogger _logger;

        public BlueEyesLogic(ArtworkEditor cardArtEditorUi)
        {
            _logger = new Logger();
            _cardArtEditorUi = cardArtEditorUi;
            _fileRepo = new FileRepository(_logger);
            _cardRepo = new CardRepository(new CardDbContext(), _logger);

            var manager = new Manager();
            manager.Load();
            _gameFileRepo = new GameFileRepository(manager);
            _artworkManager = new ArtworkManager(_fileRepo, _cardRepo, _logger, new CardDbContextFactory());
        }

        public void Run()
        {
            var gameImagesLocation = _fileRepo.LoadCardDir(Constants.GameImagesLocation);
            var replacementImagesLocation = _fileRepo.LoadCardDir(Constants.ReplacementImagesLocation);

            var gameCards = _gameFileRepo.GetAllCards();
            var artworkListWithGameCards = _artworkManager.CreateArtworkModels(gameCards, gameImagesLocation, replacementImagesLocation);
            var artworkListWithReplacementCards = _artworkManager.UpdateArtworkModelsWithReplacement(artworkListWithGameCards);

            _cardArtEditorUi.AddObjectsToObjectListView(artworkListWithReplacementCards);
        }

        public object GameImageGetter(object row)
        {
            var artworkRow = ((Artwork)row);
            var gameImagePath = artworkRow.GameImageFilePath;

            if (string.IsNullOrEmpty(gameImagePath))
            {
                throw new ArgumentNullException($"Can not load GameImageGetter for: {artworkRow}");
            }
            
            if (!_cardArtEditorUi.LargeImageListContains(gameImagePath))
            {
                UpdateImageLists(gameImagePath);
            }
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

            if (!_cardArtEditorUi.LargeImageListContains(replacementImagePath))
            {
                UpdateImageLists(replacementImagePath);
            }
            return replacementImagePath;
        }

        private void UpdateImageLists(string imagePath)
        {
            //Both lists must be in sync to use anything other than the DETAILS view. Yes I know
            //that is what we are using, but if we use another view in the future we most likely won't remember this.
            Image smallImage = Image.FromFile(imagePath);
            Image largeImage = Image.FromFile(imagePath);

            _cardArtEditorUi.SmallImageListAdd(imagePath, smallImage);
            _cardArtEditorUi.LargeImagelistAdd(imagePath, largeImage);

            var smallImageListCount = _cardArtEditorUi.SmallImageListGetCount();
            var largeImageListCount = _cardArtEditorUi.LargeImageListGetCount();

            _logger.LogInformation($"Image: {imagePath} is about to be shown");
            _logger.LogInformation($"SmallImageList count: {smallImageListCount}");
            _logger.LogInformation($"LargeImageList count: {largeImageListCount}");
        }

        public void LogFilterResults(ObjectListView listView)
        {
            _logger.LogInformation("Listing filtered results:");
            for (int i = 0; i < listView.Items.Count; i++)
            {
                _logger.LogInformation(listView.Items[i].ToString());
            }
        }
    }
}
