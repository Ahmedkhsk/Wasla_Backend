using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class DriverStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnline",
                table: "Driver");

            migrationBuilder.AddColumn<int>(
                name: "DriverStatus",
                table: "Driver",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DriverStatus",
                table: "Driver");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnline",
                table: "Driver",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
