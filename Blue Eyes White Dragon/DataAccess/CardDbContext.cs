using System.Configuration;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Models;
using JetBrains.Annotations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blue_Eyes_White_Dragon.DataAccess
{
    public class CardDbContext : DbContext, ICardDbContext
    {
        public string CardDbLocation { get; set; }

        public CardDbContext(string cardDbLocation)
        {
            CardDbLocation = cardDbLocation;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder
        optionsBuilder)
        {
            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = CardDbLocation
            }.ToString();

            var connection = new SqliteConnection(connectionStringBuilder);
            optionsBuilder.UseSqlite(connection);
        }
        public DbSet<Texts> Texts { get; set; }
    }
}
