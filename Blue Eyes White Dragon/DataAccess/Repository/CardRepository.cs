using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Models;
using Blue_Eyes_White_Dragon.Utility.Interface;
using Yu_Gi_Oh.Save_File;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly ICardDbContext _db;
        private readonly ILogger _logger;

        public CardRepository(ICardDbContext db, ILogger logger)
        {
            _db = db;
            _logger = logger;
        }

        public List<Card> SearchCards(string cardName)
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

        public List<Card> SearchCardsAsync(ICardDbContext db, string cardName)
        {
            var cards = db.Texts
                .Where(s => s.Name == cardName)
                .Select(x =>
                    new Card() {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

            return cards;
        }
    }

}
