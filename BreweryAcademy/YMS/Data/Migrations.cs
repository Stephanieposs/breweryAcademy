namespace YMS.Data
{
	public static class Migrations
	{
		public static WebApplication AutoMigrate(this WebApplication app) {
			using var scope = app.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();

			context.Database.MigrateAsync().GetAwaiter().GetResult();

			return app;
		}
	}
}
