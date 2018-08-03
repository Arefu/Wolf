using Blue_Eyes_White_Dragon.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace Blue_Eyes_White_Dragon.DataAccess.Interface
{
    public interface ICardDbContext
    {
        DbSet<Texts> Texts { get; set; }
    }
}