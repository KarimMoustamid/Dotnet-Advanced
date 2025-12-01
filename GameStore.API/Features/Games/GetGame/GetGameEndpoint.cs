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
            app.MapGet("/games/{id}",
                (Guid id, GameStoreData store) =>
                {
                    Game? game = store.GetGameById(id);
                    return game is null ? Results.NotFound() : Results.Ok(new GetGameDto(
                        game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
                }).WithName(EndpointNames.GetGame);
        }
    }
}