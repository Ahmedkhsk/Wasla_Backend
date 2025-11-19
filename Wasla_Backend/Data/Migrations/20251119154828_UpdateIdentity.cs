using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Gmail",
                table: "residentIdentities",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Gmail",
                table: "residentIdentities");
        }
    }
}
