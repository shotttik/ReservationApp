using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompanyMemberManagementPermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 32, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 33, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 34, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 49, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 50, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 51, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 52, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 20, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 23, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 13, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 14, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 27, 3 });

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
                keyValues: new object[] { 34, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 15, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 16, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 17, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 18, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 23, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 26, 4 });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 9,
                column: "Name",
                value: "CompanyMemberCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 10,
                column: "Name",
                value: "CompanyMemberRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 11,
                column: "Name",
                value: "CompanyMemberUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 12,
                column: "Name",
                value: "CompanyMemberDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 13,
                column: "Name",
                value: "ReportView");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 14,
                column: "Name",
                value: "SettingsManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 15,
                column: "Name",
                value: "WorkScheduleCompanyRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 16,
                column: "Name",
                value: "WorkScheduleCompanyCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 17,
                column: "Name",
                value: "WorkScheduleCompanyUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 18,
                column: "Name",
                value: "WorkScheduleCompanyExceptionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 19,
                column: "Name",
                value: "WorkScheduleUserRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 20,
                column: "Name",
                value: "WorkScheduleUserCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 21,
                column: "Name",
                value: "WorkScheduleUserUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 22,
                column: "Name",
                value: "WorkScheduleUserExceptionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 23,
                column: "Name",
                value: "ServiceCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 24,
                column: "Name",
                value: "ServiceRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 25,
                column: "Name",
                value: "ServiceUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 26,
                column: "Name",
                value: "ServiceDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 27,
                column: "Name",
                value: "AppointmentSchedule");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 28,
                column: "Name",
                value: "AppointmentRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 29,
                column: "Name",
                value: "AppointmentUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 30,
                column: "Name",
                value: "AppointmentCancel");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 31,
                column: "Name",
                value: "AppointmentApprove");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 32,
                column: "Name",
                value: "MediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 33,
                column: "Name",
                value: "MediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 34,
                column: "Name",
                value: "MediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 35,
                column: "Name",
                value: "CompanyMediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 36,
                column: "Name",
                value: "CompanyMediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 37,
                column: "Name",
                value: "CompanyMediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 38,
                column: "Name",
                value: "FaqCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 39,
                column: "Name",
                value: "FaqRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 40,
                column: "Name",
                value: "FaqUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 41,
                column: "Name",
                value: "FaqDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 42,
                column: "Name",
                value: "FaqCategoryCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 43,
                column: "Name",
                value: "FaqCategoryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 44,
                column: "Name",
                value: "FaqCategoryUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 45,
                column: "Name",
                value: "FaqCategoryDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 46,
                column: "Name",
                value: "InvitationSend");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 47,
                column: "Name",
                value: "InvitationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 48,
                column: "Name",
                value: "InvitationRevoke");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 49,
                column: "Name",
                value: "LocationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 50,
                column: "Name",
                value: "CityRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 51,
                column: "Name",
                value: "StateRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 52,
                column: "Name",
                value: "CountryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 53,
                column: "Name",
                value: "RoleCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 54,
                column: "Name",
                value: "RoleRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 55,
                column: "Name",
                value: "RoleUpdate");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 56, "RoleDelete" },
                    { 57, "RolePermissionManage" },
                    { 58, "UserLoginDataRead" },
                    { 59, "UserLoginDataManage" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 28, 1 },
                    { 29, 1 },
                    { 30, 1 },
                    { 45, 1 },
                    { 46, 1 },
                    { 47, 1 },
                    { 48, 1 },
                    { 54, 1 },
                    { 55, 1 },
                    { 27, 2 },
                    { 28, 2 },
                    { 9, 3 },
                    { 10, 3 },
                    { 23, 3 },
                    { 25, 3 },
                    { 26, 3 },
                    { 28, 3 },
                    { 45, 3 },
                    { 46, 3 },
                    { 47, 3 },
                    { 48, 3 },
                    { 19, 4 },
                    { 21, 4 },
                    { 22, 4 },
                    { 27, 4 },
                    { 28, 4 },
                    { 30, 4 },
                    { 56, 1 },
                    { 57, 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 58);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 59);

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 28, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 29, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 45, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 46, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 47, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 48, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 54, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 55, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 56, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 57, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 27, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 28, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 9, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 10, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 23, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 25, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 26, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 28, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 45, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 46, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 47, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 48, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 19, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 21, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 22, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 27, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 28, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 4 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 56);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 57);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 9,
                column: "Name",
                value: "ReportView");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 10,
                column: "Name",
                value: "SettingsManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 11,
                column: "Name",
                value: "WorkScheduleCompanyRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 12,
                column: "Name",
                value: "WorkScheduleCompanyCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 13,
                column: "Name",
                value: "WorkScheduleCompanyUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 14,
                column: "Name",
                value: "WorkScheduleCompanyExceptionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 15,
                column: "Name",
                value: "WorkScheduleUserRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 16,
                column: "Name",
                value: "WorkScheduleUserCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 17,
                column: "Name",
                value: "WorkScheduleUserUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 18,
                column: "Name",
                value: "WorkScheduleUserExceptionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 19,
                column: "Name",
                value: "ServiceCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 20,
                column: "Name",
                value: "ServiceRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 21,
                column: "Name",
                value: "ServiceUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 22,
                column: "Name",
                value: "ServiceDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 23,
                column: "Name",
                value: "AppointmentSchedule");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 24,
                column: "Name",
                value: "AppointmentRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 25,
                column: "Name",
                value: "AppointmentUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 26,
                column: "Name",
                value: "AppointmentCancel");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 27,
                column: "Name",
                value: "AppointmentApprove");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 28,
                column: "Name",
                value: "MediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 29,
                column: "Name",
                value: "MediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 30,
                column: "Name",
                value: "MediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 31,
                column: "Name",
                value: "CompanyMediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 32,
                column: "Name",
                value: "CompanyMediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 33,
                column: "Name",
                value: "CompanyMediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 34,
                column: "Name",
                value: "FaqCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 35,
                column: "Name",
                value: "FaqRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 36,
                column: "Name",
                value: "FaqUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 37,
                column: "Name",
                value: "FaqDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 38,
                column: "Name",
                value: "FaqCategoryCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 39,
                column: "Name",
                value: "FaqCategoryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 40,
                column: "Name",
                value: "FaqCategoryUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 41,
                column: "Name",
                value: "FaqCategoryDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 42,
                column: "Name",
                value: "InvitationSend");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 43,
                column: "Name",
                value: "InvitationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 44,
                column: "Name",
                value: "InvitationRevoke");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 45,
                column: "Name",
                value: "LocationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 46,
                column: "Name",
                value: "CityRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 47,
                column: "Name",
                value: "StateRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 48,
                column: "Name",
                value: "CountryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 49,
                column: "Name",
                value: "RoleCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 50,
                column: "Name",
                value: "RoleRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 51,
                column: "Name",
                value: "RoleUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 52,
                column: "Name",
                value: "RoleDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 53,
                column: "Name",
                value: "RolePermissionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 54,
                column: "Name",
                value: "UserLoginDataRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 55,
                column: "Name",
                value: "UserLoginDataManage");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 32, 1 },
                    { 33, 1 },
                    { 34, 1 },
                    { 49, 1 },
                    { 50, 1 },
                    { 51, 1 },
                    { 52, 1 },
                    { 20, 2 },
                    { 23, 2 },
                    { 13, 3 },
                    { 14, 3 },
                    { 27, 3 },
                    { 32, 3 },
                    { 33, 3 },
                    { 34, 3 },
                    { 15, 4 },
                    { 16, 4 },
                    { 17, 4 },
                    { 18, 4 },
                    { 23, 4 },
                    { 26, 4 }
                });
        }
    }
}
