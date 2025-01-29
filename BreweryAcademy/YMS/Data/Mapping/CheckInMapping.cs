
namespace YMS.Data.Mapping
{
	public class CheckInMapping : IEntityTypeConfiguration<CheckIn>
	{
		public void Configure(EntityTypeBuilder<CheckIn> builder)
		{
			builder.HasKey(x => x.Id);
			builder.Property(x => x.DriverDocument).HasMaxLength(20);
			builder.Property(x => x.TruckPlate).HasMaxLength(14);
		}
	}
}
