using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    public partial class updateSocial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Posts_AspNetUsers_userId",
                table: "Posts");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Posts",
                table: "Posts");

            migrationBuilder.RenameTable(
                name: "Posts",
                newName: "Socials");

            migrationBuilder.RenameIndex(
                name: "IX_Posts_userId",
                table: "Socials",
                newName: "IX_Socials_userId");

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "Socials",
                type: "nvarchar(8)",
                maxLength: 8,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "file",
                table: "Socials",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "postId",
                table: "Socials",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Socials",
                table: "Socials",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_Reports_targetId",
                table: "Reports",
                column: "targetId");

            migrationBuilder.CreateIndex(
                name: "IX_Reactions_targetId",
                table: "Reactions",
                column: "targetId");

            migrationBuilder.CreateIndex(
                name: "IX_Socials_postId",
                table: "Socials",
                column: "postId");

            // Reactions → Socials: NO ACTION (avoid cycle)
            migrationBuilder.AddForeignKey(
                name: "FK_Reactions_Socials_targetId",
                table: "Reactions",
                column: "targetId",
                principalTable: "Socials",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            // Reports → Socials: NO ACTION (avoid cycle)
            migrationBuilder.AddForeignKey(
                name: "FK_Reports_Socials_targetId",
                table: "Reports",
                column: "targetId",
                principalTable: "Socials",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);

            // User → Socials: NO ACTION (avoid cycle)
            migrationBuilder.AddForeignKey(
                name: "FK_Socials_AspNetUsers_userId",
                table: "Socials",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            // Comment → Post (self-ref): NO ACTION (avoid cycle)
            migrationBuilder.AddForeignKey(
                name: "FK_Socials_Socials_postId",
                table: "Socials",
                column: "postId",
                principalTable: "Socials",
                principalColumn: "id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reactions_Socials_targetId",
                table: "Reactions");

            migrationBuilder.DropForeignKey(
                name: "FK_Reports_Socials_targetId",
                table: "Reports");

            migrationBuilder.DropForeignKey(
                name: "FK_Socials_AspNetUsers_userId",
                table: "Socials");

            migrationBuilder.DropForeignKey(
                name: "FK_Socials_Socials_postId",
                table: "Socials");

            migrationBuilder.DropIndex(name: "IX_Reports_targetId", table: "Reports");
            migrationBuilder.DropIndex(name: "IX_Reactions_targetId", table: "Reactions");
            migrationBuilder.DropPrimaryKey(name: "PK_Socials", table: "Socials");
            migrationBuilder.DropIndex(name: "IX_Socials_postId", table: "Socials");

            migrationBuilder.DropColumn(name: "Discriminator", table: "Socials");
            migrationBuilder.DropColumn(name: "file", table: "Socials");
            migrationBuilder.DropColumn(name: "postId", table: "Socials");

            migrationBuilder.RenameTable(name: "Socials", newName: "Posts");
            migrationBuilder.RenameIndex(
                name: "IX_Socials_userId",
                table: "Posts",
                newName: "IX_Posts_userId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Posts",
                table: "Posts",
                column: "id");

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    postId = table.Column<int>(type: "int", nullable: false),
                    userId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    createdAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    file = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isHidden = table.Column<bool>(type: "bit", nullable: false),
                    updatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.id);
                    table.ForeignKey(
                        name: "FK_Comments_AspNetUsers_userId",
                        column: x => x.userId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_postId",
                        column: x => x.postId,
                        principalTable: "Posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(name: "IX_Comments_postId", table: "Comments", column: "postId");
            migrationBuilder.CreateIndex(name: "IX_Comments_userId", table: "Comments", column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_AspNetUsers_userId",
                table: "Posts",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}