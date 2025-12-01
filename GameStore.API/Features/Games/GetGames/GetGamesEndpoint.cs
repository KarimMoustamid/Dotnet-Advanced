namespace GameStore.API.Dtos
{
    using Data;

    public static class GetGamesEndpoint
    {
        public static void MapGetGames(this IEndpointRouteBuilder app, GameStoreData data)
        {
            app.MapGet("/games", (GameStoreData data) => data.GetAllGames()
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