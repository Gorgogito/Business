using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCorrelativos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Correlativos",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TipoDocumento = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Serie = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    EmpresaId = table.Column<int>(type: "int", nullable: false),
                    UltimoNumero = table.Column<int>(type: "int", nullable: false),
                    Longitud = table.Column<int>(type: "int", nullable: false),
                    Prefijo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Correlativos", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Correlativos",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "EmpresaId", "IsActive", "Longitud", "Prefijo", "Serie", "TipoDocumento", "UltimoNumero", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 8, "COT-", "COT", "COTIZACION", 0, null, null },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 8, "PED-", "PED", "PEDIDO", 0, null, null },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 8, "", "F001", "FACTURA", 0, null, null },
                    { 4, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 8, "", "B001", "BOLETA", 0, null, null },
                    { 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 8, "OC-", "OC", "ORDEN_COMPRA", 0, null, null },
                    { 6, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 8, "REC-", "REC", "RECEPCION", 0, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Correlativos_TipoDocumento_Serie_EmpresaId",
                table: "Correlativos",
                columns: new[] { "TipoDocumento", "Serie", "EmpresaId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Correlativos");
        }
    }
}
