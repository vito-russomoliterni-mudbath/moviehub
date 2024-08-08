namespace Moviehub.Api.Dtos;

public class CinemaDto
{
    public string Name { get; set; } = string.Empty;
    public decimal TicketPrice { get; set; }
    public DateTime Showtime { get; set; }
}
