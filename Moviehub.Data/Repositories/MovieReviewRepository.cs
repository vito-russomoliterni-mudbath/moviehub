using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moviehub.Data.Database;
using Moviehub.Data.Repositories.Interfaces;
using Moviehub.Data.Repositories.Models;
using DbMovieReview = Moviehub.Data.Database.Entities.MovieReview;

namespace Moviehub.Data.Repositories;

public class MovieReviewRepository : IMovieReviewRepository
{
    private readonly MoviehubDbContext _context;
    private readonly ILogger<MovieReviewRepository> _logger;

    public MovieReviewRepository(MoviehubDbContext context, ILogger<MovieReviewRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task AddMovieReview(AddMovieReview review)
    {
        var movie = await _context.Movies.FindAsync(review.MovieId);

        if (movie == null)
        {
            _logger.LogWarning("Movie {movieId} not found", review.MovieId);
            throw new ArgumentException($"Movie {review.MovieId} not found");
        }

        _logger.LogInformation("Adding review for movie {movieId}", review.MovieId);

        var entityReview = new DbMovieReview
        {
            Movie = movie,
            Score = review.Score,
            Comment = review.Comment,
            ReviewDate = review.ReviewDate
        };

        _context.MovieReviews.Add(entityReview);
        await _context.SaveChangesAsync();
    }

    public async Task<List<MovieReview>> GetMovieReviews(int movieId)
    {
        var reviews =  await _context.MovieReviews
            .Where(r => r.MovieId == movieId)
            .ToListAsync();

        if (!reviews.Any())
        {
            _logger.LogWarning("No reviews found for movie {movieId}", movieId);
            return new List<MovieReview>();
        }

        _logger.LogInformation("Retrieved {reviewCount} reviews for movie {movieId}", reviews.Count, movieId);

        return reviews.Select(r => new MovieReview
        {
            Id = r.Id,
            Comment = r.Comment,
            MovieId = r.MovieId,
            ReviewDate = r.ReviewDate,
            Score = r.Score
        }).ToList();
    }

    public async Task UpdateMovieReview(UpdateMovieReview review)
    {
        var entityReview = await _context.MovieReviews.FindAsync(review.Id);

        if (entityReview == null)
        {
            _logger.LogWarning("Review {reviewId} not found", review.Id);
            throw new ArgumentException($"Review {review.Id} not found");
        }

        _logger.LogInformation("Updating review {reviewId}", review.Id);

        entityReview.Score = review.Score;
        entityReview.Comment = review.Comment;
        entityReview.ReviewDate = review.ReviewDate;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteMovieReview(int reviewId)
    {
        var review = await _context.MovieReviews.FindAsync(reviewId);

        if (review == null)
        {
            _logger.LogWarning("Review {reviewId} not found", reviewId);
            throw new ArgumentException($"Review {reviewId} not found");
        }

        _logger.LogInformation("Deleting review {reviewId}", reviewId);

        _context.MovieReviews.Remove(review);
        await _context.SaveChangesAsync();
    }
}
