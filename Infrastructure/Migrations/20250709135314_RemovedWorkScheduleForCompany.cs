using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedWorkScheduleForCompany : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkScheduleExceptions_Companies_CompanyID",
                table: "WorkScheduleExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedules_Companies_CompanyID",
                table: "WorkSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedules_UserAccounts_UserID",
                table: "WorkSchedules");

            migrationBuilder.DropIndex(
                name: "IX_WorkSchedules_CompanyID",
                table: "WorkSchedules");

            migrationBuilder.DropIndex(
                name: "IX_WorkScheduleExceptions_CompanyID",
                table: "WorkScheduleExceptions");

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
                keyValues: new object[] { 23, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 34, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 35, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 36, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 52, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 53, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 54, 1 });

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
                keyValues: new object[] { 32, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 33, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 23, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 29, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 31, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 36, 3 });

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
                keyValues: new object[] { 27, 4 });

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
                keyValue: 63);

            migrationBuilder.DropColumn(
                name: "BreakEndTime",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "BreakStartTime",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "IsWorkingDay",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "CompanyID",
                table: "WorkScheduleExceptions");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "WorkSchedules",
                newName: "UserAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSchedules_UserID",
                table: "WorkSchedules",
                newName: "IX_WorkSchedules_UserAccountID");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 20,
                column: "Name",
                value: "WorkScheduleUserRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 21,
                column: "Name",
                value: "WorkScheduleUserCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 22,
                column: "Name",
                value: "WorkScheduleUserUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 23,
                column: "Name",
                value: "WorkScheduleUserDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 24,
                column: "Name",
                value: "WorkScheduleUserExceptionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 25,
                column: "Name",
                value: "ServiceCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 26,
                column: "Name",
                value: "ServiceRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 27,
                column: "Name",
                value: "ServiceUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 28,
                column: "Name",
                value: "ServiceDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 29,
                column: "Name",
                value: "AppointmentSchedule");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 30,
                column: "Name",
                value: "AppointmentRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 31,
                column: "Name",
                value: "AppointmentUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 32,
                column: "Name",
                value: "AppointmentCancel");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 33,
                column: "Name",
                value: "AppointmentApprove");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 34,
                column: "Name",
                value: "MediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 35,
                column: "Name",
                value: "MediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 36,
                column: "Name",
                value: "MediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 37,
                column: "Name",
                value: "CompanyMediaUpload");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 38,
                column: "Name",
                value: "CompanyMediaRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 39,
                column: "Name",
                value: "CompanyMediaDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 40,
                column: "Name",
                value: "CompanyMediaUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 41,
                column: "Name",
                value: "FaqCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 42,
                column: "Name",
                value: "FaqRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 43,
                column: "Name",
                value: "FaqUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 44,
                column: "Name",
                value: "FaqDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 45,
                column: "Name",
                value: "FaqCategoryCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 46,
                column: "Name",
                value: "FaqCategoryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 47,
                column: "Name",
                value: "FaqCategoryUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 48,
                column: "Name",
                value: "FaqCategoryDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 49,
                column: "Name",
                value: "InvitationSend");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 50,
                column: "Name",
                value: "InvitationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 51,
                column: "Name",
                value: "InvitationRevoke");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 52,
                column: "Name",
                value: "LocationRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 53,
                column: "Name",
                value: "CityRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 54,
                column: "Name",
                value: "StateRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 55,
                column: "Name",
                value: "CountryRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 56,
                column: "Name",
                value: "RoleCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 57,
                column: "Name",
                value: "RoleRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 58,
                column: "Name",
                value: "RoleUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 59,
                column: "Name",
                value: "RoleDelete");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 60,
                column: "Name",
                value: "RolePermissionManage");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 61,
                column: "Name",
                value: "UserLoginDataRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 62,
                column: "Name",
                value: "UserLoginDataManage");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 37, 1 },
                    { 38, 1 },
                    { 39, 1 },
                    { 56, 1 },
                    { 57, 1 },
                    { 58, 1 },
                    { 26, 2 },
                    { 30, 2 },
                    { 37, 3 },
                    { 38, 3 },
                    { 39, 3 },
                    { 20, 4 },
                    { 21, 4 },
                    { 22, 4 },
                    { 30, 4 }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedules_UserAccounts_UserAccountID",
                table: "WorkSchedules",
                column: "UserAccountID",
                principalTable: "UserAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkSchedules_UserAccounts_UserAccountID",
                table: "WorkSchedules");

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
                keyValues: new object[] { 56, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 57, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 58, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 26, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 30, 2 });

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
                keyValues: new object[] { 30, 4 });

            migrationBuilder.RenameColumn(
                name: "UserAccountID",
                table: "WorkSchedules",
                newName: "UserID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkSchedules_UserAccountID",
                table: "WorkSchedules",
                newName: "IX_WorkSchedules_UserID");

            migrationBuilder.AddColumn<TimeOnly>(
                name: "BreakEndTime",
                table: "WorkSchedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "BreakStartTime",
                table: "WorkSchedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "WorkSchedules",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsWorkingDay",
                table: "WorkSchedules",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "CompanyID",
                table: "WorkScheduleExceptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 60,
                column: "Name",
                value: "RoleRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 61,
                column: "Name",
                value: "RoleUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 62,
                column: "Name",
                value: "RoleDelete");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { 63, "RolePermissionManage" },
                    { 64, "UserLoginDataRead" },
                    { 65, "UserLoginDataManage" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 23, 1 },
                    { 34, 1 },
                    { 35, 1 },
                    { 36, 1 },
                    { 52, 1 },
                    { 53, 1 },
                    { 54, 1 },
                    { 61, 1 },
                    { 62, 1 },
                    { 32, 2 },
                    { 33, 2 },
                    { 23, 3 },
                    { 29, 3 },
                    { 31, 3 },
                    { 36, 3 },
                    { 52, 3 },
                    { 53, 3 },
                    { 54, 3 },
                    { 25, 4 },
                    { 27, 4 },
                    { 33, 4 },
                    { 35, 4 },
                    { 63, 1 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_CompanyID",
                table: "WorkSchedules",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleExceptions_CompanyID",
                table: "WorkScheduleExceptions",
                column: "CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkScheduleExceptions_Companies_CompanyID",
                table: "WorkScheduleExceptions",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedules_Companies_CompanyID",
                table: "WorkSchedules",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkSchedules_UserAccounts_UserID",
                table: "WorkSchedules",
                column: "UserID",
                principalTable: "UserAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
