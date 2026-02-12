using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class LastOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BaseBookings_AspNetUsers_ServiceProviderId",
                table: "BaseBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_GymBookings_BaseServices_ServiceId",
                table: "GymBookings");

            migrationBuilder.DropIndex(
                name: "IX_BaseBookings_ServiceProviderId",
                table: "BaseBookings");

            migrationBuilder.DropColumn(
                name: "ServiceProviderId",
                table: "BaseBookings");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "GymBookings",
                newName: "GymServiceType");

            migrationBuilder.RenameColumn(
                name: "Type",
                table: "BaseBookings",
                newName: "ServiceProviderType");

            migrationBuilder.AddColumn<string>(
                name: "GymId",
                table: "GymBookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_GymBookings_GymId",
                table: "GymBookings",
                column: "GymId");

            migrationBuilder.AddForeignKey(
                name: "FK_GymBookings_BaseServices_ServiceId",
                table: "GymBookings",
                column: "ServiceId",
                principalTable: "BaseServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GymBookings_Gym_GymId",
                table: "GymBookings",
                column: "GymId",
                principalTable: "Gym",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GymBookings_BaseServices_ServiceId",
                table: "GymBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_GymBookings_Gym_GymId",
                table: "GymBookings");

            migrationBuilder.DropIndex(
                name: "IX_GymBookings_GymId",
                table: "GymBookings");

            migrationBuilder.DropColumn(
                name: "GymId",
                table: "GymBookings");

            migrationBuilder.RenameColumn(
                name: "GymServiceType",
                table: "GymBookings",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "ServiceProviderType",
                table: "BaseBookings",
                newName: "Type");

            migrationBuilder.AddColumn<string>(
                name: "ServiceProviderId",
                table: "BaseBookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BaseBookings_ServiceProviderId",
                table: "BaseBookings",
                column: "ServiceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_BaseBookings_AspNetUsers_ServiceProviderId",
                table: "BaseBookings",
                column: "ServiceProviderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GymBookings_BaseServices_ServiceId",
                table: "GymBookings",
                column: "ServiceId",
                principalTable: "BaseServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
