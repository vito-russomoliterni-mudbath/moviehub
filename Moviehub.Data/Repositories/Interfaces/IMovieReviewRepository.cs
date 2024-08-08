using Moviehub.Data.Database.Entities;

namespace Moviehub.Data.Repositories.Interfaces;

public interface IMovieReviewRepository
{
    Task<MovieReview?> GetMovieReview(int id);
    Task<List<MovieReview>> GetMovieReviewsByMovieId(int movieId);
    Task<MovieReview> AddMovieReview(MovieReview movieReview);
    Task UpdateMovieReview(MovieReview movieReview);
    Task DeleteMovieReview(MovieReview movieReview);
}
