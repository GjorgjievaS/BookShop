using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShop.Repository.Migrations
{
    public partial class third : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ValidDate",
                table: "Book");

            migrationBuilder.AddColumn<DateTime>(
                name: "Published",
                table: "Book",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "Book");

            migrationBuilder.AddColumn<DateTime>(
                name: "ValidDate",
                table: "Book",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
