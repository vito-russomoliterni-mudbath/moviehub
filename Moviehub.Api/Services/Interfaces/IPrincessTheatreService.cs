using Moviehub.Api.Dtos;

namespace Moviehub.Api.Services.Interfaces;

public interface IPrincessTheatreService
{
    Task<PrincessTheatreResponseDto?> GetPrincessTheatreMovies(string provider);
}
