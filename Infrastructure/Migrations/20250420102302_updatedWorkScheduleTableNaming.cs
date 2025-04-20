using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class updatedWorkScheduleTableNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkingExceptions");

            migrationBuilder.DropTable(
                name: "WorkingSchedules");

            migrationBuilder.CreateTable(
                name: "WorkScheduleExceptions",
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
                    table.PrimaryKey("PK_WorkScheduleExceptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkScheduleExceptions_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkScheduleExceptions_UserAccounts_UserAccountID",
                        column: x => x.UserAccountID,
                        principalTable: "UserAccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkSchedules",
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
                    table.PrimaryKey("PK_WorkSchedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkSchedules_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkSchedules_UserAccounts_UserID",
                        column: x => x.UserID,
                        principalTable: "UserAccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleExceptions_CompanyID",
                table: "WorkScheduleExceptions",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkScheduleExceptions_UserAccountID",
                table: "WorkScheduleExceptions",
                column: "UserAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_CompanyID",
                table: "WorkSchedules",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkSchedules_UserID",
                table: "WorkSchedules",
                column: "UserID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "WorkScheduleExceptions");

            migrationBuilder.DropTable(
                name: "WorkSchedules");

            migrationBuilder.CreateTable(
                name: "WorkingExceptions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    UserAccountID = table.Column<int>(type: "int", nullable: true),
                    EndDateTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsFullDay = table.Column<bool>(type: "bit", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDateTime = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingExceptions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkingExceptions_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingExceptions_UserAccounts_UserAccountID",
                        column: x => x.UserAccountID,
                        principalTable: "UserAccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkingSchedules",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyID = table.Column<int>(type: "int", nullable: false),
                    UserID = table.Column<int>(type: "int", nullable: true),
                    DayOfWeek = table.Column<int>(type: "int", nullable: false),
                    EndTime = table.Column<TimeOnly>(type: "time", nullable: true),
                    IsWorkingDay = table.Column<bool>(type: "bit", nullable: false),
                    StartTime = table.Column<TimeOnly>(type: "time", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkingSchedules", x => x.ID);
                    table.ForeignKey(
                        name: "FK_WorkingSchedules_Companies_CompanyID",
                        column: x => x.CompanyID,
                        principalTable: "Companies",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_WorkingSchedules_UserAccounts_UserID",
                        column: x => x.UserID,
                        principalTable: "UserAccounts",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_WorkingExceptions_CompanyID",
                table: "WorkingExceptions",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingExceptions_UserAccountID",
                table: "WorkingExceptions",
                column: "UserAccountID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedules_CompanyID",
                table: "WorkingSchedules",
                column: "CompanyID");

            migrationBuilder.CreateIndex(
                name: "IX_WorkingSchedules_UserID",
                table: "WorkingSchedules",
                column: "UserID");
        }
    }
}
