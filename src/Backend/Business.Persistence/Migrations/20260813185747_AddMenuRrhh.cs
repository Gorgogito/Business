using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuRrhh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Icon", "IsActive", "Name", "Order", "ParentId", "Route", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 31, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "badge", true, "RR.HH.", 11, null, "#", null, null },
                    { 32, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "groups", true, "Trabajadores", 1, 31, "/trabajadores", null, null },
                    { 33, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "checklist", true, "Conceptos de Planilla", 2, 31, "/conceptos-planilla", null, null },
                    { 34, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "percent", true, "Tasas AFP", 3, 31, "/tasas-afp", null, null },
                    { 35, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "payments", true, "Planillas", 4, 31, "/planillas", null, null },
                    { 36, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "volunteer_activism", true, "Beneficios Sociales", 5, 31, "/beneficios-sociales", null, null }
                });

            migrationBuilder.InsertData(
                table: "RoleMenus",
                columns: new[] { "MenuId", "RoleId" },
                values: new object[,]
                {
                    { 31, 1 },
                    { 32, 1 },
                    { 33, 1 },
                    { 34, 1 },
                    { 35, 1 },
                    { 36, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 31, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 32, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 33, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 34, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 35, 1 });
            migrationBuilder.DeleteData(table: "RoleMenus", keyColumns: new[] { "MenuId", "RoleId" }, keyValues: new object[] { 36, 1 });

            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 32);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 33);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 34);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 35);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 36);
            migrationBuilder.DeleteData(table: "Menus", keyColumn: "Id", keyValue: 31);
        }
    }
}
