using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NovelSiteMVC.Migrations
{
    /// <inheritdoc />
    public partial class @try : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblPosts",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence",
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblNovels",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR PageIdSequence");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblChapters",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValueSql: "NEXT VALUE FOR PageIdSequence");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblBlogs",
                type: "int",
                nullable: true,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence",
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblPosts",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR PageIdSequence");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblNovels",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR PageIdSequence");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblChapters",
                type: "int",
                nullable: false,
                defaultValueSql: "NEXT VALUE FOR PageIdSequence",
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR PageIdSequence");

            migrationBuilder.AlterColumn<int>(
                name: "PageId",
                table: "tblBlogs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true,
                oldDefaultValueSql: "NEXT VALUE FOR PageIdSequence");
        }
    }
}
