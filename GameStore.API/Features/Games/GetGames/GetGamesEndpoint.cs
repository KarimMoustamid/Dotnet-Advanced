namespace GameStore.API.Features.Games.GetGames
{
    using Data;
    using Dtos;

    public static class GetGamesEndpoint
    {
        public static void MapGetGames(this IEndpointRouteBuilder app)
        {
            app.MapGet("/games", (GameStoreData store) => store.GetAllGames()
                .Select(game => new GetGamesDto(
                    game.Id,
                    game.Name,
                    game.Genre?.Name ?? string.Empty,
                    game.Price,
                    game.ReleaseDate
                )));
        }
    }
}