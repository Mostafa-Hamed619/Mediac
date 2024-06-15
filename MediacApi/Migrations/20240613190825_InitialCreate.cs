using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediacApi.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Blogs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    blogName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blogDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    blogImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    checkFollow = table.Column<bool>(type: "bit", nullable: false),
                    followers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blogs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PostName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    firstBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secondHeader = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    secondBody = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    visible = table.Column<bool>(type: "bit", nullable: false),
                    postImage = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Refrences = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BlogNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Posts_Blogs_BlogNumber",
                        column: x => x.BlogNumber,
                        principalTable: "Blogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Posts_BlogNumber",
                table: "Posts",
                column: "BlogNumber");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Posts");

            migrationBuilder.DropTable(
                name: "Blogs");
        }
    }
}
