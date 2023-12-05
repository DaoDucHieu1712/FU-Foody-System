using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class updateLocation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "Location",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Location_StoreId",
                table: "Location",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_Store_StoreId",
                table: "Location",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_Store_StoreId",
                table: "Location");

            migrationBuilder.DropIndex(
                name: "IX_Location_StoreId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "Location");
        }
    }
}
