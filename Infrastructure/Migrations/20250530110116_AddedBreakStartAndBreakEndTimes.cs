using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedBreakStartAndBreakEndTimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeOnly>(
                name: "BreakEndTime",
                table: "WorkSchedules",
                type: "time",
                nullable: true);

            migrationBuilder.AddColumn<TimeOnly>(
                name: "BreakStartTime",
                table: "WorkSchedules",
                type: "time",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BreakEndTime",
                table: "WorkSchedules");

            migrationBuilder.DropColumn(
                name: "BreakStartTime",
                table: "WorkSchedules");
        }
    }
}
