using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business.Factory.Interface;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Blue_Eyes_White_Dragon.Utility;
using Blue_Eyes_White_Dragon.Utility.Interface;

namespace Blue_Eyes_White_Dragon.Business
{
    public class ArtworkManager
    {
        private readonly IFileRepository _fileRepo;
        private readonly ICardRepository _cardRepo;
        /// <summary>
        /// Points to an error image located in the users temp directory.
        /// A rather hacky way to supply a string path to the artwork model
        /// </summary>
        private readonly ILogger _logger;
        private readonly ICardDbContextFactory _cardDbFactory;

        public ArtworkManager(IFileRepository fileRepo, ICardRepository cardRepo,
            ILogger logger, ICardDbContextFactory cardDbFactory)
        {
            _fileRepo = fileRepo;
            _cardRepo = cardRepo;
            _logger = logger;
            _cardDbFactory = cardDbFactory;
        }

        public List<Artwork> CreateArtworkModels(List<Card> gameCards, DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var artworkList = new ConcurrentBag<Artwork>();

            Parallel.For(0, gameCards.Count, i =>
            {
                var gameCard = gameCards[i];
                artworkList.Add(new Artwork()
                {
                    GameImageFile = _fileRepo.FindImageFile(gameCard, gameImagesLocation),
                    GameImageMonsterName = gameCard.Name,
                    GameImagesDir = gameImagesLocation,
                    ReplacementImagesDir = replacementImagesLocation
                });
            });
            stopwatch.Stop();
            _logger.LogInformation($"Created {gameCards.Count} ArtworkModels in {stopwatch.ElapsedMilliseconds}ms");

            return artworkList.ToList();
        }

        public List<Artwork> UpdateArtworkModelsWithReplacementAsync(List<Artwork> artworkList)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var numberOfArtwork = artworkList.Count;
            long numberProcessed = 0;
            var artworkBag = new ConcurrentBag<Artwork>(artworkList);
            Parallel.For(0, 1000, i =>
            {
                var gameArtwork = artworkList[i];
                using (var db = _cardDbFactory.CreateCardDbContext())
                {
                    ProcessArtworkAsync(gameArtwork, db);
                }
                Interlocked.Increment(ref numberProcessed);
                _logger.LogInformation($"{Interlocked.Read(ref numberProcessed)} of {numberOfArtwork} processed");
            });

            stopwatch.Stop();
            _logger.LogInformation($"Processed {artworkList.Count} in {stopwatch.ElapsedMilliseconds}ms");

            return artworkList;
        }

        public List<Artwork> UpdateArtworkModelsWithReplacement(List<Artwork> artworkList)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var numberOfArtwork = artworkList.Count;
            long numberProcessed = 0;

            foreach (var artwork in artworkList)
            {
                var gameArtwork = artwork;
                ProcessArtwork(gameArtwork);
                numberProcessed++;
                _logger.LogInformation($"{numberProcessed} of {numberOfArtwork} processed");
            }
            stopwatch.Stop();
            _logger.LogInformation($"Processed {artworkList.Count} in {stopwatch.ElapsedMilliseconds}ms");

            return artworkList;
        }


        private void ProcessArtwork(Artwork gameArtwork)
        {
            var replacementCard = FindSuitableReplacementCard(gameArtwork);
            gameArtwork.ReplacementImageMonsterName = replacementCard.GameImageMonsterName;
            gameArtwork.ReplacementImageFile = replacementCard.ReplacementImageFile;
        }
        private void ProcessArtworkAsync(Artwork gameArtwork, ICardDbContext db)
        {
            //var replacementCard = FindSuitableReplacementCardAsync(gameArtwork, db);
            //gameArtwork.ReplacementImageMonsterName = replacementCard.GameImageMonsterName;
            //gameArtwork.ReplacementImageFile = replacementCard.ReplacementImageFile;
        }

        private Artwork FindSuitableReplacementCard(Artwork gameCard)
        {
            try
            {
                var matchingCards = _cardRepo.SearchCards(gameCard.GameImageMonsterName);
                var replacementCard = matchingCards.FirstOrDefault();
                if (replacementCard == null)
                {
                    gameCard.ReplacementImageMonsterName = gameCard.GameImageMonsterName;
                    gameCard.ReplacementImageFile = _fileRepo.ErrorImage;
                    _logger.LogInformation($"No match was found for {gameCard.GameImageMonsterName} - picking the error image");
                    return gameCard;
                }

                gameCard.ReplacementImageMonsterName = replacementCard.Name;
                var imageFile = _fileRepo.FindImageFile(replacementCard, gameCard.ReplacementImagesDir);
                gameCard.ReplacementImageFile = imageFile;

                if (matchingCards.Count > 1)
                {
                    //TODO Gotta implement a way to show more than one card if multiple are found
                    _logger.LogInformation($"{matchingCards.Count} matching cards found for {gameCard.GameImageMonsterName} picked: {gameCard.ReplacementImageFileName}");
                }
                return gameCard;

            }
            catch (Exception e)
            {
                _logger.LogInformation($"Databas error: {e}, inner: {e.InnerException}");
                gameCard.ReplacementImageMonsterName = gameCard.GameImageMonsterName;
                return gameCard;
            }
        }
    }
}
