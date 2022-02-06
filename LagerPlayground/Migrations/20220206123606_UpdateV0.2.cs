using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class UpdateV02 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ArrivalDate",
                table: "ReceivingOrder_Details",
                newName: "Expected");

            migrationBuilder.RenameColumn(
                name: "CompanyName",
                table: "ReceiveCustommers",
                newName: "Vendor");

            migrationBuilder.AddColumn<DateTime>(
                name: "Closed",
                table: "ReceivingOrder_Details",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "ReceivingOrder_Details",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OrderStatus",
                table: "Order_Details",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Closed",
                table: "ReceivingOrder_Details");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "ReceivingOrder_Details");

            migrationBuilder.DropColumn(
                name: "OrderStatus",
                table: "Order_Details");

            migrationBuilder.RenameColumn(
                name: "Expected",
                table: "ReceivingOrder_Details",
                newName: "ArrivalDate");

            migrationBuilder.RenameColumn(
                name: "Vendor",
                table: "ReceiveCustommers",
                newName: "CompanyName");
        }
    }
}
