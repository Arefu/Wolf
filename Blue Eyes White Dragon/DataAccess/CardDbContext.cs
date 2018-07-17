using System.Configuration;
using Blue_Eyes_White_Dragon.DataAccess.Interface;
using Blue_Eyes_White_Dragon.DataAccess.Models;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blue_Eyes_White_Dragon.DataAccess
{
    public class CardDbContext : DbContext, ICardDbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder
        optionsBuilder)
        {
            var cardDbLocation = ConfigurationManager.AppSettings["CardDbLocation"];

            var connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = cardDbLocation
            }.ToString();

            var connection = new SqliteConnection(connectionStringBuilder);
            optionsBuilder.UseSqlite(connection);
        }
        public DbSet<Texts> Texts { get; set; }
    }
}
