using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddScoped<GameDataLogger>();
builder.Services.AddSingleton<GameStoreData>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ---------- CONTROLLERS ----------

#region Controller
app.MapGet("/", () => "Hello World!");
app.MapGames();
app.MapGenres();
#endregion


app.Run();