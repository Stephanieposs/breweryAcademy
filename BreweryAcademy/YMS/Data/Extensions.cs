
using YMS.Data.MessageBroker.Consumer;

namespace YMS.Data
{
	public static class Extensions
	{
		public static WebApplication AutoMigrate(this WebApplication app) {
			using var scope = app.Services.CreateScope();
			using var context = scope.ServiceProvider.GetRequiredService<DefaultContext>();

			context.Database.MigrateAsync().GetAwaiter().GetResult();

			return app;
		}
		public static void AddRabbitMQServices(this IServiceCollection services) {
			services.AddMassTransit(busConfigurator =>
			{
				busConfigurator.AddConsumer<StockUpdateConsumer>();

				busConfigurator.UsingRabbitMq((ctx, cfg) => {
					cfg.Host(new Uri("amqp://localhost:5672"), host=> {
						host.Username("guest");
						host.Password("guest");
					});

					cfg.ConfigureEndpoints(ctx);
				});
			});
		}
	}
}
