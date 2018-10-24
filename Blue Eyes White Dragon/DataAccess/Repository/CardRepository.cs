using System;
using System.Collections.Generic;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc.Interface;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly IDbFactory _dbFactory;
        private readonly ILogger _logger;

        public CardRepository(IDbFactory dbFactory, ILogger logger)
        {
            _dbFactory = dbFactory ?? throw new ArgumentNullException(nameof(dbFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Card> GetCards(string cardName)
        {
            var cards = _dbFactory.GetContext().Texts
                .Where(s => s.Name == cardName)
                .Select(x =>
                    new Card() {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

            return cards;
        }

        public List<Card> SearchCards(string cardName)
        {
            var cards = _dbFactory.GetContext().Texts
                .Where(s => s.Name.ToLower().Contains(cardName.ToLower()))
                .Select(x =>
                    new Card()
                    {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

            return cards;
        }
    }

}
