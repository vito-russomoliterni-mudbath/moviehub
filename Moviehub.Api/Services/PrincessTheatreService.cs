using Microsoft.Extensions.Options;
using Moviehub.Api.Dtos;
using Moviehub.Api.Services.Interfaces;
using Moviehub.Api.Settings;

namespace Moviehub.Api.Services;

public class PrincessTheatreService : IPrincessTheatreService
{
    private readonly HttpClient _httpClient;
    private readonly PrincessTheatreApiSettings _princessTheatreApiSettings;
    private readonly ILogger<PrincessTheatreService> _logger;

    public PrincessTheatreService(HttpClient httpClient, IOptions<PrincessTheatreApiSettings> options,
        ILogger<PrincessTheatreService> logger)
    {
        _httpClient = httpClient;
        _princessTheatreApiSettings = options.Value;
        _logger = logger;
    }

    public async Task<PrincessTheatreResponseDto?> GetPrincessTheatreMovies(string provider)
    {
        var url = $"{_princessTheatreApiSettings.BaseUrl}/{provider}/{_princessTheatreApiSettings.Endpoint}";
        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("x-api-key", _princessTheatreApiSettings.ApiKey);

        _logger.LogInformation("Fetching movies for provider {provider} with url {url}", provider, url);

        var response = await _httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();

        _logger.LogInformation("Movies successfully fetched for provider {provider}", provider);

        return await response.Content.ReadFromJsonAsync<PrincessTheatreResponseDto>();
    }
}
