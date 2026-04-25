using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class GenralizeDoctorServiceAndBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorBookings_AspNetUsers_userId",
                table: "DoctorBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorServices_Doctor_doctorId",
                table: "DoctorServices");

            migrationBuilder.DropIndex(
                name: "IX_DoctorBookings_userId",
                table: "DoctorBookings");

            migrationBuilder.DropColumn(
                name: "isDelete",
                table: "DoctorServices");

            migrationBuilder.DropColumn(
                name: "serviceProviderType",
                table: "DoctorBookings");

            migrationBuilder.DropColumn(
                name: "userId",
                table: "DoctorBookings");

            migrationBuilder.RenameColumn(
                name: "doctorId",
                table: "DoctorServices",
                newName: "DoctorId");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "DoctorServices",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorServices_doctorId",
                table: "DoctorServices",
                newName: "IX_DoctorServices_DoctorId");

            migrationBuilder.AlterColumn<string>(
                name: "DoctorId",
                table: "DoctorServices",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DoctorServices",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DoctorBookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .OldAnnotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorBookings_BaseBookings_Id",
                table: "DoctorBookings",
                column: "Id",
                principalTable: "BaseBookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorServices_BaseServices_Id",
                table: "DoctorServices",
                column: "Id",
                principalTable: "BaseServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorServices_Doctor_DoctorId",
                table: "DoctorServices",
                column: "DoctorId",
                principalTable: "Doctor",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DoctorBookings_BaseBookings_Id",
                table: "DoctorBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorServices_BaseServices_Id",
                table: "DoctorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorServices_Doctor_DoctorId",
                table: "DoctorServices");

            migrationBuilder.RenameColumn(
                name: "DoctorId",
                table: "DoctorServices",
                newName: "doctorId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "DoctorServices",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_DoctorServices_DoctorId",
                table: "DoctorServices",
                newName: "IX_DoctorServices_doctorId");

            migrationBuilder.AlterColumn<string>(
                name: "doctorId",
                table: "DoctorServices",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "id",
                table: "DoctorServices",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<bool>(
                name: "isDelete",
                table: "DoctorServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "DoctorBookings",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "serviceProviderType",
                table: "DoctorBookings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "userId",
                table: "DoctorBookings",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorBookings_userId",
                table: "DoctorBookings",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorBookings_AspNetUsers_userId",
                table: "DoctorBookings",
                column: "userId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_DoctorServices_Doctor_doctorId",
                table: "DoctorServices",
                column: "doctorId",
                principalTable: "Doctor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}