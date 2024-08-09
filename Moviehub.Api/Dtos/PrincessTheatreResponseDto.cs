using System.Text.Json.Serialization;

namespace Moviehub.Api.Dtos;

public class PrincessTheatreResponseDto
{
    public string Provider { get; set; } = string.Empty;
    public List<PrincessTheatreMovieDto> Movies { get; set; } = new();
}

public class PrincessTheatreMovieDto
{
    [JsonPropertyName("ID")]
    public string Id { get; set; } = string.Empty;
    public decimal Price { get; set; }
}
