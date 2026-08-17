using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuProduccion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Icon", "IsActive", "Name", "Order", "ParentId", "Route", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 37, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "precision_manufacturing", true, "Producción", 12, null, "#", null, null },
                    { 38, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "science", true, "Recetas", 1, 37, "/recetas", null, null },
                    { 39, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "precision_manufacturing", true, "Órdenes de Fabricación", 2, 37, "/ordenes-fabricacion", null, null }
                });

            migrationBuilder.InsertData(
                table: "RoleMenus",
                columns: new[] { "MenuId", "RoleId" },
                values: new object[,]
                {
                    { 37, 1 },
                    { 38, 1 },
                    { 39, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 37, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 38, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 39, 1 });

            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 38);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 39);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 37);
        }
    }
}
