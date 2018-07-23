using System;
using System.Collections.Generic;
using System.Linq;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.Misc.Interface;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly ICardDbContext _db;
        private readonly ILogger _logger;

        public CardRepository(ICardDbContext db, ILogger logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public List<Card> GetCards(string cardName)
        {
            var cards = _db.Texts
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
            var cards = _db.Texts
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
