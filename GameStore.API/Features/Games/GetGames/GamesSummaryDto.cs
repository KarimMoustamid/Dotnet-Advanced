namespace GameStore.API.Features.Games.GetGames
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Parameters used to request a paged list of games.
    /// </summary>
    /// <param name="PageNumber">1-based page number. Defaults to 1.</param>
    /// <param name="PageSize">Number of items per page. Defaults to 5.</param>
    /// <param name="SearchTerm">Optional search term to filter games by name or description.</param>
    public record GetGamesDto(int PageNumber = 1, int PageSize = 5, string? SearchTerm = null);

    /// <summary>
    /// A single page of games returned by the API.
    /// </summary>
    /// <param name="TotalPages">Total number of pages available for the current query.</param>
    /// <param name="Games">The list of game summaries on the current page.</param>
    public record GamesPageDto(int TotalPages, List<GamesSummaryDto> Games);


    /// <summary>
    /// Summary information for a game shown in list results.
    /// </summary>
    /// <param name="Id">Unique identifier of the game.</param>
    /// <param name="Name">The game's display name.</param>
    /// <param name="Genre">The game's genre name.</param>
    /// <param name="Price">The game's price.</param>
    /// <param name="ReleaseDate">The game's release date.</param>
    /// <param name="ImageUri">Public URI pointing to the game's image (if available).</param>
    public record GamesSummaryDto(
        Guid Id,
        string Name,
        string Genre,
        decimal Price,
        DateOnly ReleaseDate,
        string ImageUri
    );
}