using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class UpdateCustommer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Details_Custommer_CustommerID",
                table: "Order_Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Custommer",
                table: "Custommer");

            migrationBuilder.RenameTable(
                name: "Custommer",
                newName: "Custommers");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Custommers",
                table: "Custommers",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Details_Custommers_CustommerID",
                table: "Order_Details",
                column: "CustommerID",
                principalTable: "Custommers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Details_Custommers_CustommerID",
                table: "Order_Details");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Custommers",
                table: "Custommers");

            migrationBuilder.RenameTable(
                name: "Custommers",
                newName: "Custommer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Custommer",
                table: "Custommer",
                column: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Details_Custommer_CustommerID",
                table: "Order_Details",
                column: "CustommerID",
                principalTable: "Custommer",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
