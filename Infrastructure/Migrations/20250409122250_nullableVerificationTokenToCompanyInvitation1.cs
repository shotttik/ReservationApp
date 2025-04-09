using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class nullableVerificationTokenToCompanyInvitation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyInvitations_Token",
                table: "CompanyInvitations");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "CompanyInvitations",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInvitations_Token",
                table: "CompanyInvitations",
                column: "Token",
                unique: true,
                filter: "[Token] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyInvitations_Token",
                table: "CompanyInvitations");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "CompanyInvitations",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyInvitations_Token",
                table: "CompanyInvitations",
                column: "Token",
                unique: true);
        }
    }
}
