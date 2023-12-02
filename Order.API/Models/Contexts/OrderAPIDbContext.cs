using Microsoft.EntityFrameworkCore;

namespace Order.API.Models.Contexts
{
    public class OrderAPIDbContext: DbContext
    {
        public OrderAPIDbContext(DbContextOptions dbContextOptions):base(dbContextOptions)
        {

        }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
    }
}
