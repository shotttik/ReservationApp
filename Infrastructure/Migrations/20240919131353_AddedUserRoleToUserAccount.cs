using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserRoleToUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RoleID",
                table: "UserAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_RoleID",
                table: "UserAccounts",
                column: "RoleID");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_UserRoles_RoleID",
                table: "UserAccounts",
                column: "RoleID",
                principalTable: "UserRoles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_UserRoles_RoleID",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_RoleID",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "UserAccounts");
        }
    }
}
