using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Moviehub.Data.Database.Entities;

[Table("MovieReview")]
public class MovieReview
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("movieId")]
    public int MovieId { get; set; }
    
    [Required]
    [Column("score", TypeName = "decimal")]
    public decimal Score { get; set; }

    [Required]
    [Column("comment", TypeName = "text")]
    public string Comment { get; set; } = string.Empty;

    [Required]
    [Column("reviewDate", TypeName = "date")]
    public DateTime ReviewDate { get; set; }

    public virtual Movie Movie { get; set; } = new();
}
