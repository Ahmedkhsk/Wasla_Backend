using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class updateDB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_ServiceProvider_Id",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorBookings_AspNetUsers_userId",
                table: "DoctorBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorServices_Doctor_doctorId",
                table: "DoctorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_ServiceProvider_Id",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Gym_ServiceProvider_Id",
                table: "Gym");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_ServiceProvider_Id",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_ServiceProvider_ServiceProviderId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_ServiceProvider_AspNetUsers_Id",
                table: "ServiceProvider");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_ServiceProvider_Id",
                table: "Technicians");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_ServiceProvider_serviceProviderId",
                table: "UserEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ServiceProvider",
                table: "ServiceProvider");

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

            migrationBuilder.RenameTable(
                name: "ServiceProvider",
                newName: "serviceProvider");

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_serviceProvider",
                table: "serviceProvider",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_serviceProvider_Id",
                table: "Doctor",
                column: "Id",
                principalTable: "serviceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_serviceProvider_Id",
                table: "Driver",
                column: "Id",
                principalTable: "serviceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gym_serviceProvider_Id",
                table: "Gym",
                column: "Id",
                principalTable: "serviceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_serviceProvider_Id",
                table: "Restaurant",
                column: "Id",
                principalTable: "serviceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_serviceProvider_ServiceProviderId",
                table: "Review",
                column: "ServiceProviderId",
                principalTable: "serviceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_serviceProvider_AspNetUsers_Id",
                table: "serviceProvider",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_serviceProvider_Id",
                table: "Technicians",
                column: "Id",
                principalTable: "serviceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_serviceProvider_serviceProviderId",
                table: "UserEvents",
                column: "serviceProviderId",
                principalTable: "serviceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_serviceProvider_Id",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorBookings_BaseBookings_Id",
                table: "DoctorBookings");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorServices_BaseServices_Id",
                table: "DoctorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_DoctorServices_Doctor_DoctorId",
                table: "DoctorServices");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_serviceProvider_Id",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Gym_serviceProvider_Id",
                table: "Gym");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_serviceProvider_Id",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_serviceProvider_ServiceProviderId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_serviceProvider_AspNetUsers_Id",
                table: "serviceProvider");

            migrationBuilder.DropForeignKey(
                name: "FK_Technicians_serviceProvider_Id",
                table: "Technicians");

            migrationBuilder.DropForeignKey(
                name: "FK_UserEvents_serviceProvider_serviceProviderId",
                table: "UserEvents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_serviceProvider",
                table: "serviceProvider");

            migrationBuilder.RenameTable(
                name: "serviceProvider",
                newName: "ServiceProvider");

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

            migrationBuilder.AddColumn<bool>(
                name: "isDelete",
                table: "DoctorServices",
                type: "bit",
                nullable: false,
                defaultValue: false);

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

            migrationBuilder.AddPrimaryKey(
                name: "PK_ServiceProvider",
                table: "ServiceProvider",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorBookings_userId",
                table: "DoctorBookings",
                column: "userId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_ServiceProvider_Id",
                table: "Doctor",
                column: "Id",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Driver_ServiceProvider_Id",
                table: "Driver",
                column: "Id",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Gym_ServiceProvider_Id",
                table: "Gym",
                column: "Id",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Restaurant_ServiceProvider_Id",
                table: "Restaurant",
                column: "Id",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ServiceProvider_ServiceProviderId",
                table: "Review",
                column: "ServiceProviderId",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ServiceProvider_AspNetUsers_Id",
                table: "ServiceProvider",
                column: "Id",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Technicians_ServiceProvider_Id",
                table: "Technicians",
                column: "Id",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_ServiceProvider_serviceProviderId",
                table: "UserEvents",
                column: "serviceProviderId",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
