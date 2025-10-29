using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class MediasRenamedToMedia : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMedias_Companies_CompanyID",
                table: "CompanyMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMedias_Medias_MediaID",
                table: "CompanyMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewMedias_Medias_MediaId",
                table: "ReviewMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewMedias_Reviews_ReviewId",
                table: "ReviewMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountMedias_Medias_MediaId",
                table: "UserAccountMedias");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountMedias_UserAccounts_UserAccountId",
                table: "UserAccountMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccountMedias",
                table: "UserAccountMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewMedias",
                table: "ReviewMedias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Medias",
                table: "Medias");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyMedias",
                table: "CompanyMedias");

            migrationBuilder.RenameTable(
                name: "UserAccountMedias",
                newName: "UserAccountMedia");

            migrationBuilder.RenameTable(
                name: "ReviewMedias",
                newName: "ReviewMedia");

            migrationBuilder.RenameTable(
                name: "Medias",
                newName: "Media");

            migrationBuilder.RenameTable(
                name: "CompanyMedias",
                newName: "CompanyMedia");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccountMedias_MediaId",
                table: "UserAccountMedia",
                newName: "IX_UserAccountMedia_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewMedias_MediaId",
                table: "ReviewMedia",
                newName: "IX_ReviewMedia_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyMedias_MediaID",
                table: "CompanyMedia",
                newName: "IX_CompanyMedia_MediaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccountMedia",
                table: "UserAccountMedia",
                columns: new[] { "UserAccountId", "MediaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewMedia",
                table: "ReviewMedia",
                columns: new[] { "ReviewId", "MediaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Media",
                table: "Media",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyMedia",
                table: "CompanyMedia",
                columns: new[] { "CompanyID", "MediaID" });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMedia_Companies_CompanyID",
                table: "CompanyMedia",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMedia_Media_MediaID",
                table: "CompanyMedia",
                column: "MediaID",
                principalTable: "Media",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewMedia_Media_MediaId",
                table: "ReviewMedia",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewMedia_Reviews_ReviewId",
                table: "ReviewMedia",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountMedia_Media_MediaId",
                table: "UserAccountMedia",
                column: "MediaId",
                principalTable: "Media",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountMedia_UserAccounts_UserAccountId",
                table: "UserAccountMedia",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMedia_Companies_CompanyID",
                table: "CompanyMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_CompanyMedia_Media_MediaID",
                table: "CompanyMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewMedia_Media_MediaId",
                table: "ReviewMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_ReviewMedia_Reviews_ReviewId",
                table: "ReviewMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountMedia_Media_MediaId",
                table: "UserAccountMedia");

            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountMedia_UserAccounts_UserAccountId",
                table: "UserAccountMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserAccountMedia",
                table: "UserAccountMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ReviewMedia",
                table: "ReviewMedia");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Media",
                table: "Media");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CompanyMedia",
                table: "CompanyMedia");

            migrationBuilder.RenameTable(
                name: "UserAccountMedia",
                newName: "UserAccountMedias");

            migrationBuilder.RenameTable(
                name: "ReviewMedia",
                newName: "ReviewMedias");

            migrationBuilder.RenameTable(
                name: "Media",
                newName: "Medias");

            migrationBuilder.RenameTable(
                name: "CompanyMedia",
                newName: "CompanyMedias");

            migrationBuilder.RenameIndex(
                name: "IX_UserAccountMedia_MediaId",
                table: "UserAccountMedias",
                newName: "IX_UserAccountMedias_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_ReviewMedia_MediaId",
                table: "ReviewMedias",
                newName: "IX_ReviewMedias_MediaId");

            migrationBuilder.RenameIndex(
                name: "IX_CompanyMedia_MediaID",
                table: "CompanyMedias",
                newName: "IX_CompanyMedias_MediaID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserAccountMedias",
                table: "UserAccountMedias",
                columns: new[] { "UserAccountId", "MediaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_ReviewMedias",
                table: "ReviewMedias",
                columns: new[] { "ReviewId", "MediaId" });

            migrationBuilder.AddPrimaryKey(
                name: "PK_Medias",
                table: "Medias",
                column: "ID");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CompanyMedias",
                table: "CompanyMedias",
                columns: new[] { "CompanyID", "MediaID" });

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMedias_Companies_CompanyID",
                table: "CompanyMedias",
                column: "CompanyID",
                principalTable: "Companies",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CompanyMedias_Medias_MediaID",
                table: "CompanyMedias",
                column: "MediaID",
                principalTable: "Medias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewMedias_Medias_MediaId",
                table: "ReviewMedias",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReviewMedias_Reviews_ReviewId",
                table: "ReviewMedias",
                column: "ReviewId",
                principalTable: "Reviews",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountMedias_Medias_MediaId",
                table: "UserAccountMedias",
                column: "MediaId",
                principalTable: "Medias",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountMedias_UserAccounts_UserAccountId",
                table: "UserAccountMedias",
                column: "UserAccountId",
                principalTable: "UserAccounts",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
