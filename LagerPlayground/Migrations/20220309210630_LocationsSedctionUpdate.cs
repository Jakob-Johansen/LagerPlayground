using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class LocationsSedctionUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Section",
                table: "Locations",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Section",
                table: "Locations");
        }
    }
}
