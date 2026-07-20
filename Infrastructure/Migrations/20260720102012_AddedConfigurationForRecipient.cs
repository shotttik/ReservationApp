using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedConfigurationForRecipient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationRecipient_Notifications_NotificationId",
                table: "NotificationRecipient");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationRecipient_UserAccounts_UserAccountId",
                table: "NotificationRecipient");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationRecipient",
                table: "NotificationRecipient");

            migrationBuilder.DropIndex(
                name: "IX_NotificationRecipient_NotificationId",
                table: "NotificationRecipient");

            migrationBuilder.RenameTable(
                name: "NotificationRecipient",
                newName: "NotificationRecipients");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationRecipient_UserAccountId",
                table: "NotificationRecipients",
                newName: "IX_NotificationRecipients_UserAccountId");

            migrationBuilder.AlterColumn<string>(
                name: "LastDeliveryError",
                table: "NotificationRecipients",
                type: "nvarchar(2000)",
                maxLength: 2000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationRecipients",
                table: "NotificationRecipients",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipients_NotificationId_UserAccountId_DeliveryStatus_DeliveryAttempts_CreatedAt",
                table: "NotificationRecipients",
                columns: new[] { "NotificationId", "UserAccountId", "DeliveryStatus", "DeliveryAttempts", "CreatedAt" });

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipients_Notifications_NotificationId",
                table: "NotificationRecipients",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipients_UserAccounts_UserAccountId",
                table: "NotificationRecipients",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_NotificationRecipients_Notifications_NotificationId",
                table: "NotificationRecipients");

            migrationBuilder.DropForeignKey(
                name: "FK_NotificationRecipients_UserAccounts_UserAccountId",
                table: "NotificationRecipients");

            migrationBuilder.DropPrimaryKey(
                name: "PK_NotificationRecipients",
                table: "NotificationRecipients");

            migrationBuilder.DropIndex(
                name: "IX_NotificationRecipients_NotificationId_UserAccountId_DeliveryStatus_DeliveryAttempts_CreatedAt",
                table: "NotificationRecipients");

            migrationBuilder.RenameTable(
                name: "NotificationRecipients",
                newName: "NotificationRecipient");

            migrationBuilder.RenameIndex(
                name: "IX_NotificationRecipients_UserAccountId",
                table: "NotificationRecipient",
                newName: "IX_NotificationRecipient_UserAccountId");

            migrationBuilder.AlterColumn<string>(
                name: "LastDeliveryError",
                table: "NotificationRecipient",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldMaxLength: 2000,
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_NotificationRecipient",
                table: "NotificationRecipient",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRecipient_NotificationId",
                table: "NotificationRecipient",
                column: "NotificationId");

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipient_Notifications_NotificationId",
                table: "NotificationRecipient",
                column: "NotificationId",
                principalTable: "Notifications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_NotificationRecipient_UserAccounts_UserAccountId",
                table: "NotificationRecipient",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
