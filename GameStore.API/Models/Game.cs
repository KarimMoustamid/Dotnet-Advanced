namespace GameStore.API.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Game
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public  Genre?  Genre { get; set; }

        [ForeignKey("Genre")] public Guid GenreId { get; set; }
        public required decimal Price { get; set; }
        public required string Description { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}