using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTasasAfpYBeneficiosSociales : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BeneficiosSociales",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    TrabajadorId = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Periodo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FechaCalculo = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RemuneracionComputable = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    MesesComputables = table.Column<decimal>(type: "decimal(9,4)", precision: 9, scale: 4, nullable: false),
                    Monto = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    BonificacionExtraordinaria = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    Observacion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BeneficiosSociales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BeneficiosSociales_Trabajadores_TrabajadorId",
                        column: x => x.TrabajadorId,
                        principalTable: "Trabajadores",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "TasasAfp",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AporteFondo = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    ComisionFlujo = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    PrimaSeguro = table.Column<decimal>(type: "decimal(9,6)", precision: 9, scale: 6, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TasasAfp", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "TasasAfp",
                columns: new[] { "Id", "AporteFondo", "ComisionFlujo", "CreatedAt", "CreatedBy", "IsActive", "Nombre", "PrimaSeguro", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 0.100000m, 0.014700m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Habitat", 0.013500m, null, null },
                    { 2, 0.100000m, 0.015500m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Integra", 0.013500m, null, null },
                    { 3, 0.100000m, 0.016000m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Prima", 0.013500m, null, null },
                    { 4, 0.100000m, 0.016900m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Profuturo", 0.013500m, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiosSociales_EmpresaId_TrabajadorId_Tipo_Periodo",
                table: "BeneficiosSociales",
                columns: new[] { "EmpresaId", "TrabajadorId", "Tipo", "Periodo" });

            migrationBuilder.CreateIndex(
                name: "IX_BeneficiosSociales_TrabajadorId",
                table: "BeneficiosSociales",
                column: "TrabajadorId");

            migrationBuilder.CreateIndex(
                name: "IX_TasasAfp_Nombre",
                table: "TasasAfp",
                column: "Nombre",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BeneficiosSociales");

            migrationBuilder.DropTable(
                name: "TasasAfp");
        }
    }
}
