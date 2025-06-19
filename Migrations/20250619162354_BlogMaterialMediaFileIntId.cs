using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPortal.Migrations
{
    /// <inheritdoc />
    public partial class BlogMaterialMediaFileIntId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_MediaFiles_MediaFileId1",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_MediaFileId1",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MediaFileId1",
                table: "Blogs");

            migrationBuilder.AlterColumn<int>(
                name: "MediaFileId",
                table: "Blogs",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_MediaFileId",
                table: "Blogs",
                column: "MediaFileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_MediaFiles_MediaFileId",
                table: "Blogs",
                column: "MediaFileId",
                principalTable: "MediaFiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_MediaFiles_MediaFileId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_MediaFileId",
                table: "Blogs");

            migrationBuilder.AlterColumn<string>(
                name: "MediaFileId",
                table: "Blogs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "MediaFileId1",
                table: "Blogs",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Blogs_MediaFileId1",
                table: "Blogs",
                column: "MediaFileId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Blogs_MediaFiles_MediaFileId1",
                table: "Blogs",
                column: "MediaFileId1",
                principalTable: "MediaFiles",
                principalColumn: "Id");
        }
    }
}
