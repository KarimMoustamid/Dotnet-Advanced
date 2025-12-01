namespace GameStore.API.Features.Genres.GetGenres
{
    using Data;
    using Dtos;
    using System.Linq;

    public static class GetGenresEndpoint
    {
        public static void MapGetGenres(this IEndpointRouteBuilder app)
        {
            app.MapGet("/genres", (GameStoreData data) =>
                data.GetAllGenres().Select(g => new GenreDto(g.Id, g.Name)));
        }
    }
}