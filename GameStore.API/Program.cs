using System.Diagnostics;
using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;
using GameStore.API.shared.Timing;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("GameStore");

/*
 * - DbContext is designed to be used as a single Unit of Work.
 * - DbContext created --> entity changes tracked --> SaveChanges() called --> changes saved to database --> disposed.
 * - DB connections are expensive.
 * - DbContext is not thread-safe.
 * - Increased memory usage due to change being tracked.
 *
 * Use Scoped service to create a new DbContext for each request.
 * - Align the Context lifetime with the HTTP request lifetime.
 * - There is only one thread working with the database at a time.
 * - Ensure each request has its own isolated context.
 */

builder.Services.AddSqlite<GameStoreContext>(connectionString);
//builder.Services.AddDbContext<GameStoreContext>(options =>
//{
//    options.UseSqlite(connectionString);
//});

builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ---------- CONTROLLERS ----------


app.MapGet("/", () => "Hello World!");
app.MapGames();

app.UseMiddleware<RequestTimingMiddleware>();

app.MapGenres();

await app.InitializeDbAsync();


app.Run();