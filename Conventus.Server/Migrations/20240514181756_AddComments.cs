using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conventus.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Content = table.Column<string>(type: "TEXT", maxLength: 2147483647, nullable: false, comment: "The content of the comment"),
                    PostId = table.Column<Guid>(type: "TEXT", nullable: false, comment: "The id of the post the comment belongs to"),
                    DateCreated = table.Column<DateOnly>(type: "TEXT", nullable: false, comment: "The day the comment was created on"),
                    TimeCreated = table.Column<TimeOnly>(type: "TEXT", nullable: false, comment: "The time the comment was created on")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Comments");
        }
    }
}
