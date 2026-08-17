using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMenuAnalitica : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Menus",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Icon", "IsActive", "Name", "Order", "ParentId", "Route", "UpdatedAt", "UpdatedBy" },
                values: new object[] { 25, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "insights", true, "Analítica", 9, null, "/analitica", null, null });

            migrationBuilder.InsertData(
                table: "RoleMenus",
                columns: new[] { "MenuId", "RoleId" },
                values: new object[] { 25, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoleMenus",
                keyColumns: new[] { "MenuId", "RoleId" },
                keyValues: new object[] { 25, 1 });

            migrationBuilder.DeleteData(
                table: "Menus",
                keyColumn: "Id",
                keyValue: 25);
        }
    }
}
