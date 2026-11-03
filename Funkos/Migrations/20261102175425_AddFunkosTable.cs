using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Funkos.Migrations
{
    /// <inheritdoc />
    public partial class AddFunkosTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categoria",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categoria", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Proveedor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Descripcion = table.Column<string>(type: "character varying(300)", maxLength: 300, nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Proveedor", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Funkos",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Precio = table.Column<decimal>(type: "numeric(18,2)", nullable: false),
                    CategoriaId = table.Column<long>(type: "bigint", nullable: false),
                    ProveedorId = table.Column<long>(type: "bigint", nullable: false),
                    Stock = table.Column<int>(type: "integer", nullable: false, defaultValue: 0),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false),
                    DeleteAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Funkos", x => x.Id);
                    table.CheckConstraint("CK_Funko_CategoriaId", "\"CategoriaId\" >= 1");
                    table.CheckConstraint("CK_Funko_Precio", "\"Precio\" >= 1.00");
                    table.CheckConstraint("CK_Funko_ProveedorId", "\"ProveedorId\" >= 1");
                    table.CheckConstraint("CK_Funko_Stock", "\"Stock\" >= 0");
                    table.ForeignKey(
                        name: "FK_Funkos_Categoria_CategoriaId",
                        column: x => x.CategoriaId,
                        principalTable: "Categoria",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Funkos_Proveedor_ProveedorId",
                        column: x => x.ProveedorId,
                        principalTable: "Proveedor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Descripcion",
                table: "Categoria",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categoria_Name",
                table: "Categoria",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funkos_CategoriaId",
                table: "Funkos",
                column: "CategoriaId");

            migrationBuilder.CreateIndex(
                name: "IX_Funkos_Name",
                table: "Funkos",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Funkos_ProveedorId",
                table: "Funkos",
                column: "ProveedorId");

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_Descripcion",
                table: "Proveedor",
                column: "Descripcion",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedor_Name",
                table: "Proveedor",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Funkos");

            migrationBuilder.DropTable(
                name: "Categoria");

            migrationBuilder.DropTable(
                name: "Proveedor");
        }
    }
}
