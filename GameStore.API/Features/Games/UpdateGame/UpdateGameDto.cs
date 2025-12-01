namespace GameStore.API.Dtos
{
    using System.ComponentModel.DataAnnotations;

    public record UpdateGameDto(
        [Required] [StringLength(50)] string Name,
        Guid GenreId,
        [Range(1,100)] decimal Price,
        DateOnly ReleaseDate,
        [Required] [StringLength(500)] string Description);
}