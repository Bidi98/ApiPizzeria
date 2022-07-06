using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApiPizzeria.Migrations
{
    public partial class test4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentToken",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CurrentToken",
                table: "Users",
                type: "TEXT",
                nullable: true);
        }
    }
}
