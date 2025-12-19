namespace GameStore.API.Features.Games.GetGame
{
    using Data;
    using Dtos;
    using Features.Games.Constants;
    using Models;

    public static class GetGameEndpoint
    {
        public static void MapGetGame(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{id}",
                async (Guid id, GameStoreContext dbContext) =>
                {
                    Game? game = await dbContext.Games.FindAsync(id);

                    return game is null ? Results.NotFound() : Results.Ok(new GetGameDto(
                        game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate, game.Description));
                }).WithName(EndpointNames.GetGame);
        }
    }
}