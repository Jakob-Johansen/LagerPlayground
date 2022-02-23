using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LagerPlayground.Migrations
{
    public partial class UpdateReceiveRejectedV01 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiveRejectedReasonID",
                table: "ReceiveRejecteds",
                newName: "ReceiveRejectedReasonsID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ReceiveRejectedReasonsID",
                table: "ReceiveRejecteds",
                newName: "ReceiveRejectedReasonID");
        }
    }
}
