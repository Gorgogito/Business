using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPlanContable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CuentasContables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Codigo = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clase = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Naturaleza = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Nivel = table.Column<int>(type: "int", nullable: false),
                    EsMovimiento = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CuentasContables", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "Clase", "Codigo", "CreatedAt", "CreatedBy", "EsMovimiento", "IsActive", "Naturaleza", "Nivel", "Nombre", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, "ACTIVO", "10", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "DEUDORA", 2, "Efectivo y equivalentes de efectivo", null, null },
                    { 2, "ACTIVO", "101", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Caja", null, null },
                    { 3, "ACTIVO", "104", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Cuentas corrientes en instituciones financieras", null, null },
                    { 4, "ACTIVO", "12", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "DEUDORA", 2, "Cuentas por cobrar comerciales - Terceros", null, null },
                    { 5, "ACTIVO", "121", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Facturas, boletas y otros comprobantes por cobrar", null, null },
                    { 6, "ACTIVO", "20", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "DEUDORA", 2, "Mercaderías", null, null },
                    { 7, "ACTIVO", "201", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Mercaderías manufacturadas", null, null },
                    { 8, "PASIVO", "40", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "ACREEDORA", 2, "Tributos, contraprestaciones y aportes por pagar", null, null },
                    { 9, "PASIVO", "401", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "ACREEDORA", 3, "Gobierno central", null, null },
                    { 10, "PASIVO", "4011", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "ACREEDORA", 4, "Impuesto general a las ventas (IGV)", null, null },
                    { 11, "PASIVO", "42", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "ACREEDORA", 2, "Cuentas por pagar comerciales - Terceros", null, null },
                    { 12, "PASIVO", "421", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "ACREEDORA", 3, "Facturas, boletas y otros comprobantes por pagar", null, null },
                    { 13, "GASTO", "60", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "DEUDORA", 2, "Compras", null, null },
                    { 14, "GASTO", "601", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Mercaderías (compras)", null, null },
                    { 15, "GASTO", "61", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "DEUDORA", 2, "Variación de existencias", null, null },
                    { 16, "GASTO", "611", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Mercaderías (variación)", null, null },
                    { 17, "COSTO", "69", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "DEUDORA", 2, "Costo de ventas", null, null },
                    { 18, "COSTO", "691", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Mercaderías (costo de ventas)", null, null },
                    { 19, "INGRESO", "70", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, false, true, "ACREEDORA", 2, "Ventas", null, null },
                    { 20, "INGRESO", "701", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "ACREEDORA", 3, "Mercaderías (ventas)", null, null }
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "IsActive", "Module", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 13, "accounting.view", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Contabilidad", "Ver Contabilidad", null, null },
                    { 14, "accounting.manage", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, "Contabilidad", "Gestionar Contabilidad", null, null }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { 13, 1 },
                    { 14, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CuentasContables_Codigo",
                table: "CuentasContables",
                column: "Codigo",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CuentasContables");

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 13, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { 14, 1 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 14);
        }
    }
}
