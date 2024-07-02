using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediacApi.Migrations
{
    /// <inheritdoc />
    public partial class FixBlogSubscribeMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscribes_Posts_PostId",
                table: "subscribes");

            migrationBuilder.DropColumn(
                name: "FollowersId",
                table: "Posts");

            migrationBuilder.RenameColumn(
                name: "PostId",
                table: "subscribes",
                newName: "BlogId");

            migrationBuilder.RenameIndex(
                name: "IX_subscribes_PostId",
                table: "subscribes",
                newName: "IX_subscribes_BlogId");

            migrationBuilder.AddColumn<string>(
                name: "FollowersId",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_subscribes_Blogs_BlogId",
                table: "subscribes",
                column: "BlogId",
                principalTable: "Blogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_subscribes_Blogs_BlogId",
                table: "subscribes");

            migrationBuilder.DropColumn(
                name: "FollowersId",
                table: "Blogs");

            migrationBuilder.RenameColumn(
                name: "BlogId",
                table: "subscribes",
                newName: "PostId");

            migrationBuilder.RenameIndex(
                name: "IX_subscribes_BlogId",
                table: "subscribes",
                newName: "IX_subscribes_PostId");

            migrationBuilder.AddColumn<string>(
                name: "FollowersId",
                table: "Posts",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_subscribes_Posts_PostId",
                table: "subscribes",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
