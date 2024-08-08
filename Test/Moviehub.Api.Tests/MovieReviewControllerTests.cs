using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Moviehub.Data.Database;
using Moviehub.Api.Dtos;
using Xunit;
using static Moviehub.Api.Tests.TestDbHelper;

namespace Moviehub.Api.Tests;

public class MovieReviewControllerTests
    : IClassFixture<TestingWebAppFactory<Program>>, IAsyncLifetime
{
    
    private readonly HttpClient _client;
    private readonly TestingWebAppFactory<Program> _factory;

    public MovieReviewControllerTests(TestingWebAppFactory<Program> factory)
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
    public async Task AddMovieReview_ReturnsSuccessStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/movieReview")
        {
            Content = new StringContent(JsonSerializer.Serialize(new MovieReviewAddDto
            {
                MovieId = 1,
                Score = 3.5m,
                Comment = "I've seen better.",
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task AddMovieReview_ReturnsBadRequestStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Post, "/api/movieReview")
        {
            Content = new StringContent(JsonSerializer.Serialize(new MovieReviewAddDto
            {
                MovieId = 99,
                Score = 0.5m,
                Comment = "Ugh.",
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetMovieReviews_ReturnsSuccessStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Get, "/api/movieReview?movieId=1");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<List<MovieReviewDto>>();
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task UpdateMovieReview_ReturnsSuccessStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/movieReview")
        {
            Content = new StringContent(JsonSerializer.Serialize(new MovieReviewUpdateDto
            {
                Id = 1,
                Score = 4.5m,
                Comment = "A classic movie that will stand the test of time.",
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task UpdateMovieReview_ReturnsNotFoundStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Patch, "/api/movieReview")
        {
            Content = new StringContent(JsonSerializer.Serialize(new MovieReviewUpdateDto
            {
                Id = 99,
                Score = 4.5m,
                Comment = "A classic movie that will stand the test of time.",
            }), Encoding.UTF8, "application/json")
        };

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteMovieReview_ReturnsSuccessStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/movieReview/1");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        response.EnsureSuccessStatusCode();
    }

    [Fact]
    public async Task DeleteMovieReview_ReturnsNotFoundStatusCode()
    {
        // Arrange
        var request = new HttpRequestMessage(HttpMethod.Delete, "/api/movieReview/99");

        // Act
        var response = await _client.SendAsync(request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}
