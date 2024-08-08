namespace Moviehub.Api.Dtos;

public class MovieDetailDto : MovieDto
{
    public List<CinemaDto> Cinemas { get; set; } = new();
}
