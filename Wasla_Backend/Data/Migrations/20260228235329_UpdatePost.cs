using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class UpdatePost : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "files",
                table: "Posts",
                newName: "fileJson");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "fileJson",
                table: "Posts",
                newName: "files");
        }
    }
}
