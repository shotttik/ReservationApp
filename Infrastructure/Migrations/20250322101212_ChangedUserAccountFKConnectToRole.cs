using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ChangedUserAccountFKConnectToRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleUserAccount");

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
                name: "FK_UserAccounts_Roles_RoleID",
                table: "UserAccounts",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Roles_RoleID",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_RoleID",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "RoleID",
                table: "UserAccounts");

            migrationBuilder.CreateTable(
                name: "RoleUserAccount",
                columns: table => new
                {
                    RolesID = table.Column<int>(type: "int", nullable: false),
                    UserAccountsID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleUserAccount", x => new { x.RolesID, x.UserAccountsID });
                    table.ForeignKey(
                        name: "FK_RoleUserAccount_Roles_RolesID",
                        column: x => x.RolesID,
                        principalTable: "Roles",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleUserAccount_UserAccounts_UserAccountsID",
                        column: x => x.UserAccountsID,
                        principalTable: "UserAccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleUserAccount_UserAccountsID",
                table: "RoleUserAccount",
                column: "UserAccountsID");
        }
    }
}
