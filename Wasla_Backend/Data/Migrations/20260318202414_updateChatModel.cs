using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateChatModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "deletedByReceiverId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "deletedBySenderId",
                table: "Chats",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "receiverDeletedAt",
                table: "Chats",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "senderDeletedAt",
                table: "Chats",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "deletedByReceiverId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "deletedBySenderId",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "receiverDeletedAt",
                table: "Chats");

            migrationBuilder.DropColumn(
                name: "senderDeletedAt",
                table: "Chats");
        }
    }
}
