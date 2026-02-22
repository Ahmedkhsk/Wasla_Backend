using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateUserEvent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serviceId",
                table: "UserEvents");

            migrationBuilder.AlterColumn<int>(
                name: "eventType",
                table: "UserEvents",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "serviceProviderId",
                table: "UserEvents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "serviceProviderId",
                table: "UserEvents");

            migrationBuilder.AlterColumn<string>(
                name: "eventType",
                table: "UserEvents",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "serviceId",
                table: "UserEvents",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
