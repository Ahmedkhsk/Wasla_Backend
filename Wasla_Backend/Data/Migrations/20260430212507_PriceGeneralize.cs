using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class PriceGeneralize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "BaseBookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);


            migrationBuilder.Sql(@"
        UPDATE b
        SET b.price = g.price
        FROM BaseBookings b
        INNER JOIN GymBookings g ON b.Id = g.Id
    ");

            migrationBuilder.Sql(@"
        UPDATE b
        SET b.price = d.price
        FROM BaseBookings b
        INNER JOIN DoctorBookings d ON b.Id = d.Id
    ");

            migrationBuilder.Sql(@"
        UPDATE b
        SET b.price = t.Price
        FROM BaseBookings b
        INNER JOIN TechnicianBookings t ON b.Id = t.Id
    ");

            migrationBuilder.Sql(@"
        UPDATE b
        SET b.price = r.Price
        FROM BaseBookings b
        INNER JOIN Rides r ON b.Id = r.Id
    ");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "TechnicianBookings");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "price",
                table: "GymBookings");

            migrationBuilder.DropColumn(
                name: "price",
                table: "DoctorBookings");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "BaseBookings");

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "TechnicianBookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Price",
                table: "Rides",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "GymBookings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<double>(
                name: "price",
                table: "DoctorBookings",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
