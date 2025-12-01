using GameStore.API.Data;
using GameStore.API.Models;
using GameStore.API.Dtos;
using GameStore.API.Features.Games.CreateGame;
using GameStore.API.Features.Games.GetGame;
using GameStore.API.Features.Games.GetGames;
using GameStore.API.Features.Games.UpdateGame;
using GameStore.API.Features.Games.DeleteGame;
using System.Linq;

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
app.MapUpdateGame();
app.MapDeleteGame();

#endregion

#region GemreController

app.MapGet("/genres", (GameStoreData data) => data.GetAllGenres().Select(g => new GenreDto(g.Id, g.Name)));

#endregion

app.Run();