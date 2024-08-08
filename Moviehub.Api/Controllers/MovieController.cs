using Microsoft.AspNetCore.Mvc;
using Moviehub.Api.Dtos;
using Moviehub.Api.Services.Interfaces;

namespace Moviehub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService _movieService;
    private readonly ILogger<MovieController> _logger;

    public MovieController(IMovieService movieService, ILogger<MovieController> logger)
    {
        _movieService = movieService;
        _logger = logger;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<MovieDto>>> GetMovies([FromQuery] string title = "", [FromQuery] string genre = "")
    {
        try
        {
            var movies = await _movieService.GetMovies(title, genre);

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
    public async Task<ActionResult<MovieDetailDto>> GetMovieDetails(int id)
    {
        try
        {
            var movie = await _movieService.GetMovieDetail(id);

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
