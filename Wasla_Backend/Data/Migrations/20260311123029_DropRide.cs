using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class DropRide : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rides");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Rides",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DriverId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Distance = table.Column<double>(type: "float", nullable: false),
                    DropoffLatitude = table.Column<double>(type: "float", nullable: false),
                    DropoffLongitude = table.Column<double>(type: "float", nullable: false),
                    PickupLatitude = table.Column<double>(type: "float", nullable: false),
                    PickupLongitude = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    RideDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rides", x => x.Id);
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
    }
}
