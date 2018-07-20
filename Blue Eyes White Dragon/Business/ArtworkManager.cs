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
using Blue_Eyes_White_Dragon.UI.Models;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Business
{
    public class ArtworkManager : IArtworkManager
    {
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        /// <summary>
        /// Points to an error image located in the users temp directory.
        /// A rather hacky way to supply a string path to the artwork model
        /// </summary>
        private readonly ILogger _logger;

        public ArtworkManager(IFileRepository fileRepo, ICardRepository cardRepo, ILogger logger)
        {
            _fileRepo = fileRepo ?? throw new ArgumentNullException(nameof(fileRepo));
            _cardRepo = cardRepo ?? throw new ArgumentNullException(nameof(cardRepo));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Artwork> CreateArtworkModels(List<Card> gameCards, DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var artworkList = new ConcurrentBag<Artwork>();

            Parallel.For(0, gameCards.Count, i =>
            {
                var gameCard = gameCards[i];
                var gameImageFile = _fileRepo.FindImageFile(gameCard, gameImagesLocation);

                artworkList.Add(new Artwork()
                {
                    GameImageFile = gameImageFile,
                    GameImageMonsterName = gameCard.Name,
                    GameImagesDir = gameImagesLocation,
                    ReplacementImagesDir = replacementImagesLocation,
                    IsMatched = false
                });
            });
            stopwatch.Stop();
            _logger.LogInformation($"Created {gameCards.Count} ArtworkModels in {MiliToSec(stopwatch.ElapsedMilliseconds)}s");

            return artworkList.ToList();
        }

        private long MiliToSec(long stopwatchElapsedMilliseconds)
        {
            return stopwatchElapsedMilliseconds / 1000;
        }

        public List<Artwork> UpdateArtworkModelsWithReplacement(List<Artwork> artworkList)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var numberOfArtwork = artworkList.Count;
            long numberProcessed = 0;

            foreach (var artwork in artworkList)
            {
                ProcessArtwork(artwork);
                numberProcessed++;
                _logger.LogInformation($"{numberProcessed} of {numberOfArtwork} processed - {artwork.GameImageMonsterName}");
            }
            stopwatch.Stop();
            _logger.LogInformation($"Processed {artworkList.Count} in {MiliToSec(stopwatch.ElapsedMilliseconds)}s");

            return artworkList;
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
                return HandleNoMatch(artwork);
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
            var imageFile = _fileRepo.FindImageFile(replacementCard, artwork.ReplacementImagesDir);
            artwork.ReplacementImageFile = imageFile;
            artwork.IsMatched = true;
        }

        private void HandleMultipleMatches(ICollection<Card> matchingCards, Artwork artwork)
        {
            var firstCard = matchingCards.First();
            matchingCards.Remove(firstCard);
            HandleSingleMatch(firstCard, artwork);

            foreach (var card in matchingCards)
            {
                var imageFile = _fileRepo.FindImageFile(card, artwork.ReplacementImagesDir);
                artwork.AlternateReplacementImages.Add(imageFile);
            }
        }

        private Artwork HandleNoMatch(Artwork artwork)
        {
            artwork.ReplacementImageMonsterName = artwork.GameImageMonsterName;
            artwork.ReplacementImageFile = _fileRepo.ErrorImage;
            artwork.IsMatched = false;
            _logger.LogInformation($"No match was found for {artwork.GameImageMonsterName} - picking the error image");
            return artwork;
        }
    }
}
