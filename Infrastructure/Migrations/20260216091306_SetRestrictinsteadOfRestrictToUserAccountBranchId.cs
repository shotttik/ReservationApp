using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SetRestrictinsteadOfRestrictToUserAccountBranchId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Branches_BranchId",
                table: "UserAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Branches_BranchId",
                table: "UserAccounts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Branches_BranchId",
                table: "UserAccounts");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccounts_Branches_BranchId",
                table: "UserAccounts",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "ID",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
