using System.Collections.Generic;
using Blue_Eyes_White_Dragon.Business.Models;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface ICardRepository
    {
        List<Card> SearchCards(string cardName);
    }
}