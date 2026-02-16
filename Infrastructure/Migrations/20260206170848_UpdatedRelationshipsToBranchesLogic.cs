using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedRelationshipsToBranchesLogic : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Companies_CompanyID",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Branches_BranchId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_BranchId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "Bookings",
                newName: "BranchId");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_CompanyID",
                table: "Bookings",
                newName: "IX_Bookings_BranchId");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "UserAccounts",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "CompanyInvitations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CompanyId",
                table: "Branches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccounts_BranchId",
                table: "UserAccounts",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Branches_CompanyId",
                table: "Branches",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Branches_Companies_CompanyId",
                table: "Branches",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

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
                name: "FK_Bookings_Branches_BranchId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Branches_Companies_CompanyId",
                table: "Branches");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccounts_Branches_BranchId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_UserAccounts_BranchId",
                table: "UserAccounts");

            migrationBuilder.DropIndex(
                name: "IX_Branches_CompanyId",
                table: "Branches");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "UserAccounts");

            migrationBuilder.DropColumn(
                name: "BranchId",
                table: "CompanyInvitations");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Branches");

            migrationBuilder.RenameColumn(
                name: "BranchId",
                table: "Bookings",
                newName: "CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_BranchId",
                table: "Bookings",
                newName: "IX_Bookings_CompanyID");

            migrationBuilder.AddColumn<int>(
                name: "BranchId",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_BranchId",
                table: "Companies",
                column: "BranchId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Companies_CompanyID",
                table: "Bookings",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Branches_BranchId",
                table: "Companies",
                column: "BranchId",
                principalTable: "Branches",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
