using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YMS.Data.Mapping
{
	public class InvoiceItemMapping : IEntityTypeConfiguration<InvoiceItem>
	{
		public void Configure(EntityTypeBuilder<InvoiceItem> builder)
		{
			builder.ToTable(nameof(InvoiceItem));
			builder.HasKey(x => x.InvoiceItemId);
			builder.Property(x => x.ProductId).IsRequired();
			builder.Property(x => x.Quantity).IsRequired();
		}
	}
}
