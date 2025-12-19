namespace GameStore.API.Features.Genres.GetGenres
{
    using Data;
    using Dtos;
    using System.Linq;
    using Microsoft.EntityFrameworkCore;

    public static class GetGenresEndpoint
    {
        public static void MapGetGenres(this IEndpointRouteBuilder app)
        {
            app.MapGet("/", async (GameStoreContext dbContext) =>
                await dbContext.Genres.Select(g => new GenreDto(g.Id, g.Name)).AsNoTracking().ToListAsync());
        }
    }
}