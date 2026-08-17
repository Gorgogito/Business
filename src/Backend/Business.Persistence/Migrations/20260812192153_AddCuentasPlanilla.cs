using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCuentasPlanilla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Clase", "Codigo", "CreatedAt", "CreatedBy", "EsMovimiento", "IsActive", "Naturaleza", "Nivel", "Nombre", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 21, "GASTO", "62", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "DEUDORA", 2, "Gastos de personal, directores y gerentes", null, null },
                    { 22, "GASTO", "621", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Remuneraciones", null, null },
                    { 23, "GASTO", "627", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Seguridad, previsión social y otras contribuciones", null, null },
                    { 24, "PASIVO", "403", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "ACREEDORA", 3, "Instituciones públicas (aportes por pagar)", null, null },
                    { 25, "PASIVO", "41", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "ACREEDORA", 2, "Remuneraciones y participaciones por pagar", null, null },
                    { 26, "PASIVO", "411", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "ACREEDORA", 3, "Remuneraciones por pagar", null, null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 23);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 24);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 25);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 26);
        }
    }
}
