using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartNutriTracker.Back.Migrations
{
    /// <inheritdoc />
    public partial class FixOnAge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TMB",
                table: "Estudiantes");

            migrationBuilder.DropColumn(
                name: "Edad",
                table: "Estudiantes");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "TMB",
                table: "Estudiantes",
                type: "numeric(5,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "Edad",
                table: "Estudiantes",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
