using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Repository;
using Blue_Eyes_White_Dragon.UI.Models;

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
        private readonly string _errorImagePath;

        public ArtworkManager(IFileRepository fileRepo, ICardRepository cardRepo)
        {
            _fileRepo = fileRepo;
            _cardRepo = cardRepo;

            _errorImagePath = Path.Combine(Path.GetTempPath(), "error.bmp");
            Properties.Resources.error.Save(_errorImagePath);
        }

        public List<Artwork> CreateArtworkModels(List<Card> gameCards, DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation)
        {
            var artworkList = new List<Artwork>();

            var counter = 0;
            foreach (var gameCard in gameCards)
            {
                Debug.WriteLine($"Loading game image {counter} of {gameCards.Count}");
                artworkList.Add(new Artwork()
                {
                    GameImagePath = _fileRepo.FindImageFile(gameCard, gameImagesLocation).FullName,
                    GameImageMonsterName = gameCard.Name,
                    GameImagesLocations = gameImagesLocation,
                    ReplacementImagesLocations = replacementImagesLocation
                });
                counter++;

                if (counter == 100)
                {
                    break;
                }
            }
            return artworkList;
        }

        public List<Artwork> UpdateArtworkModelsWithReplacement(List<Artwork> artworkList)
        {
            var numberProcessed = 0;
            foreach (var gameCard in artworkList)
            {
                Debug.WriteLine($"Processing card number {numberProcessed} of {artworkList.Count}");
                var replacementCard = FindSuitableReplacementCard(gameCard);

                gameCard.ReplacementImageMonsterName = replacementCard.GameImageMonsterName;
                gameCard.ReplacementImageFileName = replacementCard.ReplacementImageFileName;
                gameCard.ReplacementImagePath = replacementCard.ReplacementImagePath;

                numberProcessed++;
                if (numberProcessed == 100)
                {
                    break;
                }
            }
            return artworkList;
        }

        private Artwork FindSuitableReplacementCard(Artwork gameCard)
        {
            try
            {
                //Card contains name + id
                var matchingCards = _cardRepo.SearchCard(gameCard.GameImageMonsterName);
                var replacementCard = matchingCards.FirstOrDefault();
                if (replacementCard == null)
                {
                    var artwork = new Artwork()
                    {
                        ReplacementImageMonsterName = gameCard.GameImageMonsterName,
                        ReplacementImagePath = _errorImagePath
                    };
                    return artwork;
                }

                gameCard.ReplacementImageMonsterName = replacementCard.Name;
                var imageFile = _fileRepo.FindImageFile(replacementCard, gameCard.ReplacementImagesLocations);

                gameCard.ReplacementImageFileName = imageFile.Name;
                gameCard.ReplacementImagePath = imageFile.FullName;

                if (matchingCards.Count > 1)
                {
                    //TODO Gotta implement a way to show more than one card if multiple are found
                    Debug.WriteLine($"{matchingCards.Count} matching cards found for {gameCard.GameImageMonsterName} picked: {gameCard.ReplacementImageFileName}");
                }
                return gameCard;

            }
            catch (Exception e)
            {
                Debug.WriteLine($"Databas error: {e}, inner: {e.InnerException}");
                gameCard.ReplacementImagePath = Constants.ErrorImageLocation;
                return gameCard;
            }
        }
    }
}
