using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changedNameForMinBoookingAmountToPrice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_EmployeeID",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "MinBookingAmount",
                table: "PromoCodes",
                newName: "MinBookingPrice");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EmployeeID_StartTime_EndTime",
                table: "Bookings",
                columns: new[] { "EmployeeID", "StartTime", "EndTime" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Bookings_EmployeeID_StartTime_EndTime",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "MinBookingPrice",
                table: "PromoCodes",
                newName: "MinBookingAmount");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_EmployeeID",
                table: "Bookings",
                column: "EmployeeID");
        }
    }
}
