using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class removedStatusChangedAtColumnUnnecessaryUpdatedAtWillWork : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "UserLoginDatas");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "Services");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "CompanyFAQs");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "CompanyFAQCategories");

            migrationBuilder.DropColumn(
                name: "StatusChangedAt",
                table: "Companies");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "UserLoginDatas",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "Services",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "CompanyFAQs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "CompanyFAQCategories",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StatusChangedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);
        }
    }
}
