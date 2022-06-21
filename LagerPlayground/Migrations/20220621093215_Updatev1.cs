using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class Updatev1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order_DetailsID",
                table: "Totes",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Totes_Order_DetailsID",
                table: "Totes",
                column: "Order_DetailsID");

            migrationBuilder.AddForeignKey(
                name: "FK_Totes_Order_Details_Order_DetailsID",
                table: "Totes",
                column: "Order_DetailsID",
                principalTable: "Order_Details",
                principalColumn: "ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Totes_Order_Details_Order_DetailsID",
                table: "Totes");

            migrationBuilder.DropIndex(
                name: "IX_Totes_Order_DetailsID",
                table: "Totes");

            migrationBuilder.DropColumn(
                name: "Order_DetailsID",
                table: "Totes");
        }
    }
}
