using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlogPortal.Migrations
{
    /// <inheritdoc />
    public partial class AddMediaFileIdToBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MediaFileId",
                table: "Blogs",
                type: "varchar(255)",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "MediaFiles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PublicId = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Url = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Format = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Folder = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsDeleted = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    BlogIds = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaFiles", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.DropTable(
                name: "MediaFiles");

            migrationBuilder.DropIndex(
                name: "IX_Blogs_MediaFileId",
                table: "Blogs");

            migrationBuilder.DropColumn(
                name: "MediaFileId",
                table: "Blogs");
        }
    }
}
