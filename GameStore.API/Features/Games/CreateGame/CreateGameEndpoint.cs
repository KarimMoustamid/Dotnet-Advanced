namespace GameStore.API.Features.Games.CreateGame
{
    using Data;
    using Features.Games.Constants;
    using Models;
    using Dtos;

    public static class CreateGameEndpoint
    {
        public static void MapCreateGame(this IEndpointRouteBuilder app)
        {
            app.MapPost("/",
                async (GameStoreContext dbContext, ILogger<Program> logger , CreateGameDto gameDto) =>
                {
                    // Validate GenreId was provided
                    if (gameDto.GenreId == Guid.Empty)
                    {
                        return Results.BadRequest(new { error = "GenreId is required." });
                    }

                    // Ensure the Genre exists to avoid foreign-key constraint failures
                    var genre = dbContext.Genres.Find(gameDto.GenreId);
                    if (genre is null)
                    {
                        return Results.BadRequest(new { error = "Genre not found." });
                    }

                    var game = new Game()
                    {
                        Name = gameDto.Name,
                        GenreId = gameDto.GenreId,
                        Genre = genre,
                        Price = gameDto.Price,
                        ReleaseDate = gameDto.ReleaseDate,
                        Description = gameDto.Description
                    };

                    dbContext.Games.Add(game);
                   await dbContext.SaveChangesAsync();

                   logger.LogInformation("Game created: {GameName} with price {GamePrice}", game.Name, game.Price);


                    return Results.CreatedAtRoute(EndpointNames.GetGame, new { id = game.Id}, new GameDetailsDto(
                        game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate, game.Description));
                }).WithParameterValidation();
        }
    }
}