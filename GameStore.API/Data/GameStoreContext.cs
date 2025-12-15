namespace GameStore.API.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class GameStoreContext(DbContextOptions<GameStoreContext> options) : DbContext(options)
    {
        public DbSet<Game> Games => Set<Game>();
        public DbSet<Genre> Genres => Set<Genre>();
    }
}