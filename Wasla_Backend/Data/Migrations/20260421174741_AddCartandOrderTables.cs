using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class AddCartandOrderTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    residentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    restaurantId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.id);
                    table.ForeignKey(
                        name: "FK_Carts_Resident_residentId",
                        column: x => x.residentId,
                        principalTable: "Resident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Carts_Restaurant_restaurantId",
                        column: x => x.restaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    residentId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    restaurantId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    totalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    deliveryFee = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    paymobOrderId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paymentKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    transactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false),
                    paymentStatus = table.Column<int>(type: "int", nullable: false),
                    paymentMethod = table.Column<int>(type: "int", nullable: false),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.id);
                    table.ForeignKey(
                        name: "FK_Orders_Resident_residentId",
                        column: x => x.residentId,
                        principalTable: "Resident",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Orders_Restaurant_restaurantId",
                        column: x => x.restaurantId,
                        principalTable: "Restaurant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CartItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cartId = table.Column<int>(type: "int", nullable: false),
                    menuItemId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_CartItems_Carts_cartId",
                        column: x => x.cartId,
                        principalTable: "Carts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CartItems_MenuItems_menuItemId",
                        column: x => x.menuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "OrderItems",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    orderId = table.Column<int>(type: "int", nullable: false),
                    menuItemId = table.Column<int>(type: "int", nullable: false),
                    quantity = table.Column<int>(type: "int", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderItems", x => x.id);
                    table.ForeignKey(
                        name: "FK_OrderItems_MenuItems_menuItemId",
                        column: x => x.menuItemId,
                        principalTable: "MenuItems",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_OrderItems_Orders_orderId",
                        column: x => x.orderId,
                        principalTable: "Orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_cartId",
                table: "CartItems",
                column: "cartId");

            migrationBuilder.CreateIndex(
                name: "IX_CartItems_menuItemId",
                table: "CartItems",
                column: "menuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_residentId",
                table: "Carts",
                column: "residentId");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_restaurantId",
                table: "Carts",
                column: "restaurantId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_menuItemId",
                table: "OrderItems",
                column: "menuItemId");

            migrationBuilder.CreateIndex(
                name: "IX_OrderItems_orderId",
                table: "OrderItems",
                column: "orderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_residentId",
                table: "Orders",
                column: "residentId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_restaurantId",
                table: "Orders",
                column: "restaurantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CartItems");

            migrationBuilder.DropTable(
                name: "OrderItems");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "Orders");
        }
    }
}
