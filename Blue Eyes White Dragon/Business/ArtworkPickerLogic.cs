using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Business
{
    public class ArtworkPickerLogic : IArtworkPickerLogic
    {
        private readonly ICardRepository _cardRepo;
        private readonly IArtworkManager _artworkManager;
        private readonly ISettingRepository _settingRepo;

        public ArtworkPickerLogic(ICardRepository cardRepo, IArtworkManager artworkManager, ISettingRepository settingRepo)
        {
            _cardRepo = cardRepo ?? throw new ArgumentNullException(nameof(cardRepo));
            _artworkManager = artworkManager ?? throw new ArgumentNullException(nameof(artworkManager));
            _settingRepo = settingRepo ?? throw new ArgumentNullException(nameof(settingRepo));
        }

        public IEnumerable<ArtworkSearch> SearchArtwork(string cardName)
        {
            var cards = _cardRepo.SearchCards(cardName);
            var replacementImagesLocation = new DirectoryInfo(_settingRepo.GetPathSetting(Constants.Setting.LastUsedReplacementImagePath));

            return cards.Select(x => new ArtworkSearch()
            {
                CardId = x.Id,
                CardName = x.Name,
                ImageFile = _artworkManager.SearchForImage(x.Id, replacementImagesLocation)
            });
        }
    }
}
