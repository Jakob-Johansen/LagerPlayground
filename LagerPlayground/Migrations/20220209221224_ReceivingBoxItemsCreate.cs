using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class ReceivingBoxItemsCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiveBox_Items",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiveBoxID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReceivingBoxID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveBox_Items", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceiveBox_Items_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveBox_Items_ReceivingBoxes_ReceivingBoxID",
                        column: x => x.ReceivingBoxID,
                        principalTable: "ReceivingBoxes",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveBox_Items_ProductID",
                table: "ReceiveBox_Items",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveBox_Items_ReceivingBoxID",
                table: "ReceiveBox_Items",
                column: "ReceivingBoxID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiveBox_Items");
        }
    }
}
