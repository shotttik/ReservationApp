using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixedCompanyDeletionSetNullToUserAccount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Companies_CompanyID",
                table: "UserAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Companies_CompanyID",
                table: "UserAccounts",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Companies_CompanyID",
                table: "UserAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Companies_CompanyID",
                table: "UserAccounts",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
