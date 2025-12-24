using System.Diagnostics;
using GameStore.API.Data;
using GameStore.API.Features.Games;
using GameStore.API.Features.Genres;
using GameStore.API.shared.ErrorHandling;
using GameStore.API.shared.Timing;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);

// Register Problem Details (RFC 7807) to convert exceptions and validation errors
// into consistent, machine-readable error responses and map domain errors to HTTP status codes.
builder.Services.AddProblemDetails().AddExceptionHandler<GlobalExceptionHandler>();
//builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

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
builder.Services.AddHttpLogging(option =>
{
    option.LoggingFields = HttpLoggingFields.RequestMethod | HttpLoggingFields.RequestPath
        | HttpLoggingFields.ResponseStatusCode
        | HttpLoggingFields.RequestHeaders
        | HttpLoggingFields.ResponseHeaders
        | HttpLoggingFields.Duration;
    option.CombineLogs = true;
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

// ---------- CONTROLLERS ----------


app.MapGet("/", () => "Hello World!");
app.MapGames();
app.MapGenres();

//app.UseMiddleware<RequestTimingMiddleware>();
app.UseHttpLogging();

if (!app.Environment.IsDevelopment())
{
    // Use the generic exception handler in non-development environments to avoid
    // leaking stack traces or sensitive information to clients. The registered
    // Problem Details middleware will format exceptions into RFC 7807-compliant
    // responses (machine-readable error details and appropriate HTTP status codes).
    app.UseExceptionHandler();
}

// Show simple, human-readable status pages for non-success responses.
// Helpful for browsers and quick debugging; API clients still receive
// RFC 7807 Problem Details responses when that middleware is active.
app.UseStatusCodePages();


await app.InitializeDbAsync();


app.Run();