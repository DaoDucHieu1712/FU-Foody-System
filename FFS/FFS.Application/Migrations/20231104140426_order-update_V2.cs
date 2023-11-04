using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class orderupdate_V2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "OrderDetail",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_StoreId",
                table: "OrderDetail",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Store_StoreId",
                table: "OrderDetail",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Store_StoreId",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_StoreId",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "OrderDetail");
        }
    }
}
