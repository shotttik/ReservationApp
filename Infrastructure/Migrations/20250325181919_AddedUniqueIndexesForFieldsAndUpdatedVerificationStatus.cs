using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniqueIndexesForFieldsAndUpdatedVerificationStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EmailValidationStatus",
                table: "UserLoginDatas",
                newName: "VerificationStatus");

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginDatas_Email",
                table: "UserLoginDatas",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Email",
                table: "Companies",
                column: "Email",
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Companies_IN",
                table: "Companies",
                column: "IN",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Companies_Phone",
                table: "Companies",
                column: "Phone",
                unique: true,
                filter: "[Phone] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserLoginDatas_Email",
                table: "UserLoginDatas");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Email",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_IN",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_Phone",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "VerificationStatus",
                table: "UserLoginDatas",
                newName: "EmailValidationStatus");
        }
    }
}
