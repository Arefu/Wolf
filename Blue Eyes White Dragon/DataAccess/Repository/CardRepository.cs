using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Models;
using Yu_Gi_Oh.Save_File;

namespace Blue_Eyes_White_Dragon.DataAccess.Repository
{
    public class CardRepository : ICardRepository
    {
        private readonly CardDbContext _db;

        public CardRepository(CardDbContext db)
        {
            _db = db;
        }

        public List<Card> SearchCard(string gameCardName)
        {
            var cards = _db.Texts
                .Where(s => s.Name == gameCardName)
                .Select(x =>
                    new Card() {
                        Id = x.Id,
                        Name = x.Name
                    }).ToList();

            return cards;
        }
    }

}
