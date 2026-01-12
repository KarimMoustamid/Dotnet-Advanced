namespace GameStore.API.Features.Games.CreateGame
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Data required to create a new game.
    /// </summary>
    /// <param name="Name">The game's name. Required; maximum length 50.</param>
    /// <param name="GenreId">Identifier of the game's genre.</param>
    /// <param name="Price">The game's price. Must be between 1 and 100.</param>
    /// <param name="ReleaseDate">The game's release date.</param>
    /// <param name="Description">The game's description. Required; maximum length 500.</param>
    public record CreateGameDto(
        [Required] [StringLength(50)] string Name,
        Guid GenreId,
        [Range(1, 100)] decimal Price,
        DateOnly ReleaseDate,
        [Required] [StringLength(500)] string Description)
    {
        /// <summary>
        /// Optional image file uploaded for the game.
        /// Use streaming APIs (OpenReadStream / CopyToAsync) for large uploads; always validate size and content type and sanitize filenames.
        /// </summary>
        public IFormFile? ImageFile { get; set; }
    }

    /// <summary>
    /// DTO returned by endpoints with full game details.
    /// </summary>
    public record GameDetailsDto(
        Guid Id,
        string Name,
        Guid GenreId,
        decimal Price,
        DateOnly ReleaseDate,
        string Description,
        string ImageUri
    );
}