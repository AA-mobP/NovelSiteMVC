using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class removeContentUrlProp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentUrl",
                table: "tblChapters");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentUrl",
                table: "tblChapters",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");
        }
    }
}
