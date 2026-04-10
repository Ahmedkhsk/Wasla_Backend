using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateRestaurantTableSetNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_RestaurantCategories_restaurantCategoryId",
                table: "Restaurant");

            migrationBuilder.AlterColumn<int>(
                name: "restaurantCategoryId",
                table: "Restaurant",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "numberOfTables",
                table: "Restaurant",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "numberOfPersons",
                table: "Restaurant",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_RestaurantCategories_restaurantCategoryId",
                table: "Restaurant",
                column: "restaurantCategoryId",
                principalTable: "RestaurantCategories",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_RestaurantCategories_restaurantCategoryId",
                table: "Restaurant");

            migrationBuilder.AlterColumn<int>(
                name: "restaurantCategoryId",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "numberOfTables",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "numberOfPersons",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_RestaurantCategories_restaurantCategoryId",
                table: "Restaurant",
                column: "restaurantCategoryId",
                principalTable: "RestaurantCategories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
