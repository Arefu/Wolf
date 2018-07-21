using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.Misc.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Business
{
    public class ArtworkManager : IArtworkManager
    {
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        private readonly ILogger _logger;
        private readonly IResourceRepository _resourceRepo;
        private readonly FileInfo _errorImage;

        public ArtworkManager(IFileRepository fileRepo, ICardRepository cardRepo, ILogger logger, IResourceRepository resourceRepo)
        {
            _fileRepo = fileRepo ?? throw new ArgumentNullException(nameof(fileRepo));
            _cardRepo = cardRepo ?? throw new ArgumentNullException(nameof(cardRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _resourceRepo = resourceRepo;
            _errorImage = _resourceRepo.LoadErrorImageFromResource();
        }

        public List<Artwork> CreateArtworkModels(List<Card> gameCards, DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var artworkList = new ConcurrentBag<Artwork>();

            Parallel.For(0, gameCards.Count, i =>
            {
                var gameCard = gameCards[i];
                var gameImageFile = SearchForImages(gameCard.Id, gameImagesLocation);

                artworkList.Add(new Artwork()
                {
                    CardId = gameCard.Id,
                    GameImageFile = gameImageFile ?? _errorImage,
                    GameImageMonsterName = gameCard.Name,
                    GameImagesDir = gameImagesLocation,
                    ReplacementImagesDir = replacementImagesLocation,
                    IsMatched = false,
                    IsPendulum = gameCard.IsPendulum
                });
            });
            stopwatch.Stop();
            _logger.LogInformation($"Created {gameCards.Count} ArtworkModels in {MiliToSec(stopwatch.ElapsedMilliseconds)}s");

            return artworkList.ToList();
        }

        public List<Artwork> UpdateArtworkModelsWithReplacement(List<Artwork> artworkList, bool useIncludedPendulum)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var numberOfArtwork = artworkList.Count;
            long progress = 0;

            foreach (var artwork in artworkList)
            {
                if (useIncludedPendulum && artwork.IsPendulum)
                {
                    ProcessArtworkAsPendulum(artwork);
                }
                else
                {
                    ProcessArtwork(artwork);
                }
                progress++;
                _logger.LogInformation(Localization.ProcessingProgress(progress, numberOfArtwork, artwork.GameImageMonsterName));
            }
            stopwatch.Stop();
            _logger.LogInformation(Localization.ProcessingDone(artworkList.Count, MiliToSec(stopwatch.ElapsedMilliseconds)));

            return artworkList;
        }

        private void ProcessArtworkAsPendulum(Artwork artwork)
        {
            var pendulumLocation = _resourceRepo.GetPendulumPathFromResource();
            var currentPendulumImage = SearchForImages(artwork.CardId, pendulumLocation);

            if (currentPendulumImage != null)
            {
                artwork.ReplacementImageFile = currentPendulumImage;
                artwork.IsMatched = true;
            }
            else
            {
                artwork.ReplacementImageFile = _errorImage;
                artwork.IsMatched = false;
            }
            artwork.ReplacementImageMonsterName = artwork.GameImageMonsterName;
        }

        private void ProcessArtwork(Artwork artwork)
        {
            var replacementCard = FindSuitableReplacementCard(artwork);
            artwork.ReplacementImageMonsterName = replacementCard.GameImageMonsterName;
            artwork.ReplacementImageFile = replacementCard.ReplacementImageFile;
            artwork.IsMatched = true;
        }

        private Artwork FindSuitableReplacementCard(Artwork artwork)
        {
            var matchingCards = SearchCards(artwork);
            var replacementCard = matchingCards.FirstOrDefault();

            if (replacementCard == null)
            {
                HandleNoMatch(artwork);
            }

            if (matchingCards.Count == 1)
            {
                HandleSingleMatch(replacementCard, artwork);
            }

            if (matchingCards.Count > 1)
            {
                HandleMultipleMatches(matchingCards, artwork);
            }

            return artwork;
        }

        private List<Card> SearchCards(Artwork artwork)
        {
            try
            {
                return _cardRepo.SearchCards(artwork.GameImageMonsterName);
            }
            catch (Exception e)
            {
                _logger.LogInformation($"Databas error: {e}, inner: {e.InnerException} for {artwork.GameImageMonsterName}");
                return new List<Card>();
            }
        }

        private void HandleSingleMatch(Card replacementCard, Artwork artwork)
        {
            var imageFile = SearchForImages(replacementCard.Id, artwork.ReplacementImagesDir);
            if (imageFile != null)
            {
                artwork.ReplacementImageFile = imageFile;
                artwork.IsMatched = true;
            }
            else
            {
                artwork.ReplacementImageFile = _errorImage;
                artwork.IsMatched = false;
            }
        }

        private void HandleMultipleMatches(ICollection<Card> matchingCards, Artwork artwork)
        {
            var firstCard = matchingCards.First();
            matchingCards.Remove(firstCard);
            HandleSingleMatch(firstCard, artwork);

            foreach (var card in matchingCards)
            {
                var imageFile = SearchForImages(card.Id, artwork.ReplacementImagesDir) ?? _errorImage;
                artwork.AlternateReplacementImages.Add(imageFile);
            }
        }

        private void HandleNoMatch(Artwork artwork)
        {
            artwork.ReplacementImageMonsterName = artwork.GameImageMonsterName;
            artwork.ReplacementImageFile = _errorImage;
            artwork.IsMatched = false;
            _logger.LogInformation(Localization.ErrorNoMatch(artwork.GameImageMonsterName));
        }

        private long MiliToSec(long stopwatchElapsedMilliseconds)
        {
            return stopwatchElapsedMilliseconds / 1000;
        }

        private FileInfo SearchForImages(int cardId, DirectoryInfo directory)
        {
            FileInfo imageFile = null;

            var images = SearchForImagesInDirectory(cardId, directory);

            if (images.Count > 1)
            {
                imageFile = images.FirstOrDefault(x => x.Extension == Constants.SupportedImageTypes.jpg.ToString());
                _logger.LogInformation($"{images.Count} images found for {cardId} picking: {imageFile?.Name}");
                //TODO what to do when a jpg and a png of the card exists in the folder?
            }
            else if (images.Count == 0)
            {
                _logger.LogInformation($"no image was found for the card: {cardId}");
                return null;
            }
            else
            {
                imageFile = images.First();
            }
            return imageFile;
        }

        private List<FileInfo> SearchForImagesInDirectory(int cardId, DirectoryInfo directory)
        {
            List<FileInfo> foundImages = new List<FileInfo>();

            var filetypes = _fileRepo.GetSupportedFileTypes();

            foreach (var supportedImageType in filetypes)
            {
                var filename = Path.ChangeExtension(cardId.ToString(), supportedImageType);
                var image = _fileRepo.GetImageFile(filename, directory);
                if (image != null)
                {
                    foundImages.Add(image);
                }
            }

            return foundImages;
        }
    }
}
