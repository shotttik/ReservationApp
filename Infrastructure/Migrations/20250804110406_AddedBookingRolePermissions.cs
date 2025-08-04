using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedBookingRolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 38, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 57, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 13, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 38, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 33, 4 });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 30,
                column: "Name",
                value: "BookingCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 31,
                column: "Name",
                value: "BookingRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 32,
                column: "Name",
                value: "BookingUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 33,
                column: "Name",
                value: "BookingDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 34,
                column: "Name",
                value: "BookingCancel");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 35,
                column: "Name",
                value: "BookingApprove");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 36,
                column: "Name",
                value: "MediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 37,
                column: "Name",
                value: "MediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 38,
                column: "Name",
                value: "MediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 39,
                column: "Name",
                value: "CompanyMediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 40,
                column: "Name",
                value: "CompanyMediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 41,
                column: "Name",
                value: "CompanyMediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 42,
                column: "Name",
                value: "CompanyMediaUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 43,
                column: "Name",
                value: "FaqCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 44,
                column: "Name",
                value: "FaqRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 45,
                column: "Name",
                value: "FaqUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 46,
                column: "Name",
                value: "FaqDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 47,
                column: "Name",
                value: "FaqCategoryCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 48,
                column: "Name",
                value: "FaqCategoryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 49,
                column: "Name",
                value: "FaqCategoryUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 50,
                column: "Name",
                value: "FaqCategoryDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 51,
                column: "Name",
                value: "InvitationSend");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 52,
                column: "Name",
                value: "InvitationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 53,
                column: "Name",
                value: "InvitationRevoke");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 54,
                column: "Name",
                value: "LocationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 55,
                column: "Name",
                value: "CityRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 56,
                column: "Name",
                value: "StateRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 57,
                column: "Name",
                value: "CountryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 58,
                column: "Name",
                value: "RoleCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 59,
                column: "Name",
                value: "RoleRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 60,
                column: "Name",
                value: "RoleUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 61,
                column: "Name",
                value: "RoleDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 62,
                column: "Name",
                value: "RolePermissionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 63,
                column: "Name",
                value: "UserLoginDataRead");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ID", "Name" },
                values: new object[] { 64, "UserLoginDataManage" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 35, 1 },
                    { 53, 1 },
                    { 62, 1 },
                    { 13, 2 },
                    { 30, 3 },
                    { 32, 3 },
                    { 33, 3 },
                    { 35, 3 },
                    { 53, 3 },
                    { 34, 4 },
                    { 35, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 64);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 35, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 53, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 62, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 13, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 32, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 33, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 35, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 53, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 34, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 35, 4 });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 30,
                column: "Name",
                value: "AppointmentSchedule");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 31,
                column: "Name",
                value: "AppointmentRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 32,
                column: "Name",
                value: "AppointmentUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 33,
                column: "Name",
                value: "AppointmentCancel");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 34,
                column: "Name",
                value: "AppointmentApprove");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 35,
                column: "Name",
                value: "MediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 36,
                column: "Name",
                value: "MediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 37,
                column: "Name",
                value: "MediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 38,
                column: "Name",
                value: "CompanyMediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 39,
                column: "Name",
                value: "CompanyMediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 40,
                column: "Name",
                value: "CompanyMediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 41,
                column: "Name",
                value: "CompanyMediaUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 42,
                column: "Name",
                value: "FaqCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 43,
                column: "Name",
                value: "FaqRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 44,
                column: "Name",
                value: "FaqUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 45,
                column: "Name",
                value: "FaqDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 46,
                column: "Name",
                value: "FaqCategoryCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 47,
                column: "Name",
                value: "FaqCategoryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 48,
                column: "Name",
                value: "FaqCategoryUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 49,
                column: "Name",
                value: "FaqCategoryDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 50,
                column: "Name",
                value: "InvitationSend");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 51,
                column: "Name",
                value: "InvitationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 52,
                column: "Name",
                value: "InvitationRevoke");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 53,
                column: "Name",
                value: "LocationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 54,
                column: "Name",
                value: "CityRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 55,
                column: "Name",
                value: "StateRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 56,
                column: "Name",
                value: "CountryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 57,
                column: "Name",
                value: "RoleCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 58,
                column: "Name",
                value: "RoleRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 59,
                column: "Name",
                value: "RoleUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 60,
                column: "Name",
                value: "RoleDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 61,
                column: "Name",
                value: "RolePermissionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 62,
                column: "Name",
                value: "UserLoginDataRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 63,
                column: "Name",
                value: "UserLoginDataManage");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 38, 1 },
                    { 57, 1 },
                    { 30, 2 },
                    { 13, 3 },
                    { 38, 3 },
                    { 30, 4 },
                    { 33, 4 }
                });
        }
    }
}
