using Microsoft.EntityFrameworkCore;
using Moviehub.Data.Database;
using Moviehub.Data.Database.Entities;

namespace Moviehub.Api.Tests;

public static class TestDbHelper
{
    public static async Task SeedData(MoviehubDbContext db)
    {
        var movie = new Movie
        {
            Title = "The Shawshank Redemption",
            ReleaseDate = new DateTime(1994, 10, 14),
            Genre = "Drama",
            Runtime = 142,
            Synopsis = "",
            Director = "Frank Darabont",
            Rating = "R"
        };
    
        var movieReview1 = new MovieReview
        {
            Movie = movie,
            Score = 4.5m,
            Comment = "A classic movie that will stand the test of time.",
            ReviewDate = new DateTime(1994, 10, 14)
        };
    
        var movieReview2 = new MovieReview
        {
            Movie = movie,
            Score = 4m,
            Comment = "Neat movie.",
            ReviewDate = new DateTime(2024, 4, 14)
        };
    
        var cinema = new Cinema
        {
            Name = "Cinema 1",
            Location = "Location 1"
        };
    
        var movieCinema = new MovieCinema
        {
            Movie = movie,
            Cinema = cinema,
            Showtime = new DateTime(2024, 4, 14, 12, 0, 0),
            TicketPrice = 10
        };

        db.Movies.Add(movie);
        movie.MovieReviews.Add(movieReview1);
        movie.MovieReviews.Add(movieReview2);
        db.Cinemas.Add(cinema);
        db.MovieCinemas.Add(movieCinema);
    
        await db.SaveChangesAsync();
    }
}
