namespace Moviehub.Data.Repositories.Dtos;

public class MovieDetail : MovieDto
{
    public List<CinemaDto> Cinemas { get; set; } = new();
}
