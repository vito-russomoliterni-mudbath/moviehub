using Moviehub.Data.Repositories.Models;

namespace Moviehub.Data.Repositories.Interfaces;

public interface IMovieReviewRepository
{
    Task AddMovieReview(AddMovieReview review);
    Task<List<MovieReview>> GetMovieReviews(int movieId);
    Task UpdateMovieReview(UpdateMovieReview review);
    Task DeleteMovieReview(int reviewId);
}
