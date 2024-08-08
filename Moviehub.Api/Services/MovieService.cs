using Moviehub.Api.Services.Interfaces;
using Moviehub.Data.Database.Entities;
using Moviehub.Api.Dtos;
using Moviehub.Data.Repositories.Interfaces;

namespace Moviehub.Api.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly ILogger<MovieService> _logger;

    public MovieService(IMovieRepository movieRepository, ILogger<MovieService> logger)
    {
        _movieRepository = movieRepository;
        _logger = logger;
    }

    public async Task<List<MovieDto>> GetMovies(string title = "", string genre = "")
    {
        var movies = new List<MovieDto>();

        var movieEntities = await _movieRepository.GetMovies(title, genre);
        _logger.LogInformation("{Count} movies retrieved", movieEntities.Count);

        foreach (var movieEntity in movieEntities)
        {
            movies.Add(
                new MovieDto
                {
                    Id = movieEntity.Id,
                    Title = movieEntity.Title,
                    ReleaseDate = movieEntity.ReleaseDate,
                    Genre = movieEntity.Genre,
                    Runtime = movieEntity.Runtime,
                    Synopsis = movieEntity.Synopsis,
                    Director = movieEntity.Director,
                    Rating = movieEntity.Rating,
                    AvgScore = CalculateAvgScore(movieEntity.MovieReviews.ToList()),
                }
            );
        }

        return movies;
    }

    public async Task<MovieDetailDto?> GetMovieDetail(int id)
    {
        var movie = await _movieRepository.GetMovie(id);

        if (movie == null)
        {
            _logger.LogInformation("No movie found for id {id}", id);
            return null;
        }

        return new MovieDetailDto
        {
            Id = movie.Id,
            Title = movie.Title,
            ReleaseDate = movie.ReleaseDate,
            Genre = movie.Genre,
            Runtime = movie.Runtime,
            Synopsis = movie.Synopsis,
            Director = movie.Director,
            Rating = movie.Rating,
            AvgScore = CalculateAvgScore(movie.MovieReviews.ToList()),
            Cinemas = movie
                .MovieCinemas.Select(mc => new CinemaDto
                {
                    Name = mc.Cinema.Name,
                    Showtime = mc.Showtime,
                    TicketPrice = mc.TicketPrice
                }).ToList()
        };
    }

    private decimal CalculateAvgScore(List<MovieReview> reviews)
    {
        if (!reviews.Any())
            return 0;

        return reviews.Average(r => r.Score);
    }
}
