using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.API.Migrations
{
    public partial class Update_FK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Share_Story_StoryId",
                table: "Share");

            migrationBuilder.DropForeignKey(
                name: "FK_Share_User_UserId",
                table: "Share");

            migrationBuilder.AddForeignKey(
                name: "FK_Share_Story_StoryId",
                table: "Share",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Share_User_UserId",
                table: "Share",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Share_Story_StoryId",
                table: "Share");

            migrationBuilder.DropForeignKey(
                name: "FK_Share_User_UserId",
                table: "Share");

            migrationBuilder.AddForeignKey(
                name: "FK_Share_Story_StoryId",
                table: "Share",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Share_User_UserId",
                table: "Share",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
