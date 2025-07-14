using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiedPhoneNumberMaxLengthAndFractionTolocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Locations",
                type: "decimal(24,15)",
                precision: 24,
                scale: 15,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(11,8)",
                oldPrecision: 11,
                oldScale: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Locations",
                type: "decimal(23,15)",
                precision: 23,
                scale: 15,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(10,8)",
                oldPrecision: 10,
                oldScale: 8,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Companies",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(9)",
                oldMaxLength: 9,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Longitude",
                table: "Locations",
                type: "decimal(11,8)",
                precision: 11,
                scale: 8,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(24,15)",
                oldPrecision: 24,
                oldScale: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Latitude",
                table: "Locations",
                type: "decimal(10,8)",
                precision: 10,
                scale: 8,
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(23,15)",
                oldPrecision: 23,
                oldScale: 15,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Phone",
                table: "Companies",
                type: "nvarchar(9)",
                maxLength: 9,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20,
                oldNullable: true);
        }
    }
}
