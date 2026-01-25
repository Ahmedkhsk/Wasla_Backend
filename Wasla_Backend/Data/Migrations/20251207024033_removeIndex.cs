using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class removeIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Booking_serviceDayId",
                table: "Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_serviceDayId",
                table: "Booking",
                column: "serviceDayId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Booking_serviceDayId",
                table: "Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_serviceDayId",
                table: "Booking",
                column: "serviceDayId",
                unique: true);
        }
    }
}
