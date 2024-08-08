using Microsoft.AspNetCore.Mvc;
using Moviehub.Api.Dtos;
using Moviehub.Api.Services.Interfaces;

namespace Moviehub.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovieReviewController : ControllerBase
{
    private readonly IMovieReviewService _movieReviewService;
    private readonly ILogger<MovieReviewController> _logger;

    public MovieReviewController(IMovieReviewService movieReviewService, ILogger<MovieReviewController> logger)
    {
        _movieReviewService = movieReviewService;
        _logger = logger;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> AddMovieReview([FromBody] MovieReviewAddDto review)
    {
        try
        {
            await _movieReviewService.AddMovieReview(review);
            return StatusCode(StatusCodes.Status201Created);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "An error occurred while adding a movie review.");
            return StatusCode(StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while adding a movie review.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding a movie review.");
        }
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<List<MovieReviewDto>>> GetMovieReviews([FromQuery] int movieId)
    {
        try
        {
            var reviews = await _movieReviewService.GetMovieReviewsByMovieId(movieId);

            if (!reviews.Any())
                return NotFound();

            return Ok(reviews);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while retrieving movie reviews.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while retrieving movie reviews.");
        }
    }

    [HttpPatch]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> UpdateMovieReview([FromBody] MovieReviewUpdateDto review)
    {
        try
        {
            await _movieReviewService.UpdateMovieReview(review);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "An error occurred while deleting a movie review.");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while updating a movie review.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while updating a movie review.");
        }
    }

    [HttpDelete("{reviewId}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteMovieReview(int reviewId)
    {
        try
        {
            await _movieReviewService.DeleteMovieReview(reviewId);
            return Ok();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "An error occurred while deleting a movie review.");
            return NotFound();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while deleting a movie review.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while deleting a movie review.");
        }
    }
}
