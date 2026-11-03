using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Funkos.Migrations
{
    /// <inheritdoc />
    public partial class AddFunkosTable3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Proveedor_Descripcion",
                table: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Categoria_Descripcion",
                table: "Categoria");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_Descripcion",
                table: "Proveedor",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Descripcion",
                table: "Categoria",
                column: "Descripcion",
                unique: true);
        }
    }
}
