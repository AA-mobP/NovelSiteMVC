using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class removefilenamepropfrompostModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BlogListViewModel");

            migrationBuilder.DropPrimaryKey(
                name: "PK_TodoModel",
                table: "TodoModel");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "tblPosts");

            migrationBuilder.RenameTable(
                name: "TodoModel",
                newName: "tblTodos");

            migrationBuilder.AddPrimaryKey(
                name: "PK_tblTodos",
                table: "tblTodos",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_tblTodos",
                table: "tblTodos");

            migrationBuilder.RenameTable(
                name: "tblTodos",
                newName: "TodoModel");

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "tblPosts",
                type: "nvarchar(25)",
                maxLength: 25,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TodoModel",
                table: "TodoModel",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "BlogListViewModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PageId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BlogListViewModel", x => x.Id);
                });
        }
    }
}
