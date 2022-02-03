using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class ReceiveCustommerUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiveCustommers_ReceivingOrder_Details_ReceivingOrder_DetailsID",
                table: "ReceiveCustommers");

            migrationBuilder.DropIndex(
                name: "IX_ReceiveCustommers_ReceivingOrder_DetailsID",
                table: "ReceiveCustommers");

            migrationBuilder.DropColumn(
                name: "ReceivingOrder_DetailsID",
                table: "ReceiveCustommers");

            migrationBuilder.RenameColumn(
                name: "ReceiveCustommer",
                table: "ReceivingOrder_Details",
                newName: "ReceiveCustommerID");

            migrationBuilder.CreateIndex(
                name: "IX_ReceivingOrder_Details_ReceiveCustommerID",
                table: "ReceivingOrder_Details",
                column: "ReceiveCustommerID",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceivingOrder_Details_ReceiveCustommers_ReceiveCustommerID",
                table: "ReceivingOrder_Details",
                column: "ReceiveCustommerID",
                principalTable: "ReceiveCustommers",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceivingOrder_Details_ReceiveCustommers_ReceiveCustommerID",
                table: "ReceivingOrder_Details");

            migrationBuilder.DropIndex(
                name: "IX_ReceivingOrder_Details_ReceiveCustommerID",
                table: "ReceivingOrder_Details");

            migrationBuilder.RenameColumn(
                name: "ReceiveCustommerID",
                table: "ReceivingOrder_Details",
                newName: "ReceiveCustommer");

            migrationBuilder.AddColumn<int>(
                name: "ReceivingOrder_DetailsID",
                table: "ReceiveCustommers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ReceiveCustommers_ReceivingOrder_DetailsID",
                table: "ReceiveCustommers",
                column: "ReceivingOrder_DetailsID");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiveCustommers_ReceivingOrder_Details_ReceivingOrder_DetailsID",
                table: "ReceiveCustommers",
                column: "ReceivingOrder_DetailsID",
                principalTable: "ReceivingOrder_Details",
                principalColumn: "ID");
        }
    }
}
