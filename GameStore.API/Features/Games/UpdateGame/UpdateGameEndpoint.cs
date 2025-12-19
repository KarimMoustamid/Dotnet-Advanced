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
                (Guid id, GameStoreContext dbContext, UpdateGameDto gameDto) =>
                {
                    Game? existingGame = dbContext.Games.Find(id);
                    if (existingGame is null)
                    {
                        return Results.NotFound("Game not found");
                    }

                    existingGame.Name = gameDto.Name;
                    existingGame.GenreId = gameDto.GenreId;
                    existingGame.Price = gameDto.Price;
                    existingGame.ReleaseDate = gameDto.ReleaseDate;
                    existingGame.Description = gameDto.Description;

                    dbContext.SaveChanges();

                    return Results.NoContent();
                }).WithParameterValidation();
        }
    }
}