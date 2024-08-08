using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moviehub.Data.Database;
using Moviehub.Data.Database.Entities;
using Moviehub.Data.Repositories.Interfaces;

namespace Moviehub.Data.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly MoviehubDbContext _context;
    private readonly ILogger<MovieRepository> _logger;

    public MovieRepository(MoviehubDbContext context, ILogger<MovieRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<List<Movie>> GetMovies(string title = "", string genre = "")
    {
        var movieQuery = _context.Movies.Include(m => m.MovieReviews).AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            _logger.LogInformation("Filtering movies by title {title}", title);
            movieQuery = movieQuery.Where(m => m.Title.ToLower().Contains(title.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            _logger.LogInformation("Filtering movies by genre {genre}", genre);
            movieQuery = movieQuery.Where(m => m.Genre.ToLower().Contains(genre.ToLower()));
        }

        return await movieQuery.ToListAsync();
    }

    public async Task<Movie?> GetMovie(int id) =>
        await _context.Movies
            .Include(m => m.MovieReviews)
            .Include(m => m.MovieCinemas)
                .ThenInclude(mc => mc.Cinema)
            .FirstOrDefaultAsync(m => m.Id == id);
}
