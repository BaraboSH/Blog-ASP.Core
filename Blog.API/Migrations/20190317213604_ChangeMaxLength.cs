using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.API.Migrations
{
    public partial class ChangeMaxLength : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Story",
                maxLength: 160,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 60,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Story",
                maxLength: 60,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 160,
                oldNullable: true);
        }
    }
}
