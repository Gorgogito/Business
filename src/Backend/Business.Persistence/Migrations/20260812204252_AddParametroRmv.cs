using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddParametroRmv : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Parametros",
                columns: new[] { "Id", "Codigo", "CreatedAt", "CreatedBy", "Descripcion", "IsActive", "Modulo", "Nombre", "UpdatedAt", "UpdatedBy", "Valor" },
                values: new object[] { 3, "RMV", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "RMV vigente (base de asignación familiar)", true, "RR.HH.", "Remuneración Mínima Vital", null, null, "1025" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
