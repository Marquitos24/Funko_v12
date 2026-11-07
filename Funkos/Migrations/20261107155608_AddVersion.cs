using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Funkos.Migrations
{
    /// <inheritdoc />
    public partial class AddVersion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Imagen",
                table: "Funkos",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Funkos",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Version",
                table: "Categoria",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Imagen",
                table: "Funkos");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Funkos");

            migrationBuilder.DropColumn(
                name: "Version",
                table: "Categoria");
        }
    }
}
