using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SAP4.Entities;

namespace SAP4;

public class SAPConfiguration : IEntityTypeConfiguration<InvoiceSAP>
{
    
        public void Configure(EntityTypeBuilder<InvoiceSAP> builder)
        {
            builder.ToTable("InvoiceSAP"); // Set the table name
            builder.HasKey(x => x.Id);     // Define the primary key
                                           
        }
    
}
