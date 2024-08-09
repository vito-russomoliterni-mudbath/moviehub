using System.Data.Common;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Moviehub.Api.Services.Interfaces;
using Moviehub.Data.Database;

namespace Moviehub.Api.Tests;

public class TestingWebAppFactory<TProgram>()
    : WebApplicationFactory<TProgram> where TProgram : class
{
    internal readonly Mock<IPrincessTheatreService> MockPrincessTheatreService = new();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);
        
        builder.ConfigureServices(services =>
        {
            // Remove the real DbContextOptions and replace it with an in-memory database
            var dbContextDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(DbContextOptions<MoviehubDbContext>));

            if (dbContextDescriptor != null)
                services.Remove(dbContextDescriptor);

            var dbConnectionDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbConnection));

            if (dbConnectionDescriptor != null)
                services.Remove(dbConnectionDescriptor);

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();
                return connection;
            });
            
            services.AddDbContext<MoviehubDbContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                options.UseSqlite(connection);
            });
            
            // Remove the real PrincessTheatreService and replace it with a mock
            var princessTheatreServiceDescriptor = services.SingleOrDefault(
                d => d.ServiceType ==
                     typeof(IPrincessTheatreService));
            
            if (princessTheatreServiceDescriptor != null)
                services.Remove(princessTheatreServiceDescriptor);

            services.AddSingleton(MockPrincessTheatreService.Object);
        });
        base.ConfigureWebHost(builder);
    }
}
