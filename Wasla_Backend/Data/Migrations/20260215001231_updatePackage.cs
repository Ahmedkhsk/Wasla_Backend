using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updatePackage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymBookings_BaseServices_ServiceId",
                table: "GymBookings");

            migrationBuilder.AddForeignKey(
                name: "FK_GymBookings_Packages_ServiceId",
                table: "GymBookings",
                column: "ServiceId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymBookings_Packages_ServiceId",
                table: "GymBookings");

            migrationBuilder.AddForeignKey(
                name: "FK_GymBookings_BaseServices_ServiceId",
                table: "GymBookings",
                column: "ServiceId",
                principalTable: "BaseServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
