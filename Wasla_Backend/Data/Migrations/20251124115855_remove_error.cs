using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.Data.Migrations
{
    public partial class remove_error : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add RowVersion (rowversion type)
            migrationBuilder.AddColumn<byte[]>(
                name: "rowversion",
                table: "Service",
                type: "rowversion",
                rowVersion: true,
                nullable: true);

            // Add IsBooked (bit)
            migrationBuilder.AddColumn<bool>(
                name: "isbooked",
                table: "Service",
                type: "bit",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "rowversion",
                table: "Service");

            migrationBuilder.DropColumn(
                name: "isbooked",
                table: "Service");
        }
    }
}
