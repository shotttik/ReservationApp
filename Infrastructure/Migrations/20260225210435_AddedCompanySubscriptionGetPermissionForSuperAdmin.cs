using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedCompanySubscriptionGetPermissionForSuperAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "ID", "Name" },
                values: new object[] { 78, "CompanySubscriptionGet" });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionID", "RoleID" },
                values: new object[] { 78, 1 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionID", "RoleID" },
                keyValues: new object[] { 78, 1 });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "ID",
                keyValue: 78);
        }
    }
}
