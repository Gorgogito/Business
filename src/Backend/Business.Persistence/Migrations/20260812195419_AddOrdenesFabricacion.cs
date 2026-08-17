using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddOrdenesFabricacion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OrdenesFabricacion",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Numero = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ProductoId = table.Column<int>(type: "int", nullable: false),
                    RecetaId = table.Column<int>(type: "int", nullable: false),
                    CantidadProducir = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    AlmacenId = table.Column<int>(type: "int", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Estado = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CostoMateriaPrima = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoManoObra = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoIndirecto = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenesFabricacion", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenesFabricacion_Productos_ProductoId",
                        column: x => x.ProductoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrdenesFabricacion_Recetas_RecetaId",
                        column: x => x.RecetaId,
                        principalTable: "Recetas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrdenFabricacionDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrdenFabricacionId = table.Column<int>(type: "int", nullable: false),
                    InsumoId = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoUnitario = table.Column<decimal>(type: "decimal(18,4)", precision: 18, scale: 4, nullable: false),
                    CostoTotal = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrdenFabricacionDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrdenFabricacionDetalles_OrdenesFabricacion_OrdenFabricacionId",
                        column: x => x.OrdenFabricacionId,
                        principalTable: "OrdenesFabricacion",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrdenFabricacionDetalles_Productos_InsumoId",
                        column: x => x.InsumoId,
                        principalTable: "Productos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Correlativos",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "EmpresaId", "IsActive", "Longitud", "Prefijo", "Serie", "TipoDocumento", "UltimoNumero", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 1, true, 6, "OF-", "OF", "ORDEN_FAB", 0, null, null });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Clase", "Codigo", "CreatedAt", "CreatedBy", "EsMovimiento", "IsActive", "Naturaleza", "Nivel", "Nombre", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 27, "ACTIVO", "21", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 2, "Productos terminados", null, null },
                    { 28, "INGRESO", "71", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "ACREEDORA", 2, "Variación de la producción almacenada", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesFabricacion_ProductoId",
                table: "OrdenesFabricacion",
                column: "ProductoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenesFabricacion_RecetaId",
                table: "OrdenesFabricacion",
                column: "RecetaId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenFabricacionDetalles_InsumoId",
                table: "OrdenFabricacionDetalles",
                column: "InsumoId");

            migrationBuilder.CreateIndex(
                name: "IX_OrdenFabricacionDetalles_OrdenFabricacionId",
                table: "OrdenFabricacionDetalles",
                column: "OrdenFabricacionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OrdenFabricacionDetalles");

            migrationBuilder.DropTable(
                name: "OrdenesFabricacion");

            migrationBuilder.DeleteData(
                table: "Correlativos",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 27);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 28);
        }
    }
}
