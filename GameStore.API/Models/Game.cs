namespace GameStore.API.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Game
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(50)]
        public required string Name { get; set; }

        public required Genre  Genre { get; set; }

        [Range(1,100)]
        public required decimal Price { get; set; }

        public required string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}