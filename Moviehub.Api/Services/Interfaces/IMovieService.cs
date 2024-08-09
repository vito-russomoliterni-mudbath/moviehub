using Moviehub.Api.Dtos;

namespace Moviehub.Api.Services.Interfaces;

public interface IMovieService
{
    Task<List<MovieDto>> GetMovies(string title = "", string genre = "");
    Task<MovieDetailDto?> GetMovieDetails(int id);
}
