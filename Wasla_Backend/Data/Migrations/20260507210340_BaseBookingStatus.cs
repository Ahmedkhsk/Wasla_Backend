using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class BaseBookingStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "baseBookingStatus",
                table: "BaseBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            // Doctor Bookings
            // upcoming(1) → Pending(0)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 0
                FROM BaseBookings b
                INNER JOIN DoctorBookings d ON b.Id = d.Id
                WHERE d.bookingStatus = 1");

            // completed(2) → done(1)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 1
                FROM BaseBookings b
                INNER JOIN DoctorBookings d ON b.Id = d.Id
                WHERE d.bookingStatus = 2");

            // canceled(3) → Canceled(2)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 2
                FROM BaseBookings b
                INNER JOIN DoctorBookings d ON b.Id = d.Id
                WHERE d.bookingStatus = 3");

            // Driver Rides
            // Pending(0), Accepted(1), InProgress(2) → Pending(0)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 0
                FROM BaseBookings b
                INNER JOIN Rides r ON b.Id = r.Id
                WHERE r.Status IN (0, 1, 2)");

            // Completed(3) → done(1)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 1
                FROM BaseBookings b
                INNER JOIN Rides r ON b.Id = r.Id
                WHERE r.Status = 3");

            // Cancelled(4) → Canceled(2)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 2
                FROM BaseBookings b
                INNER JOIN Rides r ON b.Id = r.Id
                WHERE r.Status = 4");

            // Gym Bookings
            // PaymentPending(3) → Pending(0)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 0
                FROM BaseBookings b
                INNER JOIN GymBookings g ON b.Id = g.Id
                WHERE g.BookingStatus = 3");

            // Active(0), Completed(1) → done(1)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 1
                FROM BaseBookings b
                INNER JOIN GymBookings g ON b.Id = g.Id
                WHERE g.BookingStatus IN (0, 1)");

            // Cancelled(2) → Canceled(2)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 2
                FROM BaseBookings b
                INNER JOIN GymBookings g ON b.Id = g.Id
                WHERE g.BookingStatus = 2");

            // Technician Bookings
            // Pending(1), Accepted(2) → Pending(0)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 0
                FROM BaseBookings b
                INNER JOIN TechnicianBookings t ON b.Id = t.Id
                WHERE t.Status IN (1, 2)");

            // Done(5) → done(1)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 1
                FROM BaseBookings b
                INNER JOIN TechnicianBookings t ON b.Id = t.Id
                WHERE t.Status = 5");

            // Rejected(3), Cancelled(4) → Canceled(2)
            migrationBuilder.Sql(@"
                UPDATE b SET b.baseBookingStatus = 2
                FROM BaseBookings b
                INNER JOIN TechnicianBookings t ON b.Id = t.Id
                WHERE t.Status IN (3, 4)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "baseBookingStatus",
                table: "BaseBookings");
        }
    }
}