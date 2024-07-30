namespace Moviehub.Data.Repositories.Models;

public class MovieDetail : Movie
{
    public List<Cinema> Cinemas { get; set; } = new();
}
