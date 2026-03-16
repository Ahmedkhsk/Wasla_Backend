using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class TechnicanUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Documents",
                table: "Technicians",
                newName: "DocumentsJson");

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Technicians",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Technicians");

            migrationBuilder.RenameColumn(
                name: "DocumentsJson",
                table: "Technicians",
                newName: "Documents");
        }
    }
}
