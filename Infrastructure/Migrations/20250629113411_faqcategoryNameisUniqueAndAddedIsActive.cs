using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class faqcategoryNameisUniqueAndAddedIsActive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyFAQCategories_Name",
                table: "CompanyFAQCategories");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "CompanyFAQCategories",
                type: "bit",
                nullable: false,
                defaultValue: true);

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFAQCategories_Name",
                table: "CompanyFAQCategories",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_CompanyFAQCategories_Name",
                table: "CompanyFAQCategories");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "CompanyFAQCategories");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyFAQCategories_Name",
                table: "CompanyFAQCategories",
                column: "Name");
        }
    }
}
