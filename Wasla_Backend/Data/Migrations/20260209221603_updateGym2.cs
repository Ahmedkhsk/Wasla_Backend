using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateGym2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "photos",
                table: "Gym");

            migrationBuilder.AddColumn<string>(
                name: "imagesJson",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "imagesJson",
                table: "Gym");

            migrationBuilder.AddColumn<string>(
                name: "photos",
                table: "Gym",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
