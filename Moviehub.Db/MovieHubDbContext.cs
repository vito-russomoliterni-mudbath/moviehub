using Microsoft.EntityFrameworkCore;
using Moviehub.Db.Entities;

namespace Moviehub.Db;

public class MovieHubDbContext : DbContext
{
    public DbSet<Movie> Movie { get; set; }
    public DbSet<MovieCinema> MovieCinema { get; set; }
    public DbSet<Cinema> Cinema { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=./../db/moviehub.db");
    }

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
