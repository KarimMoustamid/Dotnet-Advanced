namespace GameStore.API.Features.Games.GetGames
{
    using Data;
    using Dtos;
    using Microsoft.EntityFrameworkCore;

    public static class GetGamesEndpoint
    {
        public static void MapGetGames(this IEndpointRouteBuilder app)
        {
            app.MapGet("/",
                async (GameStoreContext dbContext, [AsParameters] GetGamesDto request) =>
                {
                    var skipCount = (request.PageNumber - 1) * request.PageSize;

                    await dbContext.Games
                        .OrderBy(game => game.Name)
                        .Skip(skipCount)
                        .Take(request.PageSize)
                        .Include(game => game.Genre)
                        .Select(game => new GamesSummaryDto(
                            game.Id,
                            game.Name,
                            game.Genre!.Name,
                            game.Price,
                            game.ReleaseDate
                        )).AsNoTracking()
                        .ToListAsync();
                });
        }
        }
    }