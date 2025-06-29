using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class modifiedMediaEntityTobeMoreComfortableToFutureAwsS3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "FileSize",
                table: "Medias",
                newName: "FileSizeInBytes");

            migrationBuilder.RenameColumn(
                name: "FilePath",
                table: "Medias",
                newName: "RemoteUrl");

            migrationBuilder.RenameColumn(
                name: "FileName",
                table: "Medias",
                newName: "OriginalName");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RemoteUrl",
                table: "Medias",
                newName: "FilePath");

            migrationBuilder.RenameColumn(
                name: "OriginalName",
                table: "Medias",
                newName: "FileName");

            migrationBuilder.RenameColumn(
                name: "FileSizeInBytes",
                table: "Medias",
                newName: "FileSize");
        }
    }
}
