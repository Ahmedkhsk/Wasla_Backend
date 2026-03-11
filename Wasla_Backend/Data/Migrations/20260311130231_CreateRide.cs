using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class CreateRide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    PickupLatitude = table.Column<double>(type: "float", nullable: false),
                    PickupLongitude = table.Column<double>(type: "float", nullable: false),
                    DropoffLatitude = table.Column<double>(type: "float", nullable: false),
                    DropoffLongitude = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    RideDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rides_BaseBookings_Id",
                        column: x => x.Id,
                        principalTable: "BaseBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Rides_Driver_DriverId",
                        column: x => x.DriverId,
                        principalTable: "Driver",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriverId",
                table: "Rides",
                column: "DriverId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rides");
        }
    }
}
