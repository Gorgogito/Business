using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddAsientosContables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AsientosContables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Glosa = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Referencia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalDebe = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    TotalHaber = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsientosContables", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AsientoContableDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AsientoContableId = table.Column<int>(type: "int", nullable: false),
                    CuentaContableId = table.Column<int>(type: "int", nullable: false),
                    Debe = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Haber = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Glosa = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AsientoContableDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AsientoContableDetalles_AsientosContables_AsientoContableId",
                        column: x => x.AsientoContableId,
                        principalTable: "AsientosContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AsientoContableDetalles_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Correlativos",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "EmpresaId", "IsActive", "Longitud", "Prefijo", "Serie", "TipoDocumento", "UltimoNumero", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 8, "ASI-", "ASI", "ASIENTO", 0, null, null });

            migrationBuilder.CreateIndex(
                name: "IX_AsientoContableDetalles_AsientoContableId",
                table: "AsientoContableDetalles",
                column: "AsientoContableId");

            migrationBuilder.CreateIndex(
                name: "IX_AsientoContableDetalles_CuentaContableId",
                table: "AsientoContableDetalles",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_AsientosContables_Fecha",
                table: "AsientosContables",
                column: "Fecha");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AsientoContableDetalles");

            migrationBuilder.DropTable(
                name: "AsientosContables");

            migrationBuilder.DeleteData(
                table: "Correlativos",
                keyColumn: "Id",
                keyValue: 10);
        }
    }
}
