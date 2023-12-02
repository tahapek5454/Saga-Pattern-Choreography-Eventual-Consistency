using Microsoft.EntityFrameworkCore;

namespace Stock.API.Models.DbContexts
{
    public class StockAPIDbContext : DbContext
    {
        public StockAPIDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Stock> Stocks { get; set; }
    }
}
