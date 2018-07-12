using System.Collections.Generic;
using Blue_Eyes_White_Dragon.Business.Models;
using Blue_Eyes_White_Dragon.DataAccess.Models;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface ICardRepository
    {
        List<Card> SearchCard(string gameCardName);
    }
}