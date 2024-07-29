using Microsoft.AspNetCore.Mvc;
using Moviehub.Data.Interfaces;
using Moviehub.Data.Models;

namespace Moviehub.Api.Controllers;

[Route("api/[controller]")]
public class MovieController : Controller
{
    private readonly IMovieRepository _movieRepository;

    public MovieController(IMovieRepository movieRepository)
    {
        _movieRepository = movieRepository;
    }

    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<Movie>>> GetMovies([FromQuery] string title, [FromQuery] string genre)
    {
        var movies = await _movieRepository.GetMovies(title, genre);

        return Ok(movies);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieDetail>> GetMovieDetails(int id)
    {
        var movie = await _movieRepository.GetMovieDetail(id);

        if (movie == null)
            return NotFound();

        return Ok(movie);
    }
}
