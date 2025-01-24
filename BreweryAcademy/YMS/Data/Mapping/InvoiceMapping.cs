using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YMS.Data.Mapping
{
	public class InvoiceMapping : IEntityTypeConfiguration<Invoice>
	{
		public void Configure(EntityTypeBuilder<Invoice> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.InvoiceId).IsRequired();
			builder.Property(x => x.InvoiceType).IsRequired();
			builder.HasMany<InvoiceItem>(x => x.Items).WithOne().HasForeignKey(x => x.Invoice);
		}
	}
}
