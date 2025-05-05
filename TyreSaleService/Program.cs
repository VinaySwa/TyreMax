using Microsoft.EntityFrameworkCore;
using Serilog;
using TyreSaleService.Data;
using TyreSaleService.Repository;
using TyreSaleService.Services;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

builder.Services.AddSingleton<IConfiguration>(configuration);

var logFilePath = configuration.GetSection("Logging:File:Path").Value ?? "Logs/TyreSaleServiceLog.txt";
var logDirectory = Path.GetDirectoryName(logFilePath);
if (!Directory.Exists(logDirectory))
{
    Directory.CreateDirectory(logDirectory); // Create directory if missing
}

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day, fileSizeLimitBytes: 10485760, rollOnFileSizeLimit: true)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddDbContext<AppDbContext>(opts =>
    opts.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<ITyreRepository, TyreRepository>();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

builder.Services.AddScoped<ITyreCompanyService, TyreCompanyService>();
builder.Services.AddScoped<ITyreModelService, TyreModelService>();
builder.Services.AddScoped<ITyreService, TyreService>();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
