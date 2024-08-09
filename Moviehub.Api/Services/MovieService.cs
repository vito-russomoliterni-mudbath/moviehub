using Moviehub.Api.Services.Interfaces;
using Moviehub.Data.Database.Entities;
using Moviehub.Api.Dtos;
using Moviehub.Data.Repositories.Interfaces;
using Moviehub.Api.Settings;
using Microsoft.Extensions.Options;

namespace Moviehub.Api.Services;

public class MovieService : IMovieService
{
    private readonly IMovieRepository _movieRepository;
    private readonly IPrincessTheatreService _princessTheatreService;
    private readonly PrincessTheatreApiSettings _princessTheatreApiSettings;
    private readonly ILogger<MovieService> _logger;

    public MovieService(IMovieRepository movieRepository,  IOptions<PrincessTheatreApiSettings> options,
        IPrincessTheatreService princessTheatreService, ILogger<MovieService> logger)
    {
        _movieRepository = movieRepository;
        _princessTheatreApiSettings = options.Value;
        _princessTheatreService = princessTheatreService;
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

    public async Task<MovieDetailDto?> GetMovieDetails(int id)
    {
        var movie = await _movieRepository.GetMovie(id);

        if (movie == null)
        {
            _logger.LogInformation("No movie found for id {id}", id);
            return null;
        }

        var cinemas = movie
            .MovieCinemas.Select(mc => new CinemaDto
            {
                Name = mc.Cinema.Name,
                Showtime = mc.Showtime,
                TicketPrice = mc.TicketPrice
            }).ToList();

        var princessTheathreCinemas = await GetPrincessTheatreCinemas(movie.PrincessTheatreMovieId);
        cinemas.AddRange(princessTheathreCinemas);

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
            Cinemas = cinemas
        };
    }

    private decimal CalculateAvgScore(List<MovieReview> reviews)
    {
        if (!reviews.Any())
            return 0;

        return reviews.Average(r => r.Score);
    }

    private async Task<List<CinemaDto>> GetPrincessTheatreCinemas(string movieId)
    {
        var cinemas = new List<CinemaDto>();

        foreach (var provider in _princessTheatreApiSettings.MovieProviders)
        {
            try
            {
                var princessTheatreMovies = await _princessTheatreService.GetPrincessTheatreMovies(provider);

                if (princessTheatreMovies == null)
                {
                    _logger.LogWarning("No movies found for provider {provider}", provider);
                    continue;
                }

                var princessTheatreShows = princessTheatreMovies.Movies
                    .Where(m => IsMovieIdMatching(movieId, m.Id))
                    .Select(m => new CinemaDto
                    {
                        Name = princessTheatreMovies.Provider,
                        TicketPrice = m.Price,
                        Showtime = DateTime.Now
                    });

                _logger.LogInformation("{Count} shows found for provider {provider}", princessTheatreShows.Count(), provider);

                cinemas.AddRange(princessTheatreShows);
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "Error fetching movies for provider {Provider}", provider);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unexpected error occurred while fetching movies for provider {provider}", provider);
            }
        }

        return cinemas;
    }

    private bool IsMovieIdMatching(string movieId, string moviePrincessTheatreId)
        => movieId.ToLower() == moviePrincessTheatreId.Substring(2).ToLower();}
