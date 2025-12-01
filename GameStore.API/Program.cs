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

#region DataModels
List<Genre> genres = new List<Genre>
{
    new Genre { Id = Guid.NewGuid(), Name = "RPG" },
    new Genre { Id = Guid.NewGuid(), Name = "Action RPG" },
    new Genre { Id = Guid.NewGuid(), Name = "Roguelike" }
};

List<Game> games = new List<Game>
{
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "The Witcher 3: Wild Hunt",
        Genre = genres[0],
        Price = 39.99M,
        Description = "Open-world action role-playing game set in a dark fantasy world.",
        ReleaseDate = new DateOnly(2015, 5, 19)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Cyberpunk 2077",
        Genre = genres[1],
        Price = 59.99M,
        Description = "Futuristic open-world action RPG set in Night City.",
        ReleaseDate = new DateOnly(2020, 12, 10)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Hades",
        Genre = genres[2],
        Price = 24.99M,
        Description = "Roguelike dungeon crawler where players fight through the underworld.",
        ReleaseDate = new DateOnly(2020, 9, 17)
    }
};

#endregion

// ---------- CONTROLLERS ----------

#region GameController
app.MapGet("/", () => "Hello World!");
app.MapGet("/games", () => games.Select(game => new GameSummaryDto(
    game.Id,
    game.Name,
    game.Genre?.Name ?? string.Empty,
    game.Price,
    game.ReleaseDate
)));

app.MapGet("/games/{id}",
    (Guid id) =>
    {
        Game? game = games.Find(g => g.Id == id);
        return game is null ? Results.NotFound() : Results.Ok(new GameDetailsDto(
            game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
    }).WithName(GetGameEndpointName);

app.MapPost("/games",
    (CreateGameDto gameDto) =>
    {
        var genre = genres.Find(g => g.Id == gameDto.GenreId);
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
        games.Add(game);
        return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, new GameDetailsDto(
            game.Id, game.Name, game.Genre.Id, game.Price, game.ReleaseDate, game.Description));
    }).WithParameterValidation();

app.MapPut("/games/{id}",
    (Guid id, Game game) =>
    {
        Game? existingGame = games.Find( g => g.Id == id);
        if (existingGame is null)
        {
           return Results.NotFound("Game not found");
        }

        existingGame.Name = game.Name;
        existingGame.Genre = game.Genre;
        existingGame.Price = game.Price;
        existingGame.ReleaseDate = game.ReleaseDate;
        existingGame.Description = game.Description;

        return Results.NoContent();
    }).WithParameterValidation();

app.MapDelete("/games/{id}",
    (Guid id) =>
    {
        games.RemoveAll(g => g.Id == id);
    });

#endregion

#region GemreController

app.MapGet("/genres", () => genres.Select(g => new GenreDto(g.Id, g.Name)));

#endregion

app.Run();