using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRedudantRolePermissions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 78, 1 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 75, 2 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 73, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 75, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 73, 4 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 75, 4 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 78);

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 73,
                column: "Name",
                value: "ReviewUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 74,
                column: "Name",
                value: "ReviewInviteCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 75,
                column: "Name",
                value: "SubscriptionPlanUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 76,
                column: "Name",
                value: "CompanySubscriptionUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 77,
                column: "Name",
                value: "CompanySubscriptionGet");

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 74, 3 },
                    { 74, 4 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 74, 3 });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 74, 4 });

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 73,
                column: "Name",
                value: "ReviewInviteCreate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 74,
                column: "Name",
                value: "ReviewInviteRead");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 75,
                column: "Name",
                value: "ReviewInviteReadLimited");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 76,
                column: "Name",
                value: "SubscriptionPlanUpdate");

            migrationBuilder.UpdateData(
                table: "Permissions",
                keyColumn: "Id",
                keyValue: 77,
                column: "Name",
                value: "CompanySubscriptionUpdate");

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "Id", "Name" },
                values: new object[] { 78, "CompanySubscriptionGet" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[,]
                {
                    { 75, 2 },
                    { 73, 3 },
                    { 75, 3 },
                    { 73, 4 },
                    { 75, 4 },
                    { 78, 1 }
                });
        }
    }
}
