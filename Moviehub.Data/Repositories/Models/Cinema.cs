namespace Moviehub.Data.Repositories.Models;

public class Cinema
{
    public string Name { get; set; } = string.Empty;
    public decimal TicketPrice { get; set; }
    public DateTime Showtime { get; set; }
}
