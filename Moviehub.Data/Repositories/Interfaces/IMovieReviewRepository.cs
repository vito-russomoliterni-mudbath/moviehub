using Moviehub.Data.Repositories.Dtos;

namespace Moviehub.Data.Repositories.Interfaces;

public interface IMovieReviewRepository
{
    Task AddMovieReview(MovieReviewAddDto review);
    Task<List<MovieReviewDto>> GetMovieReviews(int movieId);
    Task UpdateMovieReview(MovieReviewUpdateDto review);
    Task DeleteMovieReview(int reviewId);
}
