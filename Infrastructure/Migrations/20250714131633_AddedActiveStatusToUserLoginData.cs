using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedActiveStatusToUserLoginData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DeletedAt",
                table: "UserLoginDatas",
                newName: "StatusChangedAt");

            migrationBuilder.AddColumn<int>(
                name: "ActiveStatus",
                table: "UserLoginDatas",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveStatus",
                table: "UserLoginDatas");

            migrationBuilder.RenameColumn(
                name: "StatusChangedAt",
                table: "UserLoginDatas",
                newName: "DeletedAt");
        }
    }
}
