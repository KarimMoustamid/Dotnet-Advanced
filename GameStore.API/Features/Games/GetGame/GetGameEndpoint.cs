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
                (Guid id, GameStoreContext dbContext) =>
                {
                    Task<Game?> FindGameTask = dbContext.Games.FindAsync(id).AsTask();
                    return FindGameTask.ContinueWith(task =>
                    {
                        Game? game = task.Result;
                        return game is null ? Results.NotFound() : Results.Ok(new GetGameDto(
                            game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate, game.Description));
                    });
                }).WithName(EndpointNames.GetGame);
        }
    }
}