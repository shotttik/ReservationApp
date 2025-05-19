using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCountrySubregionAndRegion : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Region",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RegionId",
                table: "Countries",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Subregion",
                table: "Countries",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SubregionId",
                table: "Countries",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Region",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "RegionId",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "Subregion",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "SubregionId",
                table: "Countries");
        }
    }
}
