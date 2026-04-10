using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class AddRestaurantCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "restaurantCategoryId",
                table: "Restaurant",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RestaurantCategories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RestaurantCategories", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Restaurant_restaurantCategoryId",
                table: "Restaurant",
                column: "restaurantCategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_RestaurantCategories_restaurantCategoryId",
                table: "Restaurant",
                column: "restaurantCategoryId",
                principalTable: "RestaurantCategories",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_RestaurantCategories_restaurantCategoryId",
                table: "Restaurant");

            migrationBuilder.DropTable(
                name: "RestaurantCategories");

            migrationBuilder.DropIndex(
                name: "IX_Restaurant_restaurantCategoryId",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "restaurantCategoryId",
                table: "Restaurant");
        }
    }
}
