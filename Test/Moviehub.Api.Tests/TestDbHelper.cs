using Microsoft.EntityFrameworkCore;
using Moviehub.Data.Database;
using Moviehub.Data.Database.Entities;

namespace Moviehub.Api.Tests;

public static class TestDbHelper
{
    public static async Task SeedData(MoviehubDbContext db)
    {
        db.MovieCinemas.Add(MovieCinema);
        await db.SaveChangesAsync();
    }

    public static async Task RemoveData(MoviehubDbContext db)
    {
        var tables = new[] { "MovieCinema", "Movie", "Cinema", "MovieReview" }; 
        
        foreach (var table in tables)
        {
            var tableExists = await db.Database.ExecuteSqlAsync(
                $"SELECT name FROM sqlite_master WHERE type='table' AND name='{table}';");

            if (tableExists > 0)
            {
                await db.Database.ExecuteSqlAsync($"DELETE FROM \"{table}\"");
            }
        }
    }

    private static Movie Movie => new Movie
        {
            Title = "The Shawshank Redemption",
            ReleaseDate = new DateTime(1994, 10, 14),
            Genre = "Drama",
            Runtime = 142,
            Synopsis = "",
            Director = "Frank Darabont",
            Rating = "R",
            MovieReviews = new List<MovieReview>
                {
                    new MovieReview
                    {
                        MovieId = 1,
                        Score = 4.5m,
                        Comment = "A classic movie that will stand the test of time.",
                        ReviewDate = new DateTime(1994, 10, 14)
                    },
                    new MovieReview
                    {
                        MovieId = 1,
                        Score = 4m,
                        Comment = "Neat movie.",
                        ReviewDate = new DateTime(2024, 4, 14)
                    }
                }
        };

    private static Cinema Cinema => new Cinema
        {
            Name = "Cinema 1",
            Location = "Location 1"
        };

    private static MovieCinema MovieCinema => new MovieCinema
        {
            Movie = Movie,
            Cinema = Cinema,
            Showtime = new DateTime(2024, 4, 14, 12, 0, 0),
            TicketPrice = 10
        };
}
