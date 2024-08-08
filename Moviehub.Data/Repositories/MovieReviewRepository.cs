using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moviehub.Data.Database;
using Moviehub.Data.Repositories.Interfaces;
using Moviehub.Data.Database.Entities;

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

    public async Task<MovieReview?> GetMovieReview(int id) => 
        await _context.MovieReviews.FindAsync(id);

    public async Task<List<MovieReview>> GetMovieReviewsByMovieId(int movieId) =>
        await _context.MovieReviews
            .Where(r => r.MovieId == movieId)
            .ToListAsync();

    public async Task<MovieReview> AddMovieReview(MovieReview movieReview) 
    {
        var trackedMovieReview = _context.MovieReviews.Add(movieReview);
        await _context.SaveChangesAsync();
        return trackedMovieReview.Entity;
    }

    public async Task UpdateMovieReview(MovieReview movieReview)
    {
        _context.MovieReviews.Update(movieReview);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteMovieReview(MovieReview movieReview)
    {
        _context.MovieReviews.Remove(movieReview);
        await _context.SaveChangesAsync();
    }
}
