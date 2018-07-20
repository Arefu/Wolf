using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Presenter;
using Blue_Eyes_White_Dragon.UI.Interface;
using Blue_Eyes_White_Dragon.UI.Models;

namespace Blue_Eyes_White_Dragon.Business
{
    public class ArtworkPickerLogic : IArtworkPickerLogic
    {
        private readonly ICardRepository _cardRepo;

        public ArtworkPickerLogic(ICardRepository cardRepo)
        {
            _cardRepo = cardRepo ?? throw new ArgumentNullException(nameof(cardRepo));
        }

        public IEnumerable<ArtworkSearch> SearchCards(string cardName)
        {
            var cards = _cardRepo.SearchCards(cardName);

            foreach (var card in cards)
            {
                
            }

            var a = cards.Select(x => new ArtworkSearch()
            {
                //ImageFile = x
            });

            return null;
        }
    }
}
