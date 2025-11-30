using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartNutriTracker.Back.Migrations
{
    /// <inheritdoc />
    public partial class TMBCalculatedColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FechaNacimiento",
                table: "Estudiantes",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<decimal>(
                name: "TMB",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "IMC",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: "\"Peso\" / (\"Altura\" * \"Altura\")",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: " Peso / (Altura * Altura)",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FechaNacimiento",
                table: "Estudiantes");

            migrationBuilder.AlterColumn<decimal>(
                name: "TMB",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: "CASE WHEN \"Sexo\" = 0 THEN (10 * \"Peso\") + (6.25 * (\"Altura\" * 100)) - (5 * EXTRACT(YEAR FROM AGE(CURRENT_DATE, \"FechaNacimiento\"))) + 5 WHEN \"Sexo\" = 1 THEN (10 * \"Peso\") + (6.25 * (\"Altura\" * 100)) - (5 * EXTRACT(YEAR FROM AGE(CURRENT_DATE, \"FechaNacimiento\"))) - 161 ELSE 0 END");

            migrationBuilder.AlterColumn<decimal>(
                name: "IMC",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: " Peso / (Altura * Altura)",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: "\"Peso\" / (\"Altura\" * \"Altura\")",
                oldStored: true);
        }
    }
}
