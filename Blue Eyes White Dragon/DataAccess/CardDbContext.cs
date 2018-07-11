using System.Configuration;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Blue_Eyes_White_Dragon.DataAccess
{
    public class CardDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder
        optionsBuilder)
        {
            var cardDbLocation = ConfigurationManager.AppSettings["CardDbLocation"];

            string connectionStringBuilder = new SqliteConnectionStringBuilder()
            {
                DataSource = cardDbLocation
            }.ToString();

            var connection = new SqliteConnection(connectionStringBuilder);
            optionsBuilder.UseSqlite(connection);
        }
        public DbSet<Texts> Texts { get; set; }
    }
}
