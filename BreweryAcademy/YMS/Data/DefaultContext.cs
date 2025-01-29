namespace YMS.Data
{
	public class DefaultContext:DbContext
	{
		public DefaultContext(DbContextOptions<DefaultContext> options) : base(options) { 
			
		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
			base.OnModelCreating(modelBuilder);

		}

		public DbSet<CheckIn> CheckIns { get; set; }
		public DbSet<Invoice> Invoices { get; set; }
		public DbSet<InvoiceItem> InvoiceItems { get; set; }
	}
}
