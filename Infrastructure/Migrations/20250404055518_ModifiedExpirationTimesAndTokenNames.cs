using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedExpirationTimesAndTokenNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerificationTokenExpirationTime",
                table: "UserLoginDatas",
                newName: "VerificationTokenExpTime");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpirationTime",
                table: "UserLoginDatas",
                newName: "RefreshTokenExpTime");

            migrationBuilder.RenameColumn(
                name: "RecoveryTokenTime",
                table: "UserLoginDatas",
                newName: "RecoveryTokenExpTime");

            migrationBuilder.RenameColumn(
                name: "PasswordRecoveryToken",
                table: "UserLoginDatas",
                newName: "RecoveryToken");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerificationTokenExpTime",
                table: "UserLoginDatas",
                newName: "VerificationTokenExpirationTime");

            migrationBuilder.RenameColumn(
                name: "RefreshTokenExpTime",
                table: "UserLoginDatas",
                newName: "RefreshTokenExpirationTime");

            migrationBuilder.RenameColumn(
                name: "RecoveryTokenExpTime",
                table: "UserLoginDatas",
                newName: "RecoveryTokenTime");

            migrationBuilder.RenameColumn(
                name: "RecoveryToken",
                table: "UserLoginDatas",
                newName: "PasswordRecoveryToken");
        }
    }
}
