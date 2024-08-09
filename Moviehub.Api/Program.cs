using Moviehub.Data.Repositories.Interfaces;
using Moviehub.Data.Database;
using Moviehub.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Moviehub.Api.Services.Interfaces;
using Moviehub.Api.Services;
using Moviehub.Api.Settings;

var builder = WebApplication.CreateBuilder(args);

var assemblyPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
builder.Configuration
    .AddJsonFile(Path.Combine(assemblyPath, "appsettings.json"), optional: false, reloadOnChange: true)
    .AddJsonFile(Path.Combine(assemblyPath, $"appsettings.{builder.Environment.EnvironmentName}.json"), optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.Configure<PrincessTheatreApiSettings>(builder.Configuration.GetSection("PrincessTheatreApiSettings"));
builder.Services.AddHttpClient<PrincessTheatreService>();

builder.Services.AddControllers();
builder.Services.AddTransient<IMovieService, MovieService>();
builder.Services.AddTransient<IMovieReviewService, MovieReviewService>();
builder.Services.AddTransient<IPrincessTheatreService, PrincessTheatreService>();
builder.Services.AddTransient<IMovieRepository, MovieRepository>();
builder.Services.AddTransient<IMovieReviewRepository, MovieReviewRepository>();
builder.Services.AddDbContext<MoviehubDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddLogging(options =>
{
    options.AddConsole();
    options.AddDebug();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program { }