using Microsoft.AspNetCore.Mvc;
using Moviehub.Data.Repositories.Interfaces;
using Moviehub.Data.Repositories.Models;

namespace Moviehub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieRepository _movieRepository;
    private readonly ILogger<MovieController> _logger;

    public MovieController(IMovieRepository movieRepository, ILogger<MovieController> logger)
    {
        _movieRepository = movieRepository;
        _logger = logger;
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<Movie>>> GetMovies([FromQuery] string title, [FromQuery] string genre)
    {
        try
        {
            var movies = await _movieRepository.GetMovies(title, genre);

            if (!movies.Any())
                return NotFound();

            return Ok(movies);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving movies.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving movies.");
        }
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<MovieDetail>> GetMovieDetails(int id)
    {
        try
        {
            var movie = await _movieRepository.GetMovieDetail(id);

            if (movie == null)
                return NotFound();

            return Ok(movie);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving movie details.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving movie details.");
        }
    }
}
