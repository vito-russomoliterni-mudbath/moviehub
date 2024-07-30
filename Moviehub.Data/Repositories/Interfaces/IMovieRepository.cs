using Moviehub.Data.Repositories.Models;

namespace Moviehub.Data.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetMovies(string title = "", string genre = "");
    Task<MovieDetail?> GetMovieDetail(int id);
}
