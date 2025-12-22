using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedPhoneNumberFieldToUserLoginData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "VerificationTokenExpTime",
                table: "UserLoginDatas",
                newName: "PhoneVerificationTokenExpTime");

            migrationBuilder.RenameColumn(
                name: "VerificationToken",
                table: "UserLoginDatas",
                newName: "PhoneVerificationToken");

            migrationBuilder.RenameColumn(
                name: "VerificationStatus",
                table: "UserLoginDatas",
                newName: "PhoneVerificationStatus");

            migrationBuilder.RenameColumn(
                name: "ConfirmationToken",
                table: "UserLoginDatas",
                newName: "EmailVerificationToken");

            migrationBuilder.AddColumn<int>(
                name: "EmailVerificationStatus",
                table: "UserLoginDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EmailVerificationTokenExpTime",
                table: "UserLoginDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PendingNewPhone",
                table: "UserLoginDatas",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "UserLoginDatas",
                type: "nvarchar(15)",
                maxLength: 15,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailVerificationStatus",
                table: "UserLoginDatas");

            migrationBuilder.DropColumn(
                name: "EmailVerificationTokenExpTime",
                table: "UserLoginDatas");

            migrationBuilder.DropColumn(
                name: "PendingNewPhone",
                table: "UserLoginDatas");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "UserLoginDatas");

            migrationBuilder.RenameColumn(
                name: "PhoneVerificationTokenExpTime",
                table: "UserLoginDatas",
                newName: "VerificationTokenExpTime");

            migrationBuilder.RenameColumn(
                name: "PhoneVerificationToken",
                table: "UserLoginDatas",
                newName: "VerificationToken");

            migrationBuilder.RenameColumn(
                name: "PhoneVerificationStatus",
                table: "UserLoginDatas",
                newName: "VerificationStatus");

            migrationBuilder.RenameColumn(
                name: "EmailVerificationToken",
                table: "UserLoginDatas",
                newName: "ConfirmationToken");
        }
    }
}
