using MassTransit;
using SAP4.Bus;
using SAP4.Services;

namespace SAP4.Extensions;

internal static class AppExtensions
{
    public static void AddRabbitMQService(this IServiceCollection services)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.AddConsumer<RecordRequestedEventConsumer>();

            busConfigurator.UsingRabbitMq((ctx, cfg)=>
            {
                cfg.Host(new Uri("amqp://localhost:5672"), host =>
                {
                    host.Username("guest");
                    host.Password("guest");
                });

                cfg.ConfigureEndpoints(ctx);
            });
        });
    }
}
