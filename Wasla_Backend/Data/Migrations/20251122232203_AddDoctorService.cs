using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableDays",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "AvgConsultationTime",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "ClinicPhotos",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "ConsultationFee",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "Education",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "InsuranceSupported",
                table: "Doctor");

            migrationBuilder.DropColumn(
                name: "LicenseNumber",
                table: "Doctor");

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    serviceName_English = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    serviceName_Arabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description_English = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    description_Arabic = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    doctorId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.id);
                    table.ForeignKey(
                        name: "FK_Service_Doctor_doctorId",
                        column: x => x.doctorId,
                        principalTable: "Doctor",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceDate",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    date = table.Column<DateOnly>(type: "date", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDate", x => x.id);
                    table.ForeignKey(
                        name: "FK_ServiceDate_Service_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Service",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "ServiceDay",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dayOfWeek = table.Column<int>(type: "int", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceDay", x => x.id);
                    table.ForeignKey(
                        name: "FK_ServiceDay_Service_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Service",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "TimeSlot",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    start = table.Column<TimeSpan>(type: "time", nullable: false),
                    end = table.Column<TimeSpan>(type: "time", nullable: false),
                    serviceId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSlot", x => x.id);
                    table.ForeignKey(
                        name: "FK_TimeSlot_Service_serviceId",
                        column: x => x.serviceId,
                        principalTable: "Service",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Service_doctorId",
                table: "Service",
                column: "doctorId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDate_serviceId",
                table: "ServiceDate",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceDay_serviceId",
                table: "ServiceDay",
                column: "serviceId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_serviceId",
                table: "TimeSlot",
                column: "serviceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceDate");

            migrationBuilder.DropTable(
                name: "ServiceDay");

            migrationBuilder.DropTable(
                name: "TimeSlot");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.AddColumn<string>(
                name: "AvailableDays",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AvgConsultationTime",
                table: "Doctor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ClinicPhotos",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultationFee",
                table: "Doctor",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Education",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "InsuranceSupported",
                table: "Doctor",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "LicenseNumber",
                table: "Doctor",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
