using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class commentmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateSequence<int>(
                name: "PageIdSequence");

            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "tblNovels",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence");

            migrationBuilder.AddColumn<int>(
                name: "PageId",
                table: "tblChapters",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence");

            migrationBuilder.CreateTable(
                name: "tblComments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    ReplyTo = table.Column<int>(type: "int", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_tblComments_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblComments_PageId",
                table: "tblComments",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_tblComments_UserId",
                table: "tblComments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblComments");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "tblNovels");

            migrationBuilder.DropColumn(
                name: "PageId",
                table: "tblChapters");

            migrationBuilder.DropSequence(
                name: "PageIdSequence");
        }
    }
}
