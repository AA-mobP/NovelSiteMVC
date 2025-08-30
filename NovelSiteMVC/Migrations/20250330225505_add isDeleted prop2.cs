using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class addisDeletedprop2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "idDeleted",
                table: "tblPosts",
                newName: "isDeleted");

            migrationBuilder.RenameColumn(
                name: "idDeleted",
                table: "tblBlogs",
                newName: "isDeleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "tblPosts",
                newName: "idDeleted");

            migrationBuilder.RenameColumn(
                name: "isDeleted",
                table: "tblBlogs",
                newName: "idDeleted");
        }
    }
}
