using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class UReservationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_AspNetUsers_userId",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "restaurantId",
                table: "Reservations",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_restaurantId",
                table: "Reservations",
                column: "restaurantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Resident_userId",
                table: "Reservations",
                column: "userId",
                principalTable: "Resident",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Restaurant_restaurantId",
                table: "Reservations",
                column: "restaurantId",
                principalTable: "Restaurant",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Resident_userId",
                table: "Reservations");

            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Restaurant_restaurantId",
                table: "Reservations");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_restaurantId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "restaurantId",
                table: "Reservations");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_AspNetUsers_userId",
                table: "Reservations",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
