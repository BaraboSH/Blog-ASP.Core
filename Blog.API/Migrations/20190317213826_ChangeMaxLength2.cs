using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.API.Migrations
{
    public partial class ChangeMaxLength2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Story",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 160,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Story",
                maxLength: 160,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 100,
                oldNullable: true);
        }
    }
}
