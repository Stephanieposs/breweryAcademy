using BuildingBlocks.Behaviours;
using BuildingBlocks.Exceptions.Handler;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SAP4;
using SAP4.Extensions;
using SAP4.Repositories;
using SAP4.Services;
using Serilog;

public partial class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .Enrich.FromLogContext()
            .CreateLogger();

        builder.Host.UseSerilog();


        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddScoped<ISapService, SapService>();
        builder.Services.AddScoped<ISapRepository, SapRepository>();
        //builder.Services.AddSingleton<IBackgroundTaskQueue, InMemoryBackgroundTaskQueue>();
        //builder.Services.AddHostedService<SAP4.SapInvoiceProcessor.SapInvoiceProcessor>();

        //builder.Services.AddHostedService<Worker>();

        builder.Services.AddDbContext<DefaultContext>(options =>
            options.UseSqlServer(
                builder.Configuration.GetConnectionString("DefaultConnection")));

        builder.Services.AddExceptionHandler<CustomExceptionHandler>();

        // Adding rabbitMQ
        builder.Services.AddRabbitMQService();

        var app = builder.Build();

        app.UseMiddleware<LoggingBehaviour>();

        app.UseExceptionHandler(opt => { });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        // Apply migrations automatically
        ApplyMigrations(app);

        app.MapGet("/", () => "Hello, World!");

        app.Run();

        //CreateHostBuilder(args).Build().Run();
    }

    private static void ApplyMigrations(WebApplication app)
    {
        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<DefaultContext>();

            // Check and apply pending migrations
            var pendingMigrations = dbContext.Database.GetPendingMigrations();
            if (pendingMigrations.Any())
            {
                Console.WriteLine("Applying pending migrations...");
                dbContext.Database.Migrate();
                Console.WriteLine("Migrations applied successfully.");
            }
            else
            {
                Console.WriteLine("No pending migrations found.");
            }
        }
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                // Register SAP connection
                //services.AddSingleton(new SqlConnection("myconnection"));

                // Get the connection string from the host context
                var connectionString = hostContext.Configuration.GetConnectionString("DefaultConnection");

                // Register the DbContext with the connection string
                services.AddDbContext<DefaultContext>(options =>
                    options.UseSqlServer(connectionString));

                // Register background service
                //services.AddHostedService<SapInvoiceProcessor>();

                // Add logging
                services.AddLogging(loggingBuilder =>
                {
                    loggingBuilder.AddConsole();
                    loggingBuilder.AddDebug();
                });
            });
}