using Moviehub.Data.Models;

namespace Moviehub.Data.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetMovies(string title = "", string genre = "");
    Task<MovieDetail?> GetMovieDetail(int id);
}
