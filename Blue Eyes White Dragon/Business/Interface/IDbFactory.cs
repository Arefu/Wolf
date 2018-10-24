using Blue_Eyes_White_Dragon.DataAccess;

namespace Blue_Eyes_White_Dragon.Business.Interface
{
    public interface IDbFactory
    {
        string CardDbLocation { get; set; }
        CardDbContext GetContext();
    }
}