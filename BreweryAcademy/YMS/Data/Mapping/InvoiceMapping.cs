using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace YMS.Data.Mapping
{
	public class InvoiceMapping : IEntityTypeConfiguration<Invoice>
	{
		public void Configure(EntityTypeBuilder<Invoice> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.InvoiceId).IsRequired();
			builder.Property(x => x.InvoiceType).HasConversion<string>()
			.HasMaxLength(30).IsRequired();
			builder.HasMany<InvoiceItem>(x => x.Items).WithOne().HasForeignKey(x => x.InvoiceId);
			builder.HasOne(x => x.CheckIn).WithOne(x => x.Invoice).HasForeignKey<Invoice>(x => x.CheckInId);
		}
	}
}
