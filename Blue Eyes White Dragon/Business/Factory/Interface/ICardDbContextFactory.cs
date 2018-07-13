using Blue_Eyes_White_Dragon.DataAccess;

namespace Blue_Eyes_White_Dragon.Business.Factory.Interface
{
    public interface ICardDbContextFactory
    {
        CardDbContext CreateCardDbContext();
    }
}