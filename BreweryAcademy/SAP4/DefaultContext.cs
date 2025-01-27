using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SAP4.Entities;
using System.Reflection;


namespace SAP4
{
    public class DefaultContext : DbContext
    {
        public DefaultContext(DbContextOptions<DefaultContext> options)
        : base(options)
        {
        }

        public DbSet<InvoiceSAP> InvoiceSAPs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            //modelBuilder.Entity<InvoiceSAP>().HasKey(x => x.Id);

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<InvoiceSAP>(entity =>
            {
                entity.ToTable("InvoiceSAP");
                entity.HasKey(x => x.Id);

                // Add other configuration options for InvoiceSAP
            });
            */

            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            
       
        }
    }
}
