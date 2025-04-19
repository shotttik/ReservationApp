using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedWorkingExceptionsForCOmpanyAndUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "WorkingException",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    UserAccountID = table.Column<int>(type: "int", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFullDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingException", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkingException_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingException_UserAccounts_UserAccountID",
                        column: x => x.UserAccountID,
                        principalTable: "UserAccounts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateTable(
                name: "WorkingSchedule",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsWorkingDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingSchedule", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkingSchedule_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingSchedule_UserAccounts_UserID",
                        column: x => x.UserID,
                        principalTable: "UserAccounts",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingException_CompanyID",
                table: "WorkingException",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingException_UserAccountID",
                table: "WorkingException",
                column: "UserAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedule_CompanyID",
                table: "WorkingSchedule",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedule_UserID",
                table: "WorkingSchedule",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingException");

            migrationBuilder.DropTable(
                name: "WorkingSchedule");
        }
    }
}
