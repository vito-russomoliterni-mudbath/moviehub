namespace Moviehub.Data.Repositories.Models;

public class MovieReviewBase
{
    public decimal Score { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
}

public class AddMovieReview : MovieReviewBase
{
    public int MovieId { get; set; }
}

public class UpdateMovieReview : MovieReviewBase
{
    public int Id { get; set; }
}

public class MovieReview : MovieReviewBase
{
    public int Id { get; set; }
    public int MovieId { get; set; }
}