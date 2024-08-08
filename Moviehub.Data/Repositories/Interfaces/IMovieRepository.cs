using Moviehub.Data.Database.Entities;

namespace Moviehub.Data.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<Movie>> GetMovies(string title = "", string genre = "");
    Task<Movie?> GetMovie(int id);
}
