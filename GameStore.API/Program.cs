using GameStore.API.Models;
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

List<Game> games = new List<Game>
{
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "The Witcher 3: Wild Hunt",
        Genre = new Genre { Id = Guid.NewGuid(), Name = "RPG" },
        Price = 39.99M,
        Description = "Open-world action role-playing game set in a dark fantasy world.",
        ReleaseDate = new DateOnly(2015, 5, 19)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Cyberpunk 2077",
        Genre = new Genre { Id = Guid.NewGuid(), Name = "Action RPG" },
        Price = 59.99M,
        Description = "Futuristic open-world action RPG set in Night City.",
        ReleaseDate = new DateOnly(2020, 12, 10)
    },
    new Game
    {
        Id = Guid.NewGuid(),
        Name = "Hades",
        Genre = new Genre { Id = Guid.NewGuid(), Name = "Roguelike" },
        Price = 24.99M,
        Description = "Roguelike dungeon crawler where players fight through the underworld.",
        ReleaseDate = new DateOnly(2020, 9, 17)
    }
};

app.MapGet("/", () => "Hello World!");
app.MapGet("/games", () => games);
app.MapGet("/games/{id}",
    (Guid id) =>
    {
        Game? game = games.Find(g => g.Id == id);
        return game is null ? Results.NotFound() : Results.Ok(game);
    }).WithName(GetGameEndpointName);

app.MapPost("/games",
    (Game game) =>
    {
       // if (string.IsNullOrWhiteSpace(game.Name))
       // {
       //     return Results.BadRequest("Name is required");
       // }

        game.Id = Guid.NewGuid();
        games.Add(game);
        return Results.CreatedAtRoute(GetGameEndpointName, new { id = game.Id}, game);
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


app.Run();