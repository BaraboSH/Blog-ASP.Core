using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Blog.API.Migrations
{
    public partial class UpdatedLike : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_Story_StoryId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_User_UserId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_User_OwnerId",
                table: "Story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.DropIndex(
                name: "IX_Like_StoryId",
                table: "Like");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Like");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Like",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StoryId",
                table: "Like",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                columns: new[] { "StoryId", "UserId" });

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

            migrationBuilder.AddForeignKey(
                name: "FK_Story_User_OwnerId",
                table: "Story",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Like_Story_StoryId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Like_User_UserId",
                table: "Like");

            migrationBuilder.DropForeignKey(
                name: "FK_Story_User_OwnerId",
                table: "Story");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Like",
                table: "Like");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Like",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "StoryId",
                table: "Like",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "Like",
                nullable: false,
                defaultValue: 0)
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Like",
                table: "Like",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Like_StoryId",
                table: "Like",
                column: "StoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Like_Story_StoryId",
                table: "Like",
                column: "StoryId",
                principalTable: "Story",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Like_User_UserId",
                table: "Like",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Story_User_OwnerId",
                table: "Story",
                column: "OwnerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
