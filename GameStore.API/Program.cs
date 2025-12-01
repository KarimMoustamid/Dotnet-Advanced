using GameStore.API.Data;
using GameStore.API.Models;
using GameStore.API.Dtos;
using GameStore.API.Features.Games.CreateGame;
using GameStore.API.Features.Games.GetGame;
using GameStore.API.Features.Games.GetGames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddSingleton<GameStoreData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ---------- CONTROLLERS ----------

#region GameController
app.MapGet("/", () => "Hello World!");
app.MapGetGames();
app.MapGetGame();
app.MapCreateGame();

app.MapPut("/games/{id}",
    (Guid id, GameStoreData data, UpdateGameDto gameDto) =>
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
    (Guid id, GameStoreData data) =>
    {
        data.RemoveGame(id);
        return Results.NoContent();
    });

#endregion

#region GemreController

app.MapGet("/genres", (GameStoreData data) => data.GetAllGenres().Select(g => new GenreDto(g.Id, g.Name)));

#endregion

app.Run();