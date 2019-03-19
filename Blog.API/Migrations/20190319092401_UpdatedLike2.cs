using Microsoft.EntityFrameworkCore.Migrations;

namespace Blog.API.Migrations
{
    public partial class UpdatedLike2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_Story_StoryId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_User_UserId",
                table: "Like");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Story_StoryId",
                table: "Like",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_User_UserId",
                table: "Like",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_Story_StoryId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_User_UserId",
                table: "Like");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Story_StoryId",
                table: "Like",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_User_UserId",
                table: "Like",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
