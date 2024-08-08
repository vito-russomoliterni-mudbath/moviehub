namespace Moviehub.Api.Dtos;

public class MovieReviewBaseDto
{
    public decimal Score { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime ReviewDate { get; set; }
}

public class MovieReviewAddDto : MovieReviewBaseDto
{
    public int MovieId { get; set; }
}

public class MovieReviewUpdateDto : MovieReviewBaseDto
{
    public int Id { get; set; }
}

public class MovieReviewDto : MovieReviewBaseDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
}