namespace GameStore.API.Features.Games.UpdateGame
{
    using System;
    using Data;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using GameStore.API.Features.Games.Constants;
    using shared.FileUpload;

    public static class UpdateGameEndpoint
    {
        public static void MapUpdateGame(this IEndpointRouteBuilder app)
        {
            app.MapPut("/{id}",
                async (Guid id, GameStoreContext dbContext, ILogger<Program> logger, [FromForm] UpdateGameDto gameDto, FileUploader fileUploader) =>
                {
                    Game? existingGame = await dbContext.Games.FindAsync(id);
                    if (existingGame is null)
                    {
                        return Results.NotFound("Game not found");
                    }

                    existingGame.Name = gameDto.Name;
                    existingGame.GenreId = gameDto.GenreId;
                    existingGame.Price = gameDto.Price;
                    existingGame.ReleaseDate = gameDto.ReleaseDate;
                    existingGame.Description = gameDto.Description;

                    // If an image was provided, attempt upload but do not block the update on failure.
                    if (gameDto.ImageFile is not null)
                    {
                        var fileUploadResult = await fileUploader.UploadFileAsync(
                            gameDto.ImageFile,
                            StorageNames.GameImagesFolder
                        );

                        // If upload succeeded and returned a non-empty, absolute URL, use it.
                        if (fileUploadResult.IsSuccess && !string.IsNullOrWhiteSpace(fileUploadResult.FileUrl) && Uri.TryCreate(fileUploadResult.FileUrl, UriKind.Absolute, out var parsedUri))
                        {
                            existingGame.ImageUri = parsedUri.ToString();
                        }
                        else
                        {
                            // Log details for troubleshooting, but continue the update using the existing ImageUri.
                            logger.LogWarning("Image upload skipped/failed for game update (id={GameId}). Success={Success}, FileUrl='{FileUrl}', Error={Error}",
                                existingGame.Id,
                                fileUploadResult.IsSuccess,
                                fileUploadResult.FileUrl,
                                fileUploadResult.ErrorMessage);

                            // Optionally: consider returning a warning in the response body or emitting a specific event.
                            // For now we prefer not to block the update if the image can't be stored.
                        }
                    }

                    await dbContext.SaveChangesAsync();

                    return Results.NoContent();
                }).WithParameterValidation();
        }
    }
}