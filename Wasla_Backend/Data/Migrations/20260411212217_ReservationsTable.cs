using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class ReservationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "numberOfPersons",
                table: "Restaurant");

            migrationBuilder.DropColumn(
                name: "numberOfTables",
                table: "Restaurant");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "RestaurantCategories",
                newName: "Name_English");

            migrationBuilder.AddColumn<string>(
                name: "Name_Arabic",
                table: "RestaurantCategories",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    numberOfPersons = table.Column<int>(type: "int", nullable: false),
                    day = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    date = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    time = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.id);
                    table.ForeignKey(
                        name: "FK_Reservations_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_userId",
                table: "Reservations",
                column: "userId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropColumn(
                name: "Name_Arabic",
                table: "RestaurantCategories");

            migrationBuilder.RenameColumn(
                name: "Name_English",
                table: "RestaurantCategories",
                newName: "name");

            migrationBuilder.AddColumn<int>(
                name: "numberOfPersons",
                table: "Restaurant",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "numberOfTables",
                table: "Restaurant",
                type: "int",
                nullable: true);
        }
    }
}
