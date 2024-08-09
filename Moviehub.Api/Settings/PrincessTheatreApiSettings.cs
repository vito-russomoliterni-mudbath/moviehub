namespace Moviehub.Api.Settings;

public class PrincessTheatreApiSettings
{
    public string BaseUrl { get; set; } = string.Empty;
    public string Endpoint { get; set; } = string.Empty;
    public string ApiKey { get; set; } = string.Empty;
    public List<string> MovieProviders { get; set; } = new();
}
