using Moviehub.Api.Services.Interfaces;
using Moviehub.Data.Database.Entities;
using Moviehub.Api.Dtos;
using Moviehub.Data.Repositories.Interfaces;

namespace Moviehub.Api.Services;

public class MovieReviewService : IMovieReviewService
{
    private readonly IMovieReviewRepository _movieReviewRepository;
    private readonly IMovieRepository _movieRepository;
    private readonly ILogger<MovieReviewService> _logger;

    public MovieReviewService(IMovieReviewRepository movieReviewRepository,
        IMovieRepository movieRepository, ILogger<MovieReviewService> logger)
    {
        _movieReviewRepository = movieReviewRepository;
        _movieRepository = movieRepository;
        _logger = logger;
    }

    public async Task<MovieReviewDto?> GetMovieReview(int id)
    {
        var movieReview = await _movieReviewRepository.GetMovieReview(id);

        if (movieReview == null)
        {
            _logger.LogInformation("No movie review found for id {id}", id);
            return null;
        }

        return new MovieReviewDto
        {
            Id = movieReview.Id,
            MovieId = movieReview.MovieId,
            Score = movieReview.Score,
            Comment = movieReview.Comment,
            ReviewDate = movieReview.ReviewDate
        };
    }

    public async Task<List<MovieReviewDto>> GetMovieReviewsByMovieId(int movieId)
    {
        var movieReviews = await _movieReviewRepository.GetMovieReviewsByMovieId(movieId);

        if (movieReviews.Count == 0)
        {
            _logger.LogInformation("No movie reviews found for movie id {movieId}", movieId);
            return new List<MovieReviewDto>();
        }

        return movieReviews.Select(r => new MovieReviewDto
        {
            Id = r.Id,
            MovieId = r.MovieId,
            Score = r.Score,
            Comment = r.Comment,
            ReviewDate = r.ReviewDate
        }).ToList();
    }

    public async Task<MovieReviewDto> AddMovieReview(MovieReviewAddDto movieReviewAddDto)
    {
        var movie = await _movieRepository.GetMovie(movieReviewAddDto.MovieId);

        if (movie == null)
        {
            _logger.LogInformation("No movie found for id {id}", movieReviewAddDto.MovieId);
            throw new ArgumentException("Movie not found");
        }

        var movieReviewEntity = await _movieReviewRepository.AddMovieReview(new MovieReview
        {
            Movie = movie,
            Score = movieReviewAddDto.Score,
            Comment = movieReviewAddDto.Comment,
            ReviewDate = movieReviewAddDto.ReviewDate
        });

        return new MovieReviewDto
        {
            Id = movieReviewEntity.Id,
            MovieId = movieReviewEntity.MovieId,
            Score = movieReviewEntity.Score,
            Comment = movieReviewEntity.Comment,
            ReviewDate = movieReviewEntity.ReviewDate
        };
    }

    public async Task UpdateMovieReview(MovieReviewUpdateDto movieReviewUpdateDto)
    {
        var movieReview = await _movieReviewRepository.GetMovieReview(movieReviewUpdateDto.Id);

        if (movieReview == null)
        {
            _logger.LogInformation("No movie review found for id {id}", movieReviewUpdateDto.Id);
            throw new ArgumentException("Movie review not found");
        }

        movieReview.Score = movieReviewUpdateDto.Score;
        movieReview.Comment = movieReviewUpdateDto.Comment;
        movieReview.ReviewDate = movieReviewUpdateDto.ReviewDate;

        await _movieReviewRepository.UpdateMovieReview(movieReview);
    }

    public async Task DeleteMovieReview(int id)
    {
        var movieReview = await _movieReviewRepository.GetMovieReview(id);

        if (movieReview == null)
        {
            _logger.LogInformation("No movie review found for id {id}", id);
            throw new ArgumentException("Movie review not found");
        }

        await _movieReviewRepository.DeleteMovieReview(movieReview);
    }
}
