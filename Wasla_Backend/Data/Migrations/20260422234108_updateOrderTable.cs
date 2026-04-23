using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateOrderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BookingId",
                table: "Payments",
                newName: "entityType");

            migrationBuilder.AddColumn<int>(
                name: "entityId",
                table: "Payments",
                type: "int",
                nullable: false,
                defaultValue: 0);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "entityId",
                table: "Payments");

            migrationBuilder.RenameColumn(
                name: "entityType",
                table: "Payments",
                newName: "BookingId");
        }
    }
}
