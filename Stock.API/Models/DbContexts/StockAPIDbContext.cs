using Microsoft.EntityFrameworkCore;

namespace Stock.API.Models.DbContexts
{
    public class StockAPIDbContext : DbContext
    {
        public StockAPIDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<Stock> Stocks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>().HasData(new List<Stock>()
            {
                new Stock()
                {
                    Id = Guid.Parse("654c2128-776e-4979-8940-b334a554b515"),
                    Count = 10,
                    ProductId = Guid.Parse("12a54947-1543-4473-b7e4-58e2c99a8f90")
                },
                new Stock()
                {
                    Id = Guid.Parse("5425173e-fcae-4820-a5a1-c25cf249c14c"),
                    Count = 10,
                    ProductId = Guid.Parse("b23cfb37-a81c-492d-9d99-5c4b68ece80b")
                },
                new Stock()
                {
                    Id = Guid.Parse("7e2f2b5a-848b-4d6e-b246-9308d1f69f22"),
                    Count = 10,
                    ProductId = Guid.Parse("74688be0-2c9a-4193-be98-3ea5e906a1f3")
                },
                new Stock()
                {
                    Id = Guid.Parse("15aa2d9c-e5f3-441d-be44-d28b5b3db862"),
                    Count = 10,
                    ProductId = Guid.Parse("540dba8c-5970-45a3-ab56-969c26677921")
                },
                new Stock()
                {
                    Id = Guid.Parse("15d80b0f-16cc-4ad3-be79-cf367d935764"),
                    Count = 10,
                    ProductId = Guid.Parse("fbf11afa-c125-4fa8-a046-5fe927d792c7")
                },
            });
        }
    }
}
