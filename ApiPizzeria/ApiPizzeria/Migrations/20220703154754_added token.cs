using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPizzeria.Migrations
{
    public partial class addedtoken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentRefreshToken",
                table: "Users",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrentToken",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentRefreshToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "CurrentToken",
                table: "Users");
        }
    }
}
