using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartNutriTracker.Back.Migrations
{
    /// <inheritdoc />
    public partial class AutomaticIMCTMBCalculatedColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "IMC",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: "\"Peso\" / (\"Altura\" * \"Altura\")",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: "\"Peso\" / (\"Altura\" * \"Altura\")",
                oldStored: null);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "IMC",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                computedColumnSql: "\"Peso\" / (\"Altura\" * \"Altura\")",
                oldClrType: typeof(decimal),
                oldType: "numeric(5,2)",
                oldComputedColumnSql: "\"Peso\" / (\"Altura\" * \"Altura\")");
        }
    }
}
