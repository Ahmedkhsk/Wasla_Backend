using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class rename : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_userId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_ServiceDay_serviceDayId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Service_Doctor_doctorId",
                table: "Service");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceDay_Service_serviceId",
                table: "ServiceDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Service",
                table: "Service");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Booking",
                table: "Booking");

            migrationBuilder.RenameTable(
                name: "Service",
                newName: "DoctorServices");

            migrationBuilder.RenameTable(
                name: "Booking",
                newName: "DoctorBookings");

            migrationBuilder.RenameIndex(
                name: "IX_Service_doctorId",
                table: "DoctorServices",
                newName: "IX_DoctorServices_doctorId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_userId",
                table: "DoctorBookings",
                newName: "IX_DoctorBookings_userId");

            migrationBuilder.RenameIndex(
                name: "IX_Booking_serviceDayId",
                table: "DoctorBookings",
                newName: "IX_DoctorBookings_serviceDayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorServices",
                table: "DoctorServices",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DoctorBookings",
                table: "DoctorBookings",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorBookings_AspNetUsers_userId",
                table: "DoctorBookings",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorBookings_ServiceDay_serviceDayId",
                table: "DoctorBookings",
                column: "serviceDayId",
                principalTable: "ServiceDay",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorServices_Doctor_doctorId",
                table: "DoctorServices",
                column: "doctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceDay_DoctorServices_serviceId",
                table: "ServiceDay",
                column: "serviceId",
                principalTable: "DoctorServices",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorBookings_AspNetUsers_userId",
                table: "DoctorBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorBookings_ServiceDay_serviceDayId",
                table: "DoctorBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorServices_Doctor_doctorId",
                table: "DoctorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceDay_DoctorServices_serviceId",
                table: "ServiceDay");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorServices",
                table: "DoctorServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DoctorBookings",
                table: "DoctorBookings");

            migrationBuilder.RenameTable(
                name: "DoctorServices",
                newName: "Service");

            migrationBuilder.RenameTable(
                name: "DoctorBookings",
                newName: "Booking");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorServices_doctorId",
                table: "Service",
                newName: "IX_Service_doctorId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorBookings_userId",
                table: "Booking",
                newName: "IX_Booking_userId");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorBookings_serviceDayId",
                table: "Booking",
                newName: "IX_Booking_serviceDayId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Service",
                table: "Service",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Booking",
                table: "Booking",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_userId",
                table: "Booking",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_ServiceDay_serviceDayId",
                table: "Booking",
                column: "serviceDayId",
                principalTable: "ServiceDay",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Service_Doctor_doctorId",
                table: "Service",
                column: "doctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceDay_Service_serviceId",
                table: "ServiceDay",
                column: "serviceId",
                principalTable: "Service",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
