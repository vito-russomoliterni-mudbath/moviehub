using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviehub.Data.Database.Entities;

public class Cinema
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("name", TypeName = "varchar(64)")]
    public string Name { get; set; } = string.Empty;

    [Required]
    [Column("location", TypeName = "text")]
    public string Location { get; set; } = string.Empty;

    public virtual List<MovieCinema> MovieCinemas { get; set; } = new();
}
