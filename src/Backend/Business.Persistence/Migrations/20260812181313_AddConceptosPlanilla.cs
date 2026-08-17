using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConceptosPlanilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConceptosPlanilla",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    MetodoCalculo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Porcentaje = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: true),
                    MontoFijo = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: true),
                    AfectaAfp = table.Column<bool>(type: "bit", nullable: false),
                    AfectaEssalud = table.Column<bool>(type: "bit", nullable: false),
                    EsSistema = table.Column<bool>(type: "bit", nullable: false),
                    Orden = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConceptosPlanilla", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "ConceptosPlanilla",
                columns: new[] { "Id", "AfectaAfp", "AfectaEssalud", "Codigo", "CreatedAt", "CreatedBy", "EsSistema", "IsActive", "MetodoCalculo", "MontoFijo", "Nombre", "Orden", "Porcentaje", "Tipo", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, true, true, "SUELDO", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "MANUAL", null, "Sueldo básico", 1, null, "INGRESO", null, null },
                    { 2, true, true, "ASIGFAM", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "FIJO", 102.50m, "Asignación familiar", 2, null, "INGRESO", null, null },
                    { 3, false, false, "ONP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "PORCENTUAL", null, "ONP (Sistema Nacional)", 10, 0.130000m, "DESCUENTO", null, null },
                    { 4, false, false, "AFP_FONDO", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "PORCENTUAL", null, "AFP - Aporte al fondo", 11, 0.100000m, "DESCUENTO", null, null },
                    { 5, false, false, "AFP_COMISION", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "PORCENTUAL", null, "AFP - Comisión", 12, 0.016000m, "DESCUENTO", null, null },
                    { 6, false, false, "AFP_SEGURO", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "PORCENTUAL", null, "AFP - Prima de seguro", 13, 0.013500m, "DESCUENTO", null, null },
                    { 7, false, false, "ESSALUD", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "PORCENTUAL", null, "EsSalud (aporte empleador)", 20, 0.090000m, "APORTE", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPlanilla_Codigo",
                table: "ConceptosPlanilla",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConceptosPlanilla");
        }
    }
}
