namespace GameStore.API.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        public required string Name { get; set; }
        public required string  Genre { get; set; }
        public required decimal Price { get; set; }
        public DateOnly ReleaseDate { get; set; }
    }
}