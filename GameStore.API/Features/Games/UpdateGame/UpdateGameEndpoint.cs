namespace GameStore.API.Features.Games.UpdateGame
{
    using Data;
    using Dtos;
    using Models;

    public static class UpdateGameEndpoint
    {
        public static void MapUpdateGame(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{id}",
                async (Guid id, GameStoreContext dbContext, UpdateGameDto gameDto) =>
                {
                    Game? existingGame = await dbContext.Games.FindAsync(id);
                    if (existingGame is null)
                    {
                        return Results.NotFound("Game not found");
                    }

                    existingGame.Name = gameDto.Name;
                    existingGame.GenreId = gameDto.GenreId;
                    existingGame.Price = gameDto.Price;
                    existingGame.ReleaseDate = gameDto.ReleaseDate;
                    existingGame.Description = gameDto.Description;

                    await dbContext.SaveChangesAsync();

                    return Results.NoContent();
                }).WithParameterValidation();
        }
    }
}