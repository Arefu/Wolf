using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
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
        private readonly FileInfo _errorImageFile;

        public ArtworkManager(IFileRepository fileRepo, ICardRepository cardRepo, FileInfo errorImage)
        {
            _fileRepo = fileRepo;
            _cardRepo = cardRepo;
            _errorImageFile = errorImage;
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
                    GameImageFile = _fileRepo.FindImageFile(gameCard, gameImagesLocation),
                    GameImageMonsterName = gameCard.Name,
                    GameImagesDir = gameImagesLocation,
                    ReplacementImagesDir = replacementImagesLocation
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
                gameCard.ReplacementImageFile = replacementCard.ReplacementImageFile;

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
                var matchingCards = _cardRepo.SearchCard(gameCard.GameImageMonsterName);
                var replacementCard = matchingCards.FirstOrDefault();
                if (replacementCard == null)
                {
                    gameCard.ReplacementImageMonsterName = gameCard.GameImageMonsterName;
                    gameCard.ReplacementImageFile = _errorImageFile;
                    return gameCard;
                }

                gameCard.ReplacementImageMonsterName = replacementCard.Name;
                var imageFile = _fileRepo.FindImageFile(replacementCard, gameCard.ReplacementImagesDir);
                gameCard.ReplacementImageFile = imageFile;

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
                gameCard.ReplacementImageMonsterName = gameCard.GameImageMonsterName;
                return gameCard;
            }
        }
    }
}
