using Moviehub.Data.Repositories.Dtos;

namespace Moviehub.Data.Repositories.Interfaces;

public interface IMovieRepository
{
    Task<List<MovieDto>> GetMovies(string title = "", string genre = "");
    Task<MovieDetail?> GetMovieDetail(int id);
}
