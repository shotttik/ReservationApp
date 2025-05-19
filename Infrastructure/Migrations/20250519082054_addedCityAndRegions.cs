using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedCityAndRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Iso3 = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    NumericCode = table.Column<string>(type: "nvarchar(3)", maxLength: 3, nullable: true),
                    Iso2 = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: true),
                    PhoneCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Capital = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Currency = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrencyName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CurrencySymbol = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Tld = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Native = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Nationality = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Timezones = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: true),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: true),
                    Emoji = table.Column<string>(type: "nvarchar(191)", maxLength: 191, nullable: true),
                    EmojiU = table.Column<string>(type: "nvarchar(191)", maxLength: 191, nullable: true),
                    Flag = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    WikiDataId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    StateId = table.Column<int>(type: "int", nullable: false),
                    StateCode = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    CountryId = table.Column<int>(type: "int", nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(2)", maxLength: 2, nullable: false),
                    Latitude = table.Column<decimal>(type: "decimal(10,8)", precision: 10, scale: 8, nullable: false),
                    Longitude = table.Column<decimal>(type: "decimal(11,8)", precision: 11, scale: 8, nullable: false),
                    Flag = table.Column<bool>(type: "bit", nullable: false, defaultValue: true),
                    WikiDataId = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Cities_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cities_CountryId",
                table: "Cities",
                column: "CountryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
