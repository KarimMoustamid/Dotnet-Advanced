namespace GameStore.API.Features.Games
{
    using CreateGame;
    using DeleteGame;
    using GetGame;
    using GetGames;
    using UpdateGame;

    public static class GamesEndpoints
    {
        public static void MapGames(this IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("/games");

            group.MapGetGames();
            group.MapGetGame();
            group.MapCreateGame();
            group.MapUpdateGame();
            group.MapDeleteGame();
        }
    }
}