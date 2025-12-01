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
            app.MapPost("/games",
                (GameStoreData store, CreateGameDto gameDto) =>
                {
                    var genre = store.GetGenreById(gameDto.GenreId);
                    if (genre is null) return Results.BadRequest("Invalid genre");

                    var game = new Game()
                    {
                        Name = gameDto.Name,
                        Genre = genre,
                        Price = gameDto.Price,
                        ReleaseDate = gameDto.ReleaseDate,
                        Description = gameDto.Description
                    };

                    game.Id = Guid.NewGuid();
                    store.AddGame(game);

                    return Results.CreatedAtRoute(EndpointNames.GetGame, new { id = game.Id}, new GameDetailsDto(
                        game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
                }).WithParameterValidation();
        }
    }
}