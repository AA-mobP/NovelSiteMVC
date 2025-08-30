using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class BlogPostTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblBlogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PageId = table.Column<int>(type: "int", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBlogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblPosts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BlogId = table.Column<int>(type: "int", nullable: false),
                    LastEdit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Watches = table.Column<int>(type: "int", nullable: false),
                    FileName = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    PhotoName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Author = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PageId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPosts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblPosts_tblBlogs_BlogId",
                        column: x => x.BlogId,
                        principalTable: "tblBlogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblPosts_BlogId",
                table: "tblPosts",
                column: "BlogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblPosts");

            migrationBuilder.DropTable(
                name: "tblBlogs");
        }
    }
}
