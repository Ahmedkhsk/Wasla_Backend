using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateRestaurantTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CuisineType",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "DeliveryAvailable",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "DeliveryFee",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "MenuItemsJson",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "MinOrderValue",
                table: "Restaurant");

            migrationBuilder.RenameColumn(
                name: "PaymentMethods",
                table: "Restaurant",
                newName: "gallery");

            migrationBuilder.RenameColumn(
                name: "AverageDeliveryTime",
                table: "Restaurant",
                newName: "numberOfTables");

            migrationBuilder.AddColumn<int>(
                name: "numberOfPersons",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numberOfPersons",
                table: "Restaurant");

            migrationBuilder.RenameColumn(
                name: "numberOfTables",
                table: "Restaurant",
                newName: "AverageDeliveryTime");

            migrationBuilder.RenameColumn(
                name: "gallery",
                table: "Restaurant",
                newName: "PaymentMethods");

            migrationBuilder.AddColumn<string>(
                name: "CuisineType",
                table: "Restaurant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "DeliveryAvailable",
                table: "Restaurant",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "DeliveryFee",
                table: "Restaurant",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "MenuItemsJson",
                table: "Restaurant",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MinOrderValue",
                table: "Restaurant",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
