namespace GameStore.API.Features.Games.GetGame;

/// <summary>
/// DTO returned by the API when retrieving a single game's details.
/// </summary>
/// <param name="Id">Unique identifier of the game.</param>
/// <param name="Name">The game's name.</param>
/// <param name="GenreId">Identifier of the game's genre.</param>
/// <param name="Price">The game's price.</param>
/// <param name="ReleaseDate">The game's release date.</param>
/// <param name="Description">The game's description.</param>
/// <param name="ImageUri">A public URI pointing to the game's image (if any).</param>
public record GetGameDto(
    Guid Id,
    string Name,
    Guid GenreId,
    decimal Price,
    DateOnly ReleaseDate,
    string Description,
    string ImageUri
    );