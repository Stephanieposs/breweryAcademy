
using BuildingBlocks.Behaviours;
using HealthChecks.UI.Client;


var builder = WebApplication.CreateBuilder(args);
var assemblies = AppDomain.CurrentDomain.GetAssemblies();

/*
builder.Services.AddControllers().AddJsonOptions(options =>
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 
*/
builder.Services.AddDbContext<DefaultContext>(opt =>
	opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")).LogTo(Console.WriteLine, LogLevel.Information),
	ServiceLifetime.Scoped);



Log.Logger = new LoggerConfiguration()
	.WriteTo.Console()  
	.Enrich.FromLogContext() 
	.CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddRabbitMQServices();


builder.Services.AddExceptionHandler<CustomExceptionHandler>();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("Database")!);

builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();
builder.Services.AddScoped<IInvoiceRepository, InvoiceRepository>();	
builder.Services.AddScoped<CheckInService>();
builder.Services.AddScoped<InvoiceService>();

builder.Services.AddAutoMapper(opt => opt.AddMaps(assemblies));

var app = builder.Build();
app.UseMiddleware<LoggingBehaviour>();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
	ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});



app.UseExceptionHandler(opt => { });
app.UseAuthorization();
app.AutoMigrate();
app.MapControllers();

app.Run();
