using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuContabilidad : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Icon", "IsActive", "Name", "Order", "ParentId", "Route", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "account_balance", true, "Contabilidad", 10, null, "#", null, null },
                    { 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "list_alt", true, "Plan de Cuentas", 1, 26, "/plan-cuentas", null, null },
                    { 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "menu_book", true, "Asientos Contables", 2, 26, "/asientos-contables", null, null },
                    { 29, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "assessment", true, "Reportes Contables", 3, 26, "/reportes-contables", null, null },
                    { 30, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "tune", true, "Config. Cuentas", 4, 26, "/configuracion-contable", null, null }
                });

            migrationBuilder.InsertData(
                table: "RoleMenus",
                columns: new[] { "MenuId", "RoleId" },
                values: new object[,]
                {
                    { 26, 1 },
                    { 27, 1 },
                    { 28, 1 },
                    { 29, 1 },
                    { 30, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 26, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 27, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 28, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 29, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 30, 1 });

            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 27);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 28);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 29);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 30);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 26);
        }
    }
}
