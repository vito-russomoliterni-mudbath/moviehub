using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviehub.Data.Database.Entities;

public class MovieCinema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("movieId")]
    public int MovieId { get; set; }

    [Required]
    [Column("cinemaId")]
    public int CinemaId { get; set; }

    [Required]
    [Column("showtime", TypeName = "date")]
    public DateTime Showtime { get; set; }

    [Required]
    [Column("ticketPrice", TypeName = "decimal")]
    public decimal TicketPrice { get; set; }

    public virtual Movie Movie { get; set; } = new();

    public virtual Cinema Cinema { get; set; } = new();
}
