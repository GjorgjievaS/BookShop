using Microsoft.EntityFrameworkCore.Migrations;

namespace BookShop.Repository.Migrations
{
    public partial class deletedGenre : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Genre",
                table: "Book");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Genre",
                table: "Book",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
