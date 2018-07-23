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
        private readonly IFileRepository _fileRepo;
        private readonly IArtworkManager _artworkManager;
        private readonly ISettingRepository _settingRepo;

        public ArtworkPickerLogic(ICardRepository cardRepo, IFileRepository fileRepo, IArtworkManager artworkManager, ISettingRepository settingRepo)
        {
            _cardRepo = cardRepo ?? throw new ArgumentNullException(nameof(cardRepo));
            _fileRepo = fileRepo;
            _artworkManager = artworkManager;
            _settingRepo = settingRepo;
        }

        public IEnumerable<ArtworkSearch> SearchCards(string cardName)
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
