using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedAndAddedWorkingScheduleAndWorkingException : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingException_Companies_CompanyID",
                table: "WorkingException");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingException_UserAccounts_UserAccountID",
                table: "WorkingException");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingSchedule_Companies_CompanyID",
                table: "WorkingSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingSchedule_UserAccounts_UserID",
                table: "WorkingSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingSchedule",
                table: "WorkingSchedule");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingException",
                table: "WorkingException");

            migrationBuilder.RenameTable(
                name: "WorkingSchedule",
                newName: "WorkingSchedules");

            migrationBuilder.RenameTable(
                name: "WorkingException",
                newName: "WorkingExceptions");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingSchedule_UserID",
                table: "WorkingSchedules",
                newName: "IX_WorkingSchedules_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingSchedule_CompanyID",
                table: "WorkingSchedules",
                newName: "IX_WorkingSchedules_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingException_UserAccountID",
                table: "WorkingExceptions",
                newName: "IX_WorkingExceptions_UserAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingException_CompanyID",
                table: "WorkingExceptions",
                newName: "IX_WorkingExceptions_CompanyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingSchedules",
                table: "WorkingSchedules",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingExceptions",
                table: "WorkingExceptions",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingExceptions_Companies_CompanyID",
                table: "WorkingExceptions",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingExceptions_UserAccounts_UserAccountID",
                table: "WorkingExceptions",
                column: "UserAccountID",
                principalTable: "UserAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingSchedules_Companies_CompanyID",
                table: "WorkingSchedules",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingSchedules_UserAccounts_UserID",
                table: "WorkingSchedules",
                column: "UserID",
                principalTable: "UserAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WorkingExceptions_Companies_CompanyID",
                table: "WorkingExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingExceptions_UserAccounts_UserAccountID",
                table: "WorkingExceptions");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingSchedules_Companies_CompanyID",
                table: "WorkingSchedules");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkingSchedules_UserAccounts_UserID",
                table: "WorkingSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingSchedules",
                table: "WorkingSchedules");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WorkingExceptions",
                table: "WorkingExceptions");

            migrationBuilder.RenameTable(
                name: "WorkingSchedules",
                newName: "WorkingSchedule");

            migrationBuilder.RenameTable(
                name: "WorkingExceptions",
                newName: "WorkingException");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingSchedules_UserID",
                table: "WorkingSchedule",
                newName: "IX_WorkingSchedule_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingSchedules_CompanyID",
                table: "WorkingSchedule",
                newName: "IX_WorkingSchedule_CompanyID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingExceptions_UserAccountID",
                table: "WorkingException",
                newName: "IX_WorkingException_UserAccountID");

            migrationBuilder.RenameIndex(
                name: "IX_WorkingExceptions_CompanyID",
                table: "WorkingException",
                newName: "IX_WorkingException_CompanyID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingSchedule",
                table: "WorkingSchedule",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WorkingException",
                table: "WorkingException",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingException_Companies_CompanyID",
                table: "WorkingException",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingException_UserAccounts_UserAccountID",
                table: "WorkingException",
                column: "UserAccountID",
                principalTable: "UserAccounts",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingSchedule_Companies_CompanyID",
                table: "WorkingSchedule",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkingSchedule_UserAccounts_UserID",
                table: "WorkingSchedule",
                column: "UserID",
                principalTable: "UserAccounts",
                principalColumn: "ID");
        }
    }
}
