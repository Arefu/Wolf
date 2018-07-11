using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.BusinessModels;
using Blue_Eyes_White_Dragon.DataAccess;
using Blue_Eyes_White_Dragon.UI_Models;

namespace Blue_Eyes_White_Dragon.BusinessLogic
{
    public class ArtworkManager
    {
        private readonly CardDbContext _db;

        public ArtworkManager(CardDbContext db)
        {
            _db = db;
        }

        public List<ArtworkModel> CreateArtworkList(DirectoryInfo gameImagesLocation, DirectoryInfo replacementImagesLocation)
        {
            List<FileInfo> gameImageFiles = FileLoader.FindFiles(gameImagesLocation);
            //Dictionary<string, string> replacementImageFiles = FileLoader.FindFilesAndNameAsDictionary(replacementImagesLocation);

            Debug.WriteLine($"Number of images found in {gameImagesLocation.FullName}: {gameImageFiles.Count}");

            var artworkList = new List<ArtworkModel>();

            foreach (var gameFile in gameImageFiles)
            {
                //var replacementCard = FindSuitableReplacementCard(gameFile, replacementImageFiles);

                artworkList.Add(new ArtworkModel()
                {
                    GameImagePath = gameFile.FullName,
                    GameImageFileName = gameFile.Name,
                    ReplacementImagePath = gameFile.FullName,
                    ReplacementImageMonsterName = "",
                    ReplacementImageFileName = ""
                    //ReplacementImagePath = replacementCard.ImagePath,
                    //ReplacementImageMonsterName = replacementCard.CardName,
                    //ReplacementImageFileName = replacementCard.ReplacementImageFileName
                });
            }
            return artworkList;
        }

        public ReplacementCard FindSuitableReplacementCard(FileInfo gameFile, Dictionary<string, string> replacementImageFiles)
        {
            FileInfo cardFile;
            try
            {
                var matchingCards = _db.Texts.Where(s => s.Name == "Blue-Eyes White Dragon").ToList();
                var card = matchingCards.FirstOrDefault();
                var cardPath = replacementImageFiles.FirstOrDefault(x => x.Key == card?.Id.ToString()).Value;
                cardFile = new FileInfo(cardPath);
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Databas error: {e}, inner: {e.InnerException}");
                //throw;
                return new ReplacementCard() { ReplacementImageFileName = "", ImagePath = "", CardName = "Error" };
            }

            return new ReplacementCard()
            {
                CardName = cardFile.Name,
                ImagePath = cardFile.FullName,
                ReplacementImageFileName = cardFile.Name
            };
        }

    }
}
