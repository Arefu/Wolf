using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Models;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.Misc.Interface;
using Blue_Eyes_White_Dragon.UI.Models;
using Newtonsoft.Json;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class SaveAndAndLoadRepository : ISaveAndLoadRepository
    {
        private readonly string _directoryName;
        private readonly ILogger _logger;

        public SaveAndAndLoadRepository(string directoryName, ILogger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _directoryName = directoryName;
        }

        public string SaveArtworkMatchToFile(IEnumerable<Artwork> artworkList)
        {
            var fileName = Constants.ArtworkMatchFileName;
            var path = Path.Combine(_directoryName, fileName);

            var jsonArtwork = JsonConvert.SerializeObject(artworkList.ToList().ConvertAll(ToArtworkSerialize), Formatting.Indented);
            File.WriteAllText(path, jsonArtwork);
            return path;
        }

        private ArtworkSerialize ToArtworkSerialize(Artwork artwork)
        {
            return new ArtworkSerialize
            {
                GameImageFilePath = artwork.GameImageFilePath,
                GameImageMonsterName = artwork.GameImageMonsterName,
                ReplacementImageFilePath = artwork.ReplacementImageFilePath,
                ReplacementImageMonsterName = artwork.ReplacementImageMonsterName,
                AlternateReplacementImages = artwork.AlternateReplacementImages.Select(y => y.FullName).ToList(),
                CardId = artwork.CardId,
                IsPendulum = artwork.IsPendulum,
                IsMatched = artwork.IsMatched,
                ZibFilename = artwork.ZibFilename
            };
        }

        public IEnumerable<Artwork> LoadArtworkMatchFromFile(string path)
        {
            try
            {
                using (var reader = new StreamReader(path))
                {
                    var json = reader.ReadToEnd();
                    return JsonConvert.DeserializeObject<List<ArtworkSerialize>>(json).ConvertAll(x => ToArtworks(x));
                }
            }
            catch (Exception e)
            {
                _logger.LogException(e);
                throw;
            }
        }

        private Artwork ToArtworks(ArtworkSerialize artworkSerialize)
        {
            return new Artwork()
            {
                GameImageFile = new FileInfo(artworkSerialize.GameImageFilePath),
                GameImageMonsterName = artworkSerialize.GameImageMonsterName,
                AlternateReplacementImages = artworkSerialize.AlternateReplacementImages.Select(z => new FileInfo(z)).ToList(),
                ReplacementImageFile = new FileInfo(artworkSerialize.ReplacementImageFilePath),
                ReplacementImageMonsterName = artworkSerialize.ReplacementImageMonsterName,
                CardId = artworkSerialize.CardId,
                IsMatched = artworkSerialize.IsMatched,
                IsPendulum = artworkSerialize.IsPendulum,
                ZibFilename = artworkSerialize.ZibFilename
            };
        }
    }
}
