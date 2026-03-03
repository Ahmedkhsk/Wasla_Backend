using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class driver : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentLatitude",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "CurrentLongitude",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Driver");

            migrationBuilder.AlterColumn<int>(
                name: "VehicleType",
                table: "Driver",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CarImages",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DriverFilesJson",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CarImages",
                table: "Driver");

            migrationBuilder.DropColumn(
                name: "DriverFilesJson",
                table: "Driver");

            migrationBuilder.AlterColumn<string>(
                name: "VehicleType",
                table: "Driver",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CurrentLatitude",
                table: "Driver",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CurrentLongitude",
                table: "Driver",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Driver",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
