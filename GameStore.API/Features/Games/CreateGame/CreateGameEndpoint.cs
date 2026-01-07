namespace GameStore.API.Features.Games.CreateGame
{
    using Data;
    using Features.Games.Constants;
    using Models;
    using Dtos;
    using Microsoft.AspNetCore.Mvc;
    using shared.FileUpload;

    public static class CreateGameEndpoint
    {
        /// <summary>
        /// Service-level default placeholder image URI used by the CreateGame service when no image is provided by the client.
        /// This should point to a small, public placeholder image appropriate for listings.
        /// The constant represents the CreateGame service's default and is used when an image is not supplied.
        /// </summary>
        private const string DefaultImageUri = "https://placehold.co/100";

        /// <summary>
        /// Maps the POST / endpoint to create a new game. Validates input, optionally uploads an image,
        /// saves the game to the database, and returns a 201 Created response with the created game details.
        /// </summary>
        /// <param name="app">The endpoint route builder to register the route on.</param>

        /// <remarks>
        /// The request handler (lambda) receives the following parameters:
        /// <list type="bullet">
        /// <item>
        /// <description><c>GameStoreContext dbContext</c> — Entity Framework Core <c>DbContext</c> for accessing and persisting games and genres. This is a scoped service injected by the route handler.</description>
        /// </item>
        /// <item>
        /// <description><c>ILogger&lt;Program&gt; logger</c> — Application logger used to record informational messages and warnings during request handling.</description>
        /// </item>
        /// <item>
        /// <description><c>[FromForm] CreateGameDto gameDto</c> — The request payload bound from the multipart/form-data form. Contains fields required to create a game and an optional <c>IFormFile ImageFile</c> for image upload.</description>
        /// </item>
        /// <item>
        /// <description><c>FileUploader fileUploader</c> — Service that handles saving uploaded files to the configured web root and returning a public URL. Used when <c>gameDto.ImageFile</c> is provided.</description>
        /// </item>
        /// </list>
        /// </remarks>
        public static void MapCreateGame(this IEndpointRouteBuilder app)
        {
            // POST / — Create a new game: validate input, optionally upload image, save to DB, return 201 Created
            app.MapPost("/",
                async (GameStoreContext dbContext, ILogger<Program> logger, [FromForm] CreateGameDto gameDto, FileUploader fileUploader) =>
                {
                    var imageUri = DefaultImageUri;

                    // Validate GenreId was provided
                    if (gameDto.GenreId == Guid.Empty)
                    {
                        return Results.BadRequest(new { error = "GenreId is required." });
                    }

                    // Ensure the Genre exists to avoid foreign-key constraint failures
                    var genre = dbContext.Genres.Find(gameDto.GenreId);
                    if (genre is null)
                    {
                        return Results.BadRequest(new { error = "Genre not found." });
                    }

                    // If an image file was provided, attempt to upload it and use its URL
                    if (gameDto.ImageFile is not null)
                    {
                        var uploadResult = await fileUploader.UploadFileAsync(gameDto.ImageFile, StorageNames.GameImagesFolder);

                        if (uploadResult.IsSuccess && !string.IsNullOrEmpty(uploadResult.FileUrl))
                        {
                            // Assign the publicly-accessible URL returned by the uploader.
                            // The uploader validated and persisted the file; because we checked IsSuccess and non-empty FileUrl,
                            // it's safe to store this value and return it to clients (it's expected to be an absolute URL).
                            imageUri = uploadResult.FileUrl;
                        }
                        else
                        {
                            logger.LogWarning("Image upload failed: {Error}", uploadResult.ErrorMessage);
                            // Fall back to default image URI
                            return Results.BadRequest(new { message = uploadResult.ErrorMessage });
                        }
                    }

                    var game = new Game()
                    {
                        Name = gameDto.Name,
                        GenreId = gameDto.GenreId,
                        Genre = genre,
                        Price = gameDto.Price,
                        ReleaseDate = gameDto.ReleaseDate,
                        Description = gameDto.Description,
                        ImageUri = imageUri
                    };

                    dbContext.Games.Add(game);
                    await dbContext.SaveChangesAsync();

                    logger.LogInformation("Game created: {GameName} with price {GamePrice}", game.Name, game.Price);


                    return Results.CreatedAtRoute(EndpointNames.GetGame, new { id = game.Id }, new GameDetailsDto(
                        game.Id, game.Name, game.GenreId, game.Price, game.ReleaseDate, game.Description, game.ImageUri));
                }).WithParameterValidation();
        }
    }
}