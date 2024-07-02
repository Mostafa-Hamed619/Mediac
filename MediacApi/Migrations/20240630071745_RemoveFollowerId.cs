using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediacApi.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFollowerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FollowersId",
                table: "Blogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FollowersId",
                table: "Blogs",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
