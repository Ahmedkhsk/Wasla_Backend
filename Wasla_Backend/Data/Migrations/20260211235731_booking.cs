using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class booking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "BaseBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceProviderId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ResidentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BaseBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BaseBookings_AspNetUsers_ResidentId",
                        column: x => x.ResidentId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BaseBookings_AspNetUsers_ServiceProviderId",
                        column: x => x.ServiceProviderId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GymBookings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<int>(type: "int", nullable: false),
                    BookingStatus = table.Column<int>(type: "int", nullable: false),
                    BookingDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GymBookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GymBookings_BaseBookings_Id",
                        column: x => x.Id,
                        principalTable: "BaseBookings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GymBookings_BaseServices_ServiceId",
                        column: x => x.ServiceId,
                        principalTable: "BaseServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BaseBookings_ResidentId",
                table: "BaseBookings",
                column: "ResidentId");

            migrationBuilder.CreateIndex(
                name: "IX_BaseBookings_ServiceProviderId",
                table: "BaseBookings",
                column: "ServiceProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_GymBookings_ServiceId",
                table: "GymBookings",
                column: "ServiceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GymBookings");

            migrationBuilder.DropTable(
                name: "BaseBookings");
        }
    }
}
