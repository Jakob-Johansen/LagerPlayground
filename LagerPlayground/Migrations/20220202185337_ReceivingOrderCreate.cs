using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class ReceivingOrderCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceivingOrder_Details",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingOrder_Details", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingOrder_Items",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingOrder_DetailsID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingOrder_Items", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceivingOrder_Items_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceivingOrder_Items_ReceivingOrder_Details_ReceivingOrder_DetailsID",
                        column: x => x.ReceivingOrder_DetailsID,
                        principalTable: "ReceivingOrder_Details",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingOrder_Items_ProductID",
                table: "ReceivingOrder_Items",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingOrder_Items_ReceivingOrder_DetailsID",
                table: "ReceivingOrder_Items",
                column: "ReceivingOrder_DetailsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceivingOrder_Items");

            migrationBuilder.DropTable(
                name: "ReceivingOrder_Details");
        }
    }
}
