using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediacApi.Migrations
{
    /// <inheritdoc />
    public partial class AddFollowerDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "followers",
                columns: table => new
                {
                    FollowerUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    FolloweeUserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_followers", x => new { x.FollowerUserId, x.FolloweeUserId });
                    table.ForeignKey(
                        name: "FK_followers_Users_FolloweeUserId",
                        column: x => x.FolloweeUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_followers_Users_FollowerUserId",
                        column: x => x.FollowerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_followers_FolloweeUserId",
                table: "followers",
                column: "FolloweeUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "followers");
        }
    }
}
