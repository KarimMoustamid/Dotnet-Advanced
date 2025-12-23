namespace GameStore.API.Data
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Models;

    /// <summary>
    /// Extension helpers for data-related startup tasks.
    /// </summary>
    public static class DataExtensions
    {
        public static async Task InitializeDbAsync(this WebApplication app)
        {
           await app.MigrateDbAsync();
           await app.SeedDataAsync();
           app.Logger.LogTrace(999,"Database Is Ready!");
        }
        /// <summary>
        /// Run pending EF Core migrations at an application startup.
        /// </summary>
        /// <remarks>
        /// This method creates a short-lived DI scope because scoped services
        /// (like DbContext) must be resolved from a scope outside of an HTTP request.
        /// The scope is disposed automatically by the using statement which also
        /// disposes the resolved DbContext and its DB connections.
        /// This will not work in production if we have multiple instances of the app.
        /// In this case it is better to use a separate application for migrations.
        /// </remarks>
        private static async Task MigrateDbAsync(this WebApplication app)
        {
            // Create a new IServiceScope so we can resolve scoped services (e.g. DbContext).
            // Resolving scoped services directly from the root provider is invalid.
            using var scope = app.Services.CreateScope();

            // Resolve the application's DbContext from the scope.
            // GetRequiredService<T> will throw immediately if the service is not registered,
            // making any misconfiguration fail fast during startup.
            GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

            // Apply any pending migrations to the database. This runs IDataProvider-specific
            // SQL to update the schema to the latest model. Consider wrapping in try/catch
            // or adding logging if failure handling is required.
            await dbContext.Database.MigrateAsync();

            // The using statement disposes the scope here, which disposes the DbContext.
        }

        private static async Task SeedDataAsync(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();
            GameStoreContext dbContext = scope.ServiceProvider.GetRequiredService<GameStoreContext>();

            if (!dbContext.Genres.Any())
            {
                dbContext.Genres.AddRange(
                    new Genre { Name = "RPG" },
                    new Genre { Name = "Action RPG" },
                    new Genre { Name = "Roguelike" },
                    new Genre { Name = "Strategy" },
                    new Genre { Name = "Simulation" }
                    );

                await dbContext.SaveChangesAsync();
            }
        }
    }
}