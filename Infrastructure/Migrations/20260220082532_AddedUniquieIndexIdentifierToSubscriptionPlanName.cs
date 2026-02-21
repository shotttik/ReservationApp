using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedUniquieIndexIdentifierToSubscriptionPlanName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySubscription_Companies_CompanyId",
                table: "CompanySubscription");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanySubscription_SubscriptionPlan_SubscriptionPlanId",
                table: "CompanySubscription");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionPlan",
                table: "SubscriptionPlan");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanySubscription",
                table: "CompanySubscription");

            migrationBuilder.RenameTable(
                name: "SubscriptionPlan",
                newName: "SubscriptionPlans");

            migrationBuilder.RenameTable(
                name: "CompanySubscription",
                newName: "CompanySubscriptions");

            migrationBuilder.RenameIndex(
                name: "IX_CompanySubscription_SubscriptionPlanId",
                table: "CompanySubscriptions",
                newName: "IX_CompanySubscriptions_SubscriptionPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanySubscription_CompanyId",
                table: "CompanySubscriptions",
                newName: "IX_CompanySubscriptions_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SubscriptionPlans",
                type: "nvarchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "CompanySubscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionPlans",
                table: "SubscriptionPlans",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanySubscriptions",
                table: "CompanySubscriptions",
                column: "ID");

            migrationBuilder.CreateIndex(
                name: "IX_SubscriptionPlans_Name",
                table: "SubscriptionPlans",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySubscriptions_Companies_CompanyId",
                table: "CompanySubscriptions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "CompanySubscriptions",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlans",
                principalColumn: "ID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanySubscriptions_Companies_CompanyId",
                table: "CompanySubscriptions");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanySubscriptions_SubscriptionPlans_SubscriptionPlanId",
                table: "CompanySubscriptions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SubscriptionPlans",
                table: "SubscriptionPlans");

            migrationBuilder.DropIndex(
                name: "IX_SubscriptionPlans_Name",
                table: "SubscriptionPlans");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanySubscriptions",
                table: "CompanySubscriptions");

            migrationBuilder.RenameTable(
                name: "SubscriptionPlans",
                newName: "SubscriptionPlan");

            migrationBuilder.RenameTable(
                name: "CompanySubscriptions",
                newName: "CompanySubscription");

            migrationBuilder.RenameIndex(
                name: "IX_CompanySubscriptions_SubscriptionPlanId",
                table: "CompanySubscription",
                newName: "IX_CompanySubscription_SubscriptionPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanySubscriptions_CompanyId",
                table: "CompanySubscription",
                newName: "IX_CompanySubscription_CompanyId");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "SubscriptionPlan",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(255)",
                oldMaxLength: 255);

            migrationBuilder.AlterColumn<int>(
                name: "Status",
                table: "CompanySubscription",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_SubscriptionPlan",
                table: "SubscriptionPlan",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanySubscription",
                table: "CompanySubscription",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySubscription_Companies_CompanyId",
                table: "CompanySubscription",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanySubscription_SubscriptionPlan_SubscriptionPlanId",
                table: "CompanySubscription",
                column: "SubscriptionPlanId",
                principalTable: "SubscriptionPlan",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
