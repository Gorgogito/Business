using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddConfiguracionCuentasContables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConfiguracionesCuentasContables",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmpresaId = table.Column<int>(type: "int", nullable: false, defaultValue: 1),
                    Concepto = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Modulo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Descripcion = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CuentaContableId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfiguracionesCuentasContables", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConfiguracionesCuentasContables_CuentasContables_CuentaContableId",
                        column: x => x.CuentaContableId,
                        principalTable: "CuentasContables",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Parametros",
                columns: new[] { "Id", "Codigo", "CreatedAt", "CreatedBy", "Descripcion", "IsActive", "Modulo", "Nombre", "UpdatedAt", "UpdatedBy", "Valor" },
                values: new object[] { 4, "TOPE_ASEGURABLE_AFP", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Tope de la base de cálculo de la prima de seguro AFP (SBS)", true, "RR.HH.", "Tope remuneración asegurable AFP", null, null, "10878" });

            // Cuentas PCGE nuevas para la provisión de beneficios sociales (CTS/gratificación/vacaciones).
            migrationBuilder.InsertData(
                table: "CuentasContables",
                columns: new[] { "Id", "EmpresaId", "Clase", "Codigo", "CreatedAt", "CreatedBy", "EsMovimiento", "IsActive", "Naturaleza", "Nivel", "Nombre", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    // Ids 85/86 (no 29/30): la base de pruebas ya ocupa 29-84 con planes clonados
                    // de empresas 2 y 3 (ver CatalogoEmpresaService); MAX(Id) real era 84.
                    { 85, 1, "GASTO", "629", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "DEUDORA", 3, "Beneficios sociales de los trabajadores", null, null },
                    { 86, 1, "PASIVO", "413", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, true, "ACREEDORA", 3, "Compensación por tiempo de servicios y beneficios sociales por pagar", null, null }
                });

            // Configuración de cuentas por concepto (empresa base = 1) para los asientos automáticos:
            // deja de haber códigos hardcodeados en los servicios, cada concepto apunta a una cuenta del plan.
            migrationBuilder.InsertData(
                table: "ConfiguracionesCuentasContables",
                columns: new[] { "Id", "EmpresaId", "Concepto", "Modulo", "Descripcion", "CuentaContableId", "CreatedAt", "CreatedBy", "IsActive", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { 1, 1, "VENTA_CLIENTE", "Ventas", "Cliente / cuenta por cobrar de la factura", 5, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 2, 1, "VENTA_INGRESO", "Ventas", "Ingreso por venta de mercadería", 20, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 3, 1, "VENTA_IGV", "Ventas", "IGV de ventas", 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 4, 1, "VENTA_COSTO", "Ventas", "Costo de ventas", 18, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 5, 1, "VENTA_INVENTARIO", "Ventas", "Salida de mercadería del inventario", 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 6, 1, "COMPRA_MERCADERIA", "Compras", "Compra de mercadería", 14, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 7, 1, "COMPRA_IGV", "Compras", "IGV de compras", 10, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 8, 1, "COMPRA_PROVEEDOR", "Compras", "Proveedor / cuenta por pagar de la recepción", 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 9, 1, "COMPRA_INVENTARIO", "Compras", "Ingreso de mercadería al inventario", 7, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 10, 1, "COMPRA_VARIACION", "Compras", "Variación de existencias por compra", 16, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 11, 1, "TESORERIA_EFECTIVO", "Tesorería", "Caja (cobros/pagos en efectivo)", 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 12, 1, "TESORERIA_BANCOS", "Tesorería", "Bancos (cobros/pagos no efectivo)", 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 13, 1, "PLANILLA_REMUNERACIONES", "Planillas", "Gasto de remuneraciones", 22, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 14, 1, "PLANILLA_APORTES", "Planillas", "Aportes del empleador (EsSalud)", 23, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 15, 1, "PLANILLA_TRIBUTOS_POR_PAGAR", "Planillas", "ONP/AFP/EsSalud por pagar", 24, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 16, 1, "PLANILLA_REMUNERACIONES_POR_PAGAR", "Planillas", "Neto de planilla por pagar", 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 17, 1, "PLANILLA_BENEFICIOS_GASTO", "Planillas", "Gasto por beneficios sociales (CTS/gratificación/vacaciones)", 85, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 18, 1, "PLANILLA_BENEFICIOS_POR_PAGAR", "Planillas", "Beneficios sociales por pagar", 86, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 19, 1, "PRODUCCION_PRODUCTOS_TERMINADOS", "Producción", "Ingreso de productos terminados al inventario", 27, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 20, 1, "PRODUCCION_VARIACION", "Producción", "Variación de la producción por consumo de materia prima", 28, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 21, 1, "PRODUCCION_MOD_POR_PAGAR", "Producción", "Mano de obra directa por pagar", 26, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null },
                    { 22, 1, "PRODUCCION_CIF_POR_PAGAR", "Producción", "Costos indirectos de fabricación por pagar", 12, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, true, null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesCuentasContables_CuentaContableId",
                table: "ConfiguracionesCuentasContables",
                column: "CuentaContableId");

            migrationBuilder.CreateIndex(
                name: "IX_ConfiguracionesCuentasContables_EmpresaId_Concepto",
                table: "ConfiguracionesCuentasContables",
                columns: new[] { "EmpresaId", "Concepto" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConfiguracionesCuentasContables");

            migrationBuilder.DeleteData(
                table: "Parametros",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 85);

            migrationBuilder.DeleteData(
                table: "CuentasContables",
                keyColumn: "Id",
                keyValue: 86);
        }
    }
}
