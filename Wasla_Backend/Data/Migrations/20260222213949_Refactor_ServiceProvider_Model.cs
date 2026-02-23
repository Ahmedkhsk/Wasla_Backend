using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wasla_Backend.data
{
    /// <inheritdoc />
    public partial class Refactor_ServiceProvider_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            /* ===============================
             * 1️⃣ فك العلاقات القديمة
             * =============================== */

            migrationBuilder.DropForeignKey(
                name: "FK_Doctor_AspNetUsers_Id",
                table: "Doctor");

            migrationBuilder.DropForeignKey(
                name: "FK_Driver_AspNetUsers_Id",
                table: "Driver");

            migrationBuilder.DropForeignKey(
                name: "FK_Gym_AspNetUsers_Id",
                table: "Gym");

            migrationBuilder.DropForeignKey(
                name: "FK_Restaurant_AspNetUsers_Id",
                table: "Restaurant");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Doctor_DoctorId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Driver_DriverId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Gym_GymId",
                table: "Review");

            migrationBuilder.DropForeignKey(
                name: "FK_Review_Restaurant_RestaurantId",
                table: "Review");

            /* ===============================
             * 2️⃣ حذف الأعمدة القديمة من Review
             * =============================== */

            migrationBuilder.DropIndex(name: "IX_Review_DoctorId", table: "Review");
            migrationBuilder.DropIndex(name: "IX_Review_DriverId", table: "Review");
            migrationBuilder.DropIndex(name: "IX_Review_GymId", table: "Review");
            migrationBuilder.DropIndex(name: "IX_Review_RestaurantId", table: "Review");

            migrationBuilder.DropColumn(name: "DoctorId", table: "Review");
            migrationBuilder.DropColumn(name: "DriverId", table: "Review");
            migrationBuilder.DropColumn(name: "GymId", table: "Review");
            migrationBuilder.DropColumn(name: "RestaurantId", table: "Review");

            /* ===============================
             * 3️⃣ حذف الأعمدة المشتركة من الجداول
             * =============================== */

            string[] sharedColumns =
            {
                "BusinessName","CV","Description","OpeningHours","OwnerName","Rating"
            };

            foreach (var col in sharedColumns)
            {
                migrationBuilder.DropColumn(col, "Doctor");
                migrationBuilder.DropColumn(col, "Driver");
                migrationBuilder.DropColumn(col, "Gym");
                migrationBuilder.DropColumn(col, "Restaurant");
            }

            /* ===============================
             * 4️⃣ تعديل نوع ServiceProviderId
             * =============================== */

            migrationBuilder.AlterColumn<string>(
                name: "ServiceProviderId",
                table: "Review",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "serviceProviderId",
                table: "UserEvents",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            /* ===============================
             * 5️⃣ إنشاء جدول ServiceProvider
             * =============================== */

            migrationBuilder.CreateTable(
                name: "ServiceProvider",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    BusinessName = table.Column<string>(nullable: true),
                    OwnerName = table.Column<string>(nullable: true),
                    CV = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OpeningHours = table.Column<string>(nullable: true),
                    Rating = table.Column<float>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceProvider", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServiceProvider_AspNetUsers_Id",
                        column: x => x.Id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            /* ===============================
             * 6️⃣ ربط TPT (Cascade)
             * =============================== */

            migrationBuilder.AddForeignKey(
                name: "FK_Doctor_ServiceProvider_Id",
                table: "Doctor",
                column: "Id",
                principalTable: "ServiceProvider",
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

            /* ===============================
             * 7️⃣ Review → ServiceProvider (❗ Restrict)
             * =============================== */

            migrationBuilder.CreateIndex(
                name: "IX_Review_ServiceProviderId",
                table: "Review",
                column: "ServiceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_ServiceProvider_ServiceProviderId",
                table: "Review",
                column: "ServiceProviderId",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            /* ===============================
             * 8️⃣ UserEvents → ServiceProvider
             * =============================== */

            migrationBuilder.CreateIndex(
                name: "IX_UserEvents_serviceProviderId",
                table: "UserEvents",
                column: "serviceProviderId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEvents_ServiceProvider_serviceProviderId",
                table: "UserEvents",
                column: "serviceProviderId",
                principalTable: "ServiceProvider",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "ServiceProvider");
        }
    }
}
