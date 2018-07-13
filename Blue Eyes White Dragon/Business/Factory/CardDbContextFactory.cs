using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blue_Eyes_White_Dragon.Business.Factory.Interface;
using Blue_Eyes_White_Dragon.DataAccess;

namespace Blue_Eyes_White_Dragon.Business.Factory
{
    public class CardDbContextFactory : ICardDbContextFactory
    {
        public CardDbContext CreateCardDbContext()
        {
            return new CardDbContext();
        }
    }
}
