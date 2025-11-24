using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTotalReviewsFromDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalReviews",
                table: "Doctor");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalReviews",
                table: "Doctor",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

    }
}
