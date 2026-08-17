using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Business.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPagoBeneficioSocial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EstadoPago",
                table: "BeneficiosSociales",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "PENDIENTE");

            migrationBuilder.AddColumn<DateTime>(
                name: "FechaPago",
                table: "BeneficiosSociales",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MedioPago",
                table: "BeneficiosSociales",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstadoPago",
                table: "BeneficiosSociales");

            migrationBuilder.DropColumn(
                name: "FechaPago",
                table: "BeneficiosSociales");

            migrationBuilder.DropColumn(
                name: "MedioPago",
                table: "BeneficiosSociales");
        }
    }
}
