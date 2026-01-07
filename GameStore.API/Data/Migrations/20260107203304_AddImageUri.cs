using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameStore.API.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddImageUri : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add a nullable ImageUri column to Games.
            migrationBuilder.AddColumn<string>(
                name: "ImageUri",
                table: "Games",
                type: "TEXT",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Remove the ImageUri column when rolling back.
            migrationBuilder.DropColumn(
                name: "ImageUri",
                table: "Games");
        }
    }
}