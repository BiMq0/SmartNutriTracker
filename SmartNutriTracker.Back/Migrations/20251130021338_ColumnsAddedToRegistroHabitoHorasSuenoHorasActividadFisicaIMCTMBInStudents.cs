using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartNutriTracker.Back.Migrations
{
    /// <inheritdoc />
    public partial class ColumnsAddedToRegistroHabitoHorasSuenoHorasActividadFisicaIMCTMBInStudents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "HorasActividadFisica",
                table: "RegistroHabitos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "HorasSueno",
                table: "RegistroHabitos",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Sexo",
                table: "Estudiantes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TMB",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "IMC",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: "\"Peso\" / (\"Altura\" * \"Altura\")",
                stored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IMC",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "HorasActividadFisica",
                table: "RegistroHabitos");

            migrationBuilder.DropColumn(
                name: "HorasSueno",
                table: "RegistroHabitos");

            migrationBuilder.DropColumn(
                name: "Sexo",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "TMB",
                table: "Estudiantes");
        }
    }
}
