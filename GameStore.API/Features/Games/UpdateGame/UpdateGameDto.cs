namespace GameStore.API.Features.Games.UpdateGame
{
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// Data required to update an existing game.
    /// </summary>
    /// <param name="Name">The game's name. Required; maximum length 50.</param>
    /// <param name="GenreId">Identifier of the game's genre.</param>
    /// <param name="Price">The game's price. Must be between 1 and 100.</param>
    /// <param name="ReleaseDate">The game's release date.</param>
    /// <param name="Description">The game's description. Required; maximum length 500.</param>
    public record UpdateGameDto(
        [Required] [StringLength(50)] string Name,
        Guid GenreId,
        [Range(1, 100)] decimal Price,
        DateOnly ReleaseDate,
        [Required] [StringLength(500)] string Description)
    {
        /// <summary>
        /// Optional image file to replace the game's current image.
        /// Use streaming APIs (OpenReadStream / CopyToAsync) for large files; always validate size and content type and sanitize filenames.
        /// </summary>
        public IFormFile? ImageFile { get; set; }
    }
}