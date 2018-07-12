using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Repository;
using Blue_Eyes_White_Dragon.UI;
using Yu_Gi_Oh.File_Handling.Miscellaneous_Files;

namespace Blue_Eyes_White_Dragon.Business
{
    public class BlueEyesLogic
    {
        /// <summary>
        /// This is a reference to the actual UI. This way we can call UI methods from the logic layer
        /// when we have something new to present to the user
        /// </summary>
        private readonly CardArtEditor _cardArtEditor;
        private readonly ArtworkManager _artworkManager;
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        private readonly IGameFileRepository _gameFileRepo;

        public BlueEyesLogic(CardArtEditor cardArtEditor)
        {
            _cardArtEditor = cardArtEditor;
            _fileRepo = new FileRepository();
            _cardRepo = new CardRepository(new CardDbContext());
            var manager = new Manager();
            manager.Load();
            _gameFileRepo = new GameFileRepository(manager);
            _artworkManager = new ArtworkManager(_fileRepo, _cardRepo);
        }

        public void Run()
        {
            var gameImagesLocation = _fileRepo.LoadCardDir(Constants.GameImagesLocation);
            var replacementImagesLocation = _fileRepo.LoadCardDir(Constants.ReplacementImagesLocation);

            var gameCards = _gameFileRepo.GetAllCards();
            var artworkListWithGameCards = _artworkManager.CreateArtworkModels(gameCards, gameImagesLocation, replacementImagesLocation);
            var artworkListWithReplacementCards = _artworkManager.UpdateArtworkModelsWithReplacement(artworkListWithGameCards);

            _cardArtEditor.AddObjectsToObjectListView(artworkListWithReplacementCards);
        }
    }
}
