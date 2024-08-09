using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.DependencyInjection;
using Moviehub.Data.Database;
using Moviehub.Api.Dtos;
using Xunit;
using static Moviehub.Api.Tests.TestDbHelper;
using Moq;

namespace Moviehub.Api.Tests;

public class MovieControllerTests
    : IClassFixture<TestingWebAppFactory<Program>>, IAsyncLifetime
{
    private readonly HttpClient _client;
    private readonly TestingWebAppFactory<Program> _factory;

    public MovieControllerTests(TestingWebAppFactory<Program> factory)
    {
        _client = factory.CreateClient();
        _factory = factory;
    }

    public async Task InitializeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MoviehubDbContext>();
        await dbContext.Database.EnsureCreatedAsync();
        await SeedData(dbContext);
    }

    public async Task DisposeAsync()
    {
        using var scope = _factory.Services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MoviehubDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
    }

    [Fact]
    public async Task GetMovies_ReturnsSuccessStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/movie");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<MovieDto>>();
        Assert.NotNull(result);
        Assert.Single(result);
    }

    [Fact]
    public async Task GetMovies_ReturnsNotFoundStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/movie?title=Invalid");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task GetMovieDetails_ReturnsSuccessStatusCode()
    {
        // Arrange
        _factory.MockPrincessTheatreService
            .Setup(x => x.GetPrincessTheatreMovies(It.IsAny<string>()))
            .ReturnsAsync(new PrincessTheatreResponseDto());
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/movie/1");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<MovieDetailDto>();
        Assert.NotNull(result);
        Assert.Equal("The Shawshank Redemption", result.Title);
        Assert.NotEmpty(result.Cinemas);
        Assert.Equal(10, result.Cinemas.First().TicketPrice);
        Assert.Equal(4.25m, result.AvgScore);
    }

    [Fact]
    public async Task GetMovieDetails_ReturnsNotFoundStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/movie/99");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
