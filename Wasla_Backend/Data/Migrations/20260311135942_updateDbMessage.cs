using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateDbMessage : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isEdited",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "senderId",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "receiverId",
                table: "Chats",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_receiverId",
                table: "Chats",
                column: "receiverId");

            migrationBuilder.CreateIndex(
                name: "IX_Chats_senderId",
                table: "Chats",
                column: "senderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_receiverId",
                table: "Chats",
                column: "receiverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Chats_AspNetUsers_senderId",
                table: "Chats",
                column: "senderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_receiverId",
                table: "Chats");

            migrationBuilder.DropForeignKey(
                name: "FK_Chats_AspNetUsers_senderId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_receiverId",
                table: "Chats");

            migrationBuilder.DropIndex(
                name: "IX_Chats_senderId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "isEdited",
                table: "Messages");

            migrationBuilder.AlterColumn<string>(
                name: "senderId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "receiverId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}