using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateGym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassScheduleJson",
                table: "Gym");

            migrationBuilder.DropColumn(
                name: "DayPassPrice",
                table: "Gym");

            migrationBuilder.DropColumn(
                name: "Facilities",
                table: "Gym");

            migrationBuilder.DropColumn(
                name: "MaxCapacity",
                table: "Gym");

            migrationBuilder.DropColumn(
                name: "MembershipPlansJson",
                table: "Gym");

            migrationBuilder.DropColumn(
                name: "TrainerCount",
                table: "Gym");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phones",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");

            migrationBuilder.AddColumn<string>(
                name: "photos",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "[]");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "Gym");

            migrationBuilder.DropColumn(
                name: "phones",
                table: "Gym");

            migrationBuilder.DropColumn(
                name: "photos",
                table: "Gym");

            migrationBuilder.AddColumn<string>(
                name: "ClassScheduleJson",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DayPassPrice",
                table: "Gym",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Facilities",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MaxCapacity",
                table: "Gym",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "MembershipPlansJson",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TrainerCount",
                table: "Gym",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
