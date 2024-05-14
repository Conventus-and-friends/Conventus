using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Conventus.Server.Migrations
{
    /// <inheritdoc />
    public partial class AddDbComments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                comment: "The title of the post",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeCreated",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                comment: "The time the post was created on",
                oldClrType: typeof(TimeOnly),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateCreated",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                comment: "The day the post was created on",
                oldClrType: typeof(DateOnly),
                oldType: "TEXT");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "TEXT",
                maxLength: 2147483647,
                nullable: true,
                comment: "The content of the post",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2147483647,
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                comment: "The id of the category the post belongs to",
                oldClrType: typeof(long),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                comment: "The name of the category",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                comment: "The description of the category",
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Posts",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 50,
                oldComment: "The title of the post");

            migrationBuilder.AlterColumn<TimeOnly>(
                name: "TimeCreated",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(TimeOnly),
                oldType: "TEXT",
                oldComment: "The time the post was created on");

            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateCreated",
                table: "Posts",
                type: "TEXT",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "TEXT",
                oldComment: "The day the post was created on");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Posts",
                type: "TEXT",
                maxLength: 2147483647,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 2147483647,
                oldNullable: true,
                oldComment: "The content of the post");

            migrationBuilder.AlterColumn<long>(
                name: "CategoryId",
                table: "Posts",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "INTEGER",
                oldComment: "The id of the category the post belongs to");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Categories",
                type: "TEXT",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 30,
                oldComment: "The name of the category");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Categories",
                type: "TEXT",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "TEXT",
                oldMaxLength: 200,
                oldNullable: true,
                oldComment: "The description of the category");
        }
    }
}
