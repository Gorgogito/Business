using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddMultiempresa : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Stocks",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "RecepcionesCompra",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Proveedores",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Productos",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Pedidos",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "OrdenesCompra",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "NotasVenta",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "MovimientosInventario",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "GuiasRemision",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Facturas",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "CuentasPorPagar",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "CuentasPorCobrar",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Cotizaciones",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Clientes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Categorias",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "EmpresaId",
                table: "Almacenes",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "Almacenes",
                keyColumn: "Id",
                keyValue: 1,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 1,
                column: "EmpresaId",
                value: 1);

            migrationBuilder.UpdateData(
                table: "Categorias",
                keyColumn: "Id",
                keyValue: 2,
                column: "EmpresaId",
                value: 1);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Stocks");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "RecepcionesCompra");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Proveedores");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Productos");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Pedidos");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "OrdenesCompra");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "NotasVenta");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "MovimientosInventario");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "GuiasRemision");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Facturas");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "CuentasPorPagar");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "CuentasPorCobrar");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Cotizaciones");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Clientes");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Categorias");

            migrationBuilder.DropColumn(
                name: "EmpresaId",
                table: "Almacenes");
        }
    }
}
