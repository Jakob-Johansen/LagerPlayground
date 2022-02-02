using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class UpdateReceivingOrderDetailsCreateReceiveCustommer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "ArrivalDate",
                table: "ReceivingOrder_Details",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "ReceiveCustommers",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Modified = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ReceivingOrder_DetailsID = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiveCustommers", x => x.ID);
                    table.ForeignKey(
                        name: "FK_ReceiveCustommers_ReceivingOrder_Details_ReceivingOrder_DetailsID",
                        column: x => x.ReceivingOrder_DetailsID,
                        principalTable: "ReceivingOrder_Details",
                        principalColumn: "ID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveCustommers_ReceivingOrder_DetailsID",
                table: "ReceiveCustommers",
                column: "ReceivingOrder_DetailsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReceiveCustommers");

            migrationBuilder.DropColumn(
                name: "ArrivalDate",
                table: "ReceivingOrder_Details");
        }
    }
}
