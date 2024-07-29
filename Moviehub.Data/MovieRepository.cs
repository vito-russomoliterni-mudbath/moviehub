using Microsoft.EntityFrameworkCore;
using Moviehub.Data.Interfaces;
using Moviehub.Data.Models;
using Moviehub.Db;

namespace Moviehub.Data;

public class MovieRepository : IMovieRepository
{
    private readonly MoviehubDbContext _context;

    public MovieRepository(MoviehubDbContext context)
    {
        _context = context;
    }

    public async Task<List<Movie>> GetMovies(string title = "", string genre = "")
    {
        var movies = new List<Movie>();
        var movieQuery = _context.Movie.AsQueryable();

        if (!string.IsNullOrWhiteSpace(title))
        {
            movieQuery = movieQuery.Where(m => m.Title.Contains(title));
        }

        if (!string.IsNullOrWhiteSpace(genre))
        {
            movieQuery = movieQuery.Where(m => m.Genre.Contains(genre));
        }

        var entityMovies = await movieQuery.ToListAsync();

        foreach (var entityMovie in movieQuery)
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
        var entityMovie = await _context.Movie.FindAsync(id);
        if (entityMovie == null)
        {
            return null;
        }
        
        var movieCinemas = await _context.MovieCinema
            .Include(mc => mc.Cinema)
            .Where(mc => mc.MovieId == id)
            .ToListAsync();

        if (!movieCinemas.Any())
        {
            return null;
        }

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
