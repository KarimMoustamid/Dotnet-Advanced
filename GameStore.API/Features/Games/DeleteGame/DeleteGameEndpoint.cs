namespace GameStore.API.Features.Games.DeleteGame
{
    using Data;

    public static class DeleteGameEndpoint
    {
        public static void MapDeleteGame(this IEndpointRouteBuilder app)
        {
            app.MapDelete("/games/{id}", (Guid id, GameStoreData store) =>
            {
                var removed = store.RemoveGame(id);
                return removed ? Results.NoContent() : Results.NotFound();
            });
        }
    }
}