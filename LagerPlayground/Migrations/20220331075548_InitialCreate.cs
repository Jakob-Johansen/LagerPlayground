using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Custommers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MobileNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Zipcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Custommers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Section = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Row = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dynamic = table.Column<bool>(type: "bit", nullable: false),
                    Warehouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BarcodeID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BrandName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveCustommers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Vendor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveCustommers", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveRejectedReasons",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveRejectedReasons", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingBoxes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Warehouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingBoxes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Totes",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Number = table.Column<int>(type: "int", nullable: false),
                    Warehouse = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Totes", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Order_Details",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CustommerID = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Details", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Details_Custommers_CustommerID",
                        column: x => x.CustommerID,
                        principalTable: "Custommers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations_Racks",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LocationsID = table.Column<int>(type: "int", nullable: false),
                    RackNumber = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations_Racks", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Locations_Racks_Locations_LocationsID",
                        column: x => x.LocationsID,
                        principalTable: "Locations",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceivingOrder_Details",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceiveCustommerID = table.Column<int>(type: "int", nullable: false),
                    OrderStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Expected = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Closed = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceivingOrder_Details", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceivingOrder_Details_ReceiveCustommers_ReceiveCustommerID",
                        column: x => x.ReceiveCustommerID,
                        principalTable: "ReceiveCustommers",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveBox_Items",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingBoxID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
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
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Order_Items",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Order_DetailsID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order_Items", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Order_Items_Order_Details_Order_DetailsID",
                        column: x => x.Order_DetailsID,
                        principalTable: "Order_Details",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Order_Items_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Locations_Shelfs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Locations_RacksID = table.Column<int>(type: "int", nullable: false),
                    ShelfNumber = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations_Shelfs", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Locations_Shelfs_Locations_Racks_Locations_RacksID",
                        column: x => x.Locations_RacksID,
                        principalTable: "Locations_Racks",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
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
                    Accepted = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Locations_Positions",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Locations_ShelfsID = table.Column<int>(type: "int", nullable: false),
                    PositionNumber = table.Column<int>(type: "int", nullable: false),
                    FullLocationBarcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pickable = table.Column<bool>(type: "bit", nullable: true, defaultValue: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations_Positions", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Locations_Positions_Locations_Shelfs_Locations_ShelfsID",
                        column: x => x.Locations_ShelfsID,
                        principalTable: "Locations_Shelfs",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiveRejecteds",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReceivingOrder_ItemsID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    ReceiveRejectedReasonsID = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveRejecteds", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceiveRejecteds_ReceiveRejectedReasons_ReceiveRejectedReasonsID",
                        column: x => x.ReceiveRejectedReasonsID,
                        principalTable: "ReceiveRejectedReasons",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiveRejecteds_ReceivingOrder_Items_ReceivingOrder_ItemsID",
                        column: x => x.ReceivingOrder_ItemsID,
                        principalTable: "ReceivingOrder_Items",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Product_Locations",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Locations_PositionsID = table.Column<int>(type: "int", nullable: false),
                    ProductID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    LocationBarcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product_Locations", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Product_Locations_Locations_Positions_Locations_PositionsID",
                        column: x => x.Locations_PositionsID,
                        principalTable: "Locations_Positions",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Product_Locations_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Positions_Locations_ShelfsID",
                table: "Locations_Positions",
                column: "Locations_ShelfsID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Racks_LocationsID",
                table: "Locations_Racks",
                column: "LocationsID");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_Shelfs_Locations_RacksID",
                table: "Locations_Shelfs",
                column: "Locations_RacksID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Details_CustommerID",
                table: "Order_Details",
                column: "CustommerID",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_Order_DetailsID",
                table: "Order_Items",
                column: "Order_DetailsID");

            migrationBuilder.CreateIndex(
                name: "IX_Order_Items_ProductID",
                table: "Order_Items",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Locations_Locations_PositionsID",
                table: "Product_Locations",
                column: "Locations_PositionsID");

            migrationBuilder.CreateIndex(
                name: "IX_Product_Locations_ProductID",
                table: "Product_Locations",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveBox_Items_ProductID",
                table: "ReceiveBox_Items",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveBox_Items_ReceivingBoxID",
                table: "ReceiveBox_Items",
                column: "ReceivingBoxID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveRejecteds_ReceiveRejectedReasonsID",
                table: "ReceiveRejecteds",
                column: "ReceiveRejectedReasonsID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveRejecteds_ReceivingOrder_ItemsID",
                table: "ReceiveRejecteds",
                column: "ReceivingOrder_ItemsID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingOrder_Details_ReceiveCustommerID",
                table: "ReceivingOrder_Details",
                column: "ReceiveCustommerID",
                unique: true);

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
                name: "Order_Items");

            migrationBuilder.DropTable(
                name: "Product_Locations");

            migrationBuilder.DropTable(
                name: "ReceiveBox_Items");

            migrationBuilder.DropTable(
                name: "ReceiveRejecteds");

            migrationBuilder.DropTable(
                name: "Totes");

            migrationBuilder.DropTable(
                name: "Order_Details");

            migrationBuilder.DropTable(
                name: "Locations_Positions");

            migrationBuilder.DropTable(
                name: "ReceivingBoxes");

            migrationBuilder.DropTable(
                name: "ReceiveRejectedReasons");

            migrationBuilder.DropTable(
                name: "ReceivingOrder_Items");

            migrationBuilder.DropTable(
                name: "Custommers");

            migrationBuilder.DropTable(
                name: "Locations_Shelfs");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "ReceivingOrder_Details");

            migrationBuilder.DropTable(
                name: "Locations_Racks");

            migrationBuilder.DropTable(
                name: "ReceiveCustommers");

            migrationBuilder.DropTable(
                name: "Locations");
        }
    }
}
