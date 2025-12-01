namespace GameStore.API.Dtos;

public record GetGamesDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
    );