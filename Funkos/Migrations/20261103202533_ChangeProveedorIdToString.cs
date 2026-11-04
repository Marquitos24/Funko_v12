using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Funkos.Migrations
{
    /// <inheritdoc />
    public partial class ChangeProveedorIdToString : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Funkos_Proveedor_ProveedorId",
                table: "Funkos");

            migrationBuilder.DropTable(
                name: "Proveedor");

            migrationBuilder.DropIndex(
                name: "IX_Funkos_ProveedorId",
                table: "Funkos");

            migrationBuilder.DropCheckConstraint(
                name: "CK_Funko_ProveedorId",
                table: "Funkos");

            migrationBuilder.AlterColumn<string>(
                name: "ProveedorId",
                table: "Funkos",
                type: "character varying(24)",
                maxLength: 24,
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ProveedorId",
                table: "Funkos",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(24)",
                oldMaxLength: 24);

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DeleteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Funkos_ProveedorId",
                table: "Funkos",
                column: "ProveedorId");

            migrationBuilder.AddCheckConstraint(
                name: "CK_Funko_ProveedorId",
                table: "Funkos",
                sql: "\"ProveedorId\" >= 1");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_Name",
                table: "Proveedor",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Funkos_Proveedor_ProveedorId",
                table: "Funkos",
                column: "ProveedorId",
                principalTable: "Proveedor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
