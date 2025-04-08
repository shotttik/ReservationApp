using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MemberIDChangedToUserAccountID : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyInvitations_Companies_CompanyId",
                table: "CompanyInvitations");

            migrationBuilder.RenameColumn(
                name: "CompanyId",
                table: "CompanyInvitations",
                newName: "CompanyID");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "CompanyInvitations",
                newName: "ID");

            migrationBuilder.RenameColumn(
                name: "MemberID",
                table: "CompanyInvitations",
                newName: "UserAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyInvitations_CompanyId",
                table: "CompanyInvitations",
                newName: "IX_CompanyInvitations_CompanyID");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyInvitations_Companies_CompanyID",
                table: "CompanyInvitations",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyInvitations_Companies_CompanyID",
                table: "CompanyInvitations");

            migrationBuilder.RenameColumn(
                name: "CompanyID",
                table: "CompanyInvitations",
                newName: "CompanyId");

            migrationBuilder.RenameColumn(
                name: "ID",
                table: "CompanyInvitations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "UserAccountID",
                table: "CompanyInvitations",
                newName: "MemberID");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyInvitations_CompanyID",
                table: "CompanyInvitations",
                newName: "IX_CompanyInvitations_CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyInvitations_Companies_CompanyId",
                table: "CompanyInvitations",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
