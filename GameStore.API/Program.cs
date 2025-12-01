using GameStore.API.Data;
using GameStore.API.Models;
using GameStore.API.Dtos;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

const string GetGameEndpointName = "GetGame";

// ---------- DATA ----------

GameStoreData data = new();

// ---------- CONTROLLERS ----------

#region GameController
app.MapGet("/", () => "Hello World!");
app.MapGet("/games", () => data.GetAllGames()
    .Select(game => new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre?.Name ?? string.Empty,
    game.Price,
    game.ReleaseDate
)));

app.MapGet("/games/{id}",
    (Guid id) =>
    {
        Game? game = data.GetGameById(id);
        return game is null ? Results.NotFound() : Results.Ok(new GameDetailsDto(
            game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
    }).WithName(GetGameEndpointName);

app.MapPost("/games",
    (CreateGameDto gameDto) =>
    {
        var genre = data.GetGenreById(gameDto.GenreId);
        if (genre is null) return Results.BadRequest("Invalid genre");

        var game = new Game()
        {
            Name = gameDto.Name,
            Genre = genre,
            Price = gameDto.Price,
            ReleaseDate = gameDto.ReleaseDate,
            Description = gameDto.Description
        };

        game.Id = Guid.NewGuid();
        data.AddGame(game);

        return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, new GameDetailsDto(
            game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
    }).WithParameterValidation();

app.MapPut("/games/{id}",
    (Guid id, UpdateGameDto gameDto) =>
    {
        var genre = data.GetGenreById(gameDto.GenreId);
        if (genre is null) return Results.BadRequest("Invalid genre");

        Game? existingGame = data.GetGameById(id);
        if (existingGame is null)
        {
           return Results.NotFound("Game not found");
        }

        existingGame.Name = gameDto.Name;
        existingGame.Genre = genre;
        existingGame.Price = gameDto.Price;
        existingGame.ReleaseDate = gameDto.ReleaseDate;
        existingGame.Description = gameDto.Description;

        return Results.NoContent();
    }).WithParameterValidation();

app.MapDelete("/games/{id}",
    (Guid id) =>
    {
        data.RemoveGame(id);
    });

#endregion

#region GemreController

app.MapGet("/genres", () => data.GetAllGenres().Select(g => new GenreDto(g.Id, g.Name)));

#endregion

app.Run();