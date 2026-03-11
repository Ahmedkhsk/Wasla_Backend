using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    public partial class RideProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_BaseBookings_Id",
                table: "Rides");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Rides",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Rides");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Rides",
                type: "int",
                nullable: false)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Rides",
                table: "Rides",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Rides",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Rides");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Rides",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_BaseBookings_Id",
                table: "Rides",
                column: "Id",
                principalTable: "BaseBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}