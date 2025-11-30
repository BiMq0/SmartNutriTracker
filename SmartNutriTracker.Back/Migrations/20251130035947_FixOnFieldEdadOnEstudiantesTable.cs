using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartNutriTracker.Back.Migrations
{
    /// <inheritdoc />
    public partial class FixOnFieldEdadOnEstudiantesTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Edad",
                table: "Estudiantes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Edad",
                table: "Estudiantes",
                type: "integer",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer",
                oldComputedColumnSql: "EXTRACT(YEAR FROM AGE(CURRENT_DATE, \"FechaNacimiento\"))");
        }
    }
}
