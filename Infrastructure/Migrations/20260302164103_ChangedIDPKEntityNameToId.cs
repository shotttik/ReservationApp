using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIDPKEntityNameToId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ID",
                table: "WorkSchedules",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "WorkScheduleExceptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UserLoginDatas",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "UserAccounts",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "SubscriptionPlans",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "States",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Services",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Reviews",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "ReviewInvites",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Permissions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Media",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Countries",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CompanySubscriptions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CompanyInvitations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CompanyFAQs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CompanyFAQCategories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Companies",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Cities",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Branches",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "BookingVerifications",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "Bookings",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "BookingGuestInfos",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkSchedules",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "WorkScheduleExceptions",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserLoginDatas",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "UserAccounts",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "SubscriptionPlans",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "States",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Services",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Reviews",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ReviewInvites",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Permissions",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Media",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Countries",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompanySubscriptions",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompanyInvitations",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompanyFAQs",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompanyFAQCategories",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Companies",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Cities",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Branches",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BookingVerifications",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Bookings",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "BookingGuestInfos",
                newName: "ID");
        }
    }
}
