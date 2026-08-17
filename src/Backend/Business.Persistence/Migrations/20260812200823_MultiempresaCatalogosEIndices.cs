using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class MultiempresaCatalogosEIndices : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trabajadores_Codigo",
                table: "Trabajadores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_RUC",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Productos_Codigo",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_CuentasContables_Codigo",
                table: "CuentasContables");

            migrationBuilder.DropIndex(
                name: "IX_ConceptosPlanilla_Codigo",
                table: "ConceptosPlanilla");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_RUC",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Almacenes_Codigo",
                table: "Almacenes");

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "CuentasContables",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "ConceptosPlanilla",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "ConceptosPlanilla",
                keyColumn: "Id",
                keyValue: 1,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ConceptosPlanilla",
                keyColumn: "Id",
                keyValue: 2,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ConceptosPlanilla",
                keyColumn: "Id",
                keyValue: 3,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ConceptosPlanilla",
                keyColumn: "Id",
                keyValue: 4,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ConceptosPlanilla",
                keyColumn: "Id",
                keyValue: 5,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ConceptosPlanilla",
                keyColumn: "Id",
                keyValue: 6,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "ConceptosPlanilla",
                keyColumn: "Id",
                keyValue: 7,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 1,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 2,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 3,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 4,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 5,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 6,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 7,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 8,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 9,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 10,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 11,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 12,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 13,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 14,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 15,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 16,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 17,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 18,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 19,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 20,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 21,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 22,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 23,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 24,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 25,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 26,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 27,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 28,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_EmpresaId_Codigo",
                table: "Trabajadores",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_EmpresaId_RUC",
                table: "Proveedores",
                columns: new[] { "EmpresaId", "RUC" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_EmpresaId_Codigo",
                table: "Productos",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CuentasContables_EmpresaId_Codigo",
                table: "CuentasContables",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPlanilla_EmpresaId_Codigo",
                table: "ConceptosPlanilla",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_EmpresaId_RUC",
                table: "Clientes",
                columns: new[] { "EmpresaId", "RUC" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Almacenes_EmpresaId_Codigo",
                table: "Almacenes",
                columns: new[] { "EmpresaId", "Codigo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Trabajadores_EmpresaId_Codigo",
                table: "Trabajadores");

            migrationBuilder.DropIndex(
                name: "IX_Proveedores_EmpresaId_RUC",
                table: "Proveedores");

            migrationBuilder.DropIndex(
                name: "IX_Productos_EmpresaId_Codigo",
                table: "Productos");

            migrationBuilder.DropIndex(
                name: "IX_CuentasContables_EmpresaId_Codigo",
                table: "CuentasContables");

            migrationBuilder.DropIndex(
                name: "IX_ConceptosPlanilla_EmpresaId_Codigo",
                table: "ConceptosPlanilla");

            migrationBuilder.DropIndex(
                name: "IX_Clientes_EmpresaId_RUC",
                table: "Clientes");

            migrationBuilder.DropIndex(
                name: "IX_Almacenes_EmpresaId_Codigo",
                table: "Almacenes");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "CuentasContables");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "ConceptosPlanilla");

            migrationBuilder.CreateIndex(
                name: "IX_Trabajadores_Codigo",
                table: "Trabajadores",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Proveedores_RUC",
                table: "Proveedores",
                column: "RUC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Productos_Codigo",
                table: "Productos",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_CuentasContables_Codigo",
                table: "CuentasContables",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConceptosPlanilla_Codigo",
                table: "ConceptosPlanilla",
                column: "Codigo",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clientes_RUC",
                table: "Clientes",
                column: "RUC",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Almacenes_Codigo",
                table: "Almacenes",
                column: "Codigo",
                unique: true);
        }
    }
}
