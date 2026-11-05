using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Funkos.Migrations
{
    /// <inheritdoc />
    public partial class IdYoutubeCategorias : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funkos_Categoria_CategoriaId",
                table: "Funkos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Funko_CategoriaId",
                table: "Funkos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Categoria");

            migrationBuilder.AlterColumn<string>(
                name: "CategoriaId",
                table: "Funkos",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "IdY",
                table: "Categoria",
                type: "character varying(11)",
                maxLength: 11,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria",
                column: "IdY");

            migrationBuilder.AddForeignKey(
                name: "FK_Funkos_Categoria_CategoriaId",
                table: "Funkos",
                column: "CategoriaId",
                principalTable: "Categoria",
                principalColumn: "IdY",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funkos_Categoria_CategoriaId",
                table: "Funkos");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria");

            migrationBuilder.DropColumn(
                name: "IdY",
                table: "Categoria");

            migrationBuilder.AlterColumn<long>(
                name: "CategoriaId",
                table: "Funkos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(11)",
                oldMaxLength: 11);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Categoria",
                type: "bigint",
                nullable: false,
                defaultValue: 0L)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categoria",
                table: "Categoria",
                column: "Id");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Funko_CategoriaId",
                table: "Funkos",
                sql: "\"CategoriaId\" >= 1");

            migrationBuilder.AddForeignKey(
                name: "FK_Funkos_Categoria_CategoriaId",
                table: "Funkos",
                column: "CategoriaId",
                principalTable: "Categoria",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
