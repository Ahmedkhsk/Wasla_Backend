using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class HandleDateInBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "BaseBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1));


            migrationBuilder.Sql(@"
        UPDATE b
        SET b.Date = g.BookingDate
        FROM BaseBookings b
        INNER JOIN GymBookings g ON b.Id = g.Id
    ");

            migrationBuilder.Sql(@"
        UPDATE b
        SET b.Date = t.BookingDate
        FROM BaseBookings b
        INNER JOIN TechnicianBookings t ON b.Id = t.Id
    ");

            migrationBuilder.Sql(@"
        UPDATE b
        SET b.Date = r.RideDate
        FROM BaseBookings b
        INNER JOIN Rides r ON b.Id = r.Id
    ");

            migrationBuilder.Sql(@"
        UPDATE b
        SET b.Date = CAST(d.bookingDate AS datetime2)
        FROM BaseBookings b
        INNER JOIN DoctorBookings d ON b.Id = d.Id
    ");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "TechnicianBookings");

            migrationBuilder.DropColumn(
                name: "RideDate",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "BookingDate",
                table: "GymBookings");

            migrationBuilder.DropColumn(
                name: "bookingDate",
                table: "DoctorBookings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "BaseBookings");

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "TechnicianBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RideDate",
                table: "Rides",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "BookingDate",
                table: "GymBookings",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateOnly>(
                name: "bookingDate",
                table: "DoctorBookings",
                type: "date",
                nullable: false,
                defaultValue: new DateOnly(1, 1, 1));
        }
    }
}
