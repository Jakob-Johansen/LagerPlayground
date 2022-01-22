using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class Order_DetailsUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order_ItemsID",
                table: "Order_Details");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order_ItemsID",
                table: "Order_Details",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
