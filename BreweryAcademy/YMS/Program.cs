
using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

/*
builder.Services.AddControllers().AddJsonOptions(options =>
	options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter())); 
*/
builder.Services.AddDbContext<DefaultContext>(opt =>
	opt.UseSqlServer(builder.Configuration.GetConnectionString("Database")).LogTo(Console.WriteLine, LogLevel.Information),
	ServiceLifetime.Scoped);

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
    });

builder.Services.AddHealthChecks().AddSqlServer(builder.Configuration.GetConnectionString("Database")!);

builder.Services.AddSwaggerGen();

var assemblies = AppDomain.CurrentDomain.GetAssemblies();
builder.Services.AddScoped<ICheckInRepository, CheckInRepository>();
builder.Services.AddScoped<CheckInService>();
builder.Services.AddAutoMapper(opt => opt.AddMaps(assemblies));

var app = builder.Build();

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
app.UseAuthorization();

app.MapControllers();

app.Run();
