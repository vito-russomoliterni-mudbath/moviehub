namespace Moviehub.Data.Models;

public class MovieDetail : Movie
{
    public List<Cinema> Cinemas { get; set; } = new();
}
