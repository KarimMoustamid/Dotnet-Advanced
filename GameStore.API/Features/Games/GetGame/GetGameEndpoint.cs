namespace GameStore.API.Features.Games.GetGame
{
    using System.Diagnostics;
    using Data;
    using Dtos;
    using Features.Games.Constants;
    using Microsoft.Data.Sqlite;
    using Models;

    public static class GetGameEndpoint
    {
        public static void MapGetGame(this IEndpointRouteBuilder app)
        {
            app.MapGet("/{id}",
                async (Guid id, GameStoreContext dbContext, ILogger<Program> logger) =>
                {
                    try
                    {
                        Game? game = await FindGameAsync(dbContext, id);

                        return game is null ? Results.NotFound() : Results.Ok(new GetGameDto(
                            game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate, game.Description));
                    }
                    catch (Exception ex)
                    {
                        var traceId = Activity.Current?.TraceId;
                        logger.LogError(ex, "Error getting game with id {Id} . Machine {} , trace id {TraceId}", id, Environment.MachineName , traceId);
                        return Results.Problem(
                            title: "An error occurred while processing your request.",
                            statusCode: StatusCodes.Status500InternalServerError,
                            extensions: new Dictionary<string, object?> { { "traceId", traceId.ToString() } }
                            );
                    }
                }).WithName(EndpointNames.GetGame);
        }

        private async static Task<Game?> FindGameAsync(GameStoreContext dbContext, Guid id)
        {
            throw new SqliteException("The database is not available.", 14);
            Game? game = await dbContext.Games.FindAsync(id);
            return game;
        }
    }
}