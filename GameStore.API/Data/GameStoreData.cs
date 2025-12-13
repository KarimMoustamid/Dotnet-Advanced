namespace GameStore.API.Data
{
    using Models;

    public class GameStoreData
    {
        // _lock is a dedicated synchronization object. We use it with the lock statement to ensure only one thread executes the protected block at a time.
        private readonly object _lock = new();
        private readonly List<Genre> _genres = new()
        {
            new Genre { Id = Guid.NewGuid(), Name = "RPG" },
            new Genre { Id = Guid.NewGuid(), Name = "Action RPG" },
            new Genre { Id = Guid.NewGuid(), Name = "Roguelike" }
        };

        private readonly List<Game> _games;

        public GameStoreData()
        {
            _games = new List<Game>
            {
                new Game { Id = Guid.NewGuid(), Name = "The Witcher 3: Wild Hunt", Genre = _genres[0], GenreId = _genres[0].Id, Price = 39.99M, Description = "...", ReleaseDate = new DateOnly(2015,5,19) },
                new Game { Id = Guid.NewGuid(), Name = "Cyberpunk 2077", Genre = _genres[1], GenreId = _genres[1].Id, Price = 59.99M, Description = "...", ReleaseDate = new DateOnly(2020,12,10) },
                new Game { Id = Guid.NewGuid(), Name = "Hades", Genre = _genres[2], GenreId = _genres[2].Id, Price = 24.99M, Description = "...", ReleaseDate = new DateOnly(2020,9,17) }
            };
        }

        // Readers return copies to avoid concurrent-enumeration problems.
        public IReadOnlyList<Game> GetAllGames()
        {
            lock (_lock)
            {
                return _games.ToList();
            }
        }

        public Game? GetGameById(Guid id)
        {
            lock (_lock)
            {
                return _games.Find(g => g.Id == id);
            }
        }

        public void AddGame(Game game)
        {
            lock (_lock)
            {
                game.Id = Guid.NewGuid();
                _games.Add(game);
            }
        }

        public bool RemoveGame(Guid id)
        {
            lock (_lock)
            {
                int removed = _games.RemoveAll(g => g.Id == id);
                return removed > 0;
            }
        }

        public IReadOnlyList<Genre> GetAllGenres()
        {
            lock (_lock)
            {
                return _genres.ToList();
            }
        }

        public Genre? GetGenreById(Guid id)
        {
            lock (_lock)
            {
                return _genres.Find(g => g.Id == id);
            }
        }
    }
}