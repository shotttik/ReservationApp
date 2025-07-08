using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedNecesseryPermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 8, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 37, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 38, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 39, 1 });

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
                keyValues: new object[] { 6, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 24, 2 });

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
                keyValues: new object[] { 11, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 18, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 19, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 35, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 37, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 38, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 39, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 19, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 20, 4 });

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
                keyValues: new object[] { 28, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 4 });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 8,
                column: "Name",
                value: "CompanyUpdatePartial");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 9,
                column: "Name",
                value: "CompanyUpdateFull");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 10,
                column: "Name",
                value: "CompanyDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 11,
                column: "Name",
                value: "CompanyReadAll");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 12,
                column: "Name",
                value: "CompanyReadOwn");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 13,
                column: "Name",
                value: "CompanyReadLimited");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 14,
                column: "Name",
                value: "CompanyMemberCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 15,
                column: "Name",
                value: "CompanyMemberRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 16,
                column: "Name",
                value: "CompanyMemberUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 17,
                column: "Name",
                value: "CompanyMemberDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 18,
                column: "Name",
                value: "ReportView");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 19,
                column: "Name",
                value: "SettingsManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 20,
                column: "Name",
                value: "WorkScheduleCompanyRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 21,
                column: "Name",
                value: "WorkScheduleCompanyCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 22,
                column: "Name",
                value: "WorkScheduleCompanyUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 23,
                column: "Name",
                value: "WorkScheduleCompanyExceptionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 24,
                column: "Name",
                value: "WorkScheduleUserRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 25,
                column: "Name",
                value: "WorkScheduleUserCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 26,
                column: "Name",
                value: "WorkScheduleUserUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 27,
                column: "Name",
                value: "WorkScheduleUserExceptionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 28,
                column: "Name",
                value: "ServiceCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 29,
                column: "Name",
                value: "ServiceRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 30,
                column: "Name",
                value: "ServiceUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 31,
                column: "Name",
                value: "ServiceDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 32,
                column: "Name",
                value: "AppointmentSchedule");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 33,
                column: "Name",
                value: "AppointmentRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 34,
                column: "Name",
                value: "AppointmentUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 35,
                column: "Name",
                value: "AppointmentCancel");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 36,
                column: "Name",
                value: "AppointmentApprove");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 37,
                column: "Name",
                value: "MediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 38,
                column: "Name",
                value: "MediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 39,
                column: "Name",
                value: "MediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 40,
                column: "Name",
                value: "CompanyMediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 41,
                column: "Name",
                value: "CompanyMediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 42,
                column: "Name",
                value: "CompanyMediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 43,
                column: "Name",
                value: "CompanyMediaUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 44,
                column: "Name",
                value: "FaqCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 45,
                column: "Name",
                value: "FaqRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 46,
                column: "Name",
                value: "FaqUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 47,
                column: "Name",
                value: "FaqDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 48,
                column: "Name",
                value: "FaqCategoryCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 49,
                column: "Name",
                value: "FaqCategoryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 50,
                column: "Name",
                value: "FaqCategoryUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 51,
                column: "Name",
                value: "FaqCategoryDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 52,
                column: "Name",
                value: "InvitationSend");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 53,
                column: "Name",
                value: "InvitationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 54,
                column: "Name",
                value: "InvitationRevoke");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 55,
                column: "Name",
                value: "LocationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 56,
                column: "Name",
                value: "CityRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 57,
                column: "Name",
                value: "StateRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 58,
                column: "Name",
                value: "CountryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 59,
                column: "Name",
                value: "RoleCreate");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 60, "RoleRead" },
                    { 61, "RoleUpdate" },
                    { 62, "RoleDelete" },
                    { 63, "RolePermissionManage" },
                    { 64, "UserLoginDataRead" },
                    { 65, "UserLoginDataManage" }
                });

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
                    { 59, 1 },
                    { 29, 2 },
                    { 32, 2 },
                    { 33, 2 },
                    { 13, 3 },
                    { 14, 3 },
                    { 27, 3 },
                    { 29, 3 },
                    { 30, 3 },
                    { 33, 3 },
                    { 49, 3 },
                    { 50, 3 },
                    { 51, 3 },
                    { 52, 3 },
                    { 53, 3 },
                    { 54, 3 },
                    { 25, 4 },
                    { 26, 4 },
                    { 29, 4 },
                    { 32, 4 },
                    { 33, 4 },
                    { 35, 4 },
                    { 60, 1 },
                    { 61, 1 },
                    { 62, 1 },
                    { 63, 1 }
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
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 65);

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
                keyValues: new object[] { 59, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 60, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 61, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 62, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 63, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 29, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 32, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 33, 2 });

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
                keyValues: new object[] { 29, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 33, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 49, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 50, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 51, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 52, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 53, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 54, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 25, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 26, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 29, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 32, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 33, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 35, 4 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 60);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 61);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 62);

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 63);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 8,
                column: "Name",
                value: "CompanyDelete");

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

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 56,
                column: "Name",
                value: "RoleDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 57,
                column: "Name",
                value: "RolePermissionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 58,
                column: "Name",
                value: "UserLoginDataRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 59,
                column: "Name",
                value: "UserLoginDataManage");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 8, 1 },
                    { 37, 1 },
                    { 38, 1 },
                    { 39, 1 },
                    { 55, 1 },
                    { 56, 1 },
                    { 57, 1 },
                    { 6, 2 },
                    { 24, 2 },
                    { 27, 2 },
                    { 28, 2 },
                    { 9, 3 },
                    { 11, 3 },
                    { 18, 3 },
                    { 19, 3 },
                    { 35, 3 },
                    { 37, 3 },
                    { 38, 3 },
                    { 39, 3 },
                    { 19, 4 },
                    { 20, 4 },
                    { 21, 4 },
                    { 22, 4 },
                    { 28, 4 },
                    { 30, 4 }
                });
        }
    }
}
