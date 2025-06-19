using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPortal.Migrations
{
    /// <inheritdoc />
    public partial class MediaFileIntId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Blogs_MediaFiles_MediaFileId",
                table: "Blogs");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_MediaFileId",
                table: "Blogs");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "MediaFiles",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)")
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "MediaFileId",
                table: "Blogs",
                type: "longtext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "MediaFiles",
                type: "varchar(255)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AlterColumn<string>(
                name: "MediaFileId",
                table: "Blogs",
                type: "varchar(255)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
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
    }
}
