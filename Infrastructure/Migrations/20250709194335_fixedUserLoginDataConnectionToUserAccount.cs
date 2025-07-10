using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixedUserLoginDataConnectionToUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserLoginDatas_UserAccounts_UserAccountID",
                table: "UserLoginDatas");

            migrationBuilder.DropIndex(
                name: "IX_UserLoginDatas_UserAccountID",
                table: "UserLoginDatas");

            migrationBuilder.DropColumn(
                name: "UserAccountID",
                table: "UserLoginDatas");

            migrationBuilder.AddColumn<int>(
                name: "UserLoginDataID",
                table: "UserAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_UserLoginDataID",
                table: "UserAccounts",
                column: "UserLoginDataID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_UserLoginDatas_UserLoginDataID",
                table: "UserAccounts",
                column: "UserLoginDataID",
                principalTable: "UserLoginDatas",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_UserLoginDatas_UserLoginDataID",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_UserLoginDataID",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "UserLoginDataID",
                table: "UserAccounts");

            migrationBuilder.AddColumn<int>(
                name: "UserAccountID",
                table: "UserLoginDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginDatas_UserAccountID",
                table: "UserLoginDatas",
                column: "UserAccountID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserLoginDatas_UserAccounts_UserAccountID",
                table: "UserLoginDatas",
                column: "UserAccountID",
                principalTable: "UserAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
