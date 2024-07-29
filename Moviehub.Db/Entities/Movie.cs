using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviehub.Db.Entities;

public class Movie
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("title", TypeName = "varchar(128)")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Column("releaseDate", TypeName = "date")]
    public DateTime ReleaseDate { get; set; }

    [Required]
    [Column("genre", TypeName = "varchar(64)")]
    public string Genre { get; set; } = string.Empty;

    [Required]
    [Column("runtime")]
    public int Runtime { get; set; }

    [Required]
    [Column("synopsis", TypeName = "text")]
    public string Synopsis { get; set; } = string.Empty;

    [Required]
    [Column("director", TypeName = "varchar(64)")]
    public string Director { get; set; } = string.Empty;

    [Required]
    [Column("rating", TypeName = "varchar(8)")]
    public string Rating { get; set; } = string.Empty;

    [Required]
    [Column("princessTheatreMovieId", TypeName = "varchar(16)")]
    public string PrincessTheatreMovieId { get; set; } = string.Empty;

    public virtual List<MovieCinema> MovieCinemas { get; set; } = new();
}
