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
                    //var filteredGames = dbContext.Games.Where(game => game.Name.Contains(request.SearchTerm, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(request.SearchTerm)); // System.InvalidOperationException

                    var filteredGames = dbContext.Games.Where(game => EF.Functions.Like(game.Name, $"%{request.SearchTerm}%" ) || string.IsNullOrWhiteSpace(request.SearchTerm)); // System.InvalidOperationException

                    var gamesOnPage = await filteredGames
                        .OrderBy(game => game.Name)
                        .Skip(skipCount)
                        .Take(request.PageSize)
                        .Include(game => game.Genre)
                        .Select(game => new GamesSummaryDto(
                            game.Id,
                            game.Name,
                            game.Genre!.Name,
                            game.Price,
                            game.ReleaseDate,
                            game.ImageUri
                        )).AsNoTracking()
                        .ToListAsync();

                    var totalGames = await filteredGames.CountAsync();
                    var totalPages = (int)Math.Ceiling(totalGames / (decimal)request.PageSize);

                    return new GamesPageDto(totalPages, gamesOnPage);
                });
        }
        }
    }