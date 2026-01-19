using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedNameOfServiceIdToServiceID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Services_ServiceId",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ServiceId",
                table: "Bookings",
                newName: "ServiceID");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ServiceId",
                table: "Bookings",
                newName: "IX_Bookings_ServiceID");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Services_ServiceID",
                table: "Bookings",
                column: "ServiceID",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Services_ServiceID",
                table: "Bookings");

            migrationBuilder.RenameColumn(
                name: "ServiceID",
                table: "Bookings",
                newName: "ServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_ServiceID",
                table: "Bookings",
                newName: "IX_Bookings_ServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Services_ServiceId",
                table: "Bookings",
                column: "ServiceId",
                principalTable: "Services",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
