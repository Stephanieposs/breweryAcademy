using Microsoft.EntityFrameworkCore;
using WMS.Entities;
namespace WMS.Data
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options): base(options) {}

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Item> Items { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>()
                .HasKey(p => p.Id);

            modelBuilder.Entity<Stock>()
                .HasKey(s => s.Id);

            modelBuilder.Entity<Item>()
            .Property(i => i.Id)
            .ValueGeneratedNever();

            base.OnModelCreating(modelBuilder);
        }
    }
}
