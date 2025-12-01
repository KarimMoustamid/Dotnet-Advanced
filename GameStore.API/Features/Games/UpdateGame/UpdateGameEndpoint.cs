namespace GameStore.API.Features.Games.UpdateGame
{
    using Data;
    using Dtos;
    using Models;

    public static class UpdateGameEndpoint
    {
        public static void MapUpdateGame(this IEndpointRouteBuilder app)
        {
            app.MapPut("/games/{id}",
                (Guid id, GameStoreData store, UpdateGameDto gameDto) =>
                {
                    var genre = store.GetGenreById(gameDto.GenreId);
                    if (genre is null) return Results.BadRequest("Invalid genre");

                    Game? existingGame = store.GetGameById(id);
                    if (existingGame is null)
                    {
                        return Results.NotFound("Game not found");
                    }

                    existingGame.Name = gameDto.Name;
                    existingGame.Genre = genre;
                    existingGame.Price = gameDto.Price;
                    existingGame.ReleaseDate = gameDto.ReleaseDate;
                    existingGame.Description = gameDto.Description;

                    return Results.NoContent();
                }).WithParameterValidation();
        }
    }
}