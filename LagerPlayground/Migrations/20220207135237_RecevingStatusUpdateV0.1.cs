using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class RecevingStatusUpdateV01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceivingOrder_DetailsID",
                table: "ReceiveStatus",
                newName: "ReceivingOrder_ItemsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceivingOrder_ItemsID",
                table: "ReceiveStatus",
                newName: "ReceivingOrder_DetailsID");
        }
    }
}
