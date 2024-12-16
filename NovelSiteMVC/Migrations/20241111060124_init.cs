using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblNovels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AlterNames = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: true),
                    Synposis = table.Column<string>(type: "nvarchar(750)", maxLength: 750, nullable: true),
                    PublishDate = table.Column<DateOnly>(type: "date", nullable: false),
                    LastEdit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Author = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Artist = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Publisher = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Theme = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Genres = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblNovels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "tblChapters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ContentUrl = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Number = table.Column<int>(type: "int", nullable: false),
                    NovelId = table.Column<int>(type: "int", nullable: false),
                    LastEdit = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Releaser = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    TLor = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    PRer = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    QCer = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Watches = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblChapters", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblChapters_tblNovels_NovelId",
                        column: x => x.NovelId,
                        principalTable: "tblNovels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblChapters_NovelId",
                table: "tblChapters",
                column: "NovelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblChapters");

            migrationBuilder.DropTable(
                name: "tblNovels");
        }
    }
}
