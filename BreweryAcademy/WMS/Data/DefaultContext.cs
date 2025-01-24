using Microsoft.EntityFrameworkCore;
using WMS.Entities;
namespace WMS.Data
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options): base(options) {}

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
           
        }
    }
}
