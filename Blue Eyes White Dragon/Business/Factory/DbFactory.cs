using System;
using Blue_Eyes_White_Dragon.Business.Interface;
using Blue_Eyes_White_Dragon.DataAccess;

namespace Blue_Eyes_White_Dragon.Business.Factory
{
    public class DbFactory : IDbFactory
    {
        private CardDbContext _cardDbContext;
        public string CardDbLocation { get; set; }

        public CardDbContext GetContext()
        {
            if (_cardDbContext == null || !_cardDbContext.CardDbLocation.Equals(CardDbLocation))
            {
                _cardDbContext = Create();
                return _cardDbContext;
            }
            else
            {
                return _cardDbContext;
            }
        }

        private CardDbContext Create()
        {
            if (!string.IsNullOrEmpty(CardDbLocation))
            {
                return new CardDbContext(CardDbLocation);
            }
            else
            {
                throw new ArgumentNullException("CardDbLocation");
            }
        }
    }
}
