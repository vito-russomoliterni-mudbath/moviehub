using Moviehub.Api.Dtos;

namespace Moviehub.Api.Services.Interfaces;

public interface IMovieReviewService
{
    Task<MovieReviewDto?> GetMovieReview(int id);
    Task<List<MovieReviewDto>> GetMovieReviewsByMovieId(int movieId);
    Task<MovieReviewDto> AddMovieReview(MovieReviewAddDto movieReviewAddDto);
    Task UpdateMovieReview(MovieReviewUpdateDto movieReviewUpdateDto);
    Task DeleteMovieReview(int id);
}
