using Microsoft.EntityFrameworkCore;
using Moviehub.Data.Repositories.Interfaces;
using Moviehub.Data.Repositories.Models;
using Moviehub.Data.Database;
using Microsoft.Extensions.Logging;

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
        var movies = new List<Movie>();
        var movieQuery = _context.Movies.AsQueryable();

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

        var entityMovies = await movieQuery.ToListAsync();
        _logger.LogInformation("{Count} movies found", entityMovies.Count);

        foreach (var entityMovie in entityMovies)
        {
            movies.Add(new Movie
            {
                Id = entityMovie.Id,
                Title = entityMovie.Title,
                ReleaseDate = entityMovie.ReleaseDate,
                Genre = entityMovie.Genre,
                Runtime = entityMovie.Runtime,
                Synopsis = entityMovie.Synopsis,
                Director = entityMovie.Director,
                Rating = entityMovie.Rating
            });
        }

        return movies;
    }

    public async Task<MovieDetail?> GetMovieDetail(int id)
    {
        var entityMovie = await _context.Movies.FindAsync(id);
        if (entityMovie == null)
        {
            _logger.LogInformation("No movie found for id {id}", id);
            return null;
        }
        
        var movieCinemas = await _context.MovieCinemas
            .Include(mc => mc.Cinema)
            .Where(mc => mc.MovieId == id)
            .ToListAsync();

        _logger.LogInformation("{Count} cinemas found for movie with id {id}", movieCinemas.Count, id);

        return new MovieDetail
        {
            Id = entityMovie.Id,
            Title = entityMovie.Title,
            ReleaseDate = entityMovie.ReleaseDate,
            Genre = entityMovie.Genre,
            Runtime = entityMovie.Runtime,
            Synopsis = entityMovie.Synopsis,
            Director = entityMovie.Director,
            Rating = entityMovie.Rating,
            Cinemas = movieCinemas.Select(mc => new Cinema
            {
                Name = mc.Cinema.Name,
                Showtime = mc.Showtime,
                TicketPrice = mc.TicketPrice
            }).ToList()
        };
    }
}
