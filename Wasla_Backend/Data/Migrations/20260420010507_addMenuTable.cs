using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class addMenuTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MenuItemCategories",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_English = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_Arabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    restaurantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItemCategories", x => x.id);
                    table.ForeignKey(
                        name: "FK_MenuItemCategories_Restaurant_restaurantId",
                        column: x => x.restaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MenuItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name_English = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name_Arabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    discountPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    imageUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isAvailable = table.Column<bool>(type: "bit", nullable: false),
                    preparationTime = table.Column<int>(type: "int", nullable: true),
                    restaurantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    categoryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MenuItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_MenuItems_MenuItemCategories_categoryId",
                        column: x => x.categoryId,
                        principalTable: "MenuItemCategories",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "FK_MenuItems_Restaurant_restaurantId",
                        column: x => x.restaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MenuItemCategories_restaurantId",
                table: "MenuItemCategories",
                column: "restaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_categoryId",
                table: "MenuItems",
                column: "categoryId");

            migrationBuilder.CreateIndex(
                name: "IX_MenuItems_restaurantId",
                table: "MenuItems",
                column: "restaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MenuItems");

            migrationBuilder.DropTable(
                name: "MenuItemCategories");
        }
    }
}
