namespace GameStore.API.Features.Games.GetGames
{
    using Data;
    using Dtos;
    using Microsoft.EntityFrameworkCore;

    public static class GetGamesEndpoint
    {
        public static void MapGetGames(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (GameStoreContext dbContext) => await dbContext.Games.Include(game => game.Genre)
                .Select(game => new GetGamesDto(
                    game.Id,
                    game.Name,
                    game.Genre!.Name,
                    game.Price,
                    game.ReleaseDate
                )).AsNoTracking().ToListAsync());
        }
    }
}