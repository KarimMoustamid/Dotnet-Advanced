namespace GameStore.API.Dtos;

public record GetGamesDto(int PageNumber = 1, int PageSize = 5, string? SearchTerm = null);

public record GamesPageDto(int TotalPages, List<GamesSummaryDto> Games);


public record GamesSummaryDto(
    Guid Id,
    string Name,
    string Genre,
    decimal Price,
    DateOnly ReleaseDate
    );