using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class LocationDetailUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rack",
                table: "Locations");

            migrationBuilder.AddColumn<int>(
                name: "Rack",
                table: "Locations_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rack",
                table: "Locations_Details");

            migrationBuilder.AddColumn<int>(
                name: "Rack",
                table: "Locations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
