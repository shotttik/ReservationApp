using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addPendingNewEmailField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PendingNewEmail",
                table: "UserLoginDatas",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserLoginDatas_PendingNewEmail",
                table: "UserLoginDatas",
                column: "PendingNewEmail",
                unique: true,
                filter: "[PendingNewEmail] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_UserLoginDatas_PendingNewEmail",
                table: "UserLoginDatas");

            migrationBuilder.DropColumn(
                name: "PendingNewEmail",
                table: "UserLoginDatas");
        }
    }
}
