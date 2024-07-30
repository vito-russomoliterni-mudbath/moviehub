using Microsoft.EntityFrameworkCore;
using Moviehub.Data.Database.Entities;

namespace Moviehub.Data.Database;

public class MoviehubDbContext : DbContext
{
    public MoviehubDbContext(DbContextOptions<MoviehubDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movie { get; set; }
    public DbSet<MovieCinema> MovieCinema { get; set; }
    public DbSet<Cinema> Cinema { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<MovieCinema>()
            .HasOne(mc => mc.Movie)
            .WithMany(m => m.MovieCinemas)
            .HasForeignKey(mc => mc.MovieId);
        modelBuilder.Entity<MovieCinema>()
            .HasOne(mc => mc.Cinema)
            .WithMany(c => c.MovieCinemas)
            .HasForeignKey(mc => mc.CinemaId);
    }
}
