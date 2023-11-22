using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class flashsalev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FlashSaleDetail",
                table: "FlashSaleDetail");

            migrationBuilder.DropIndex(
                name: "IX_FlashSaleDetail_FoodId",
                table: "FlashSaleDetail");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "FlashSales");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "FlashSaleDetail");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "FlashSaleDetail");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "FlashSaleDetail");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "FlashSaleDetail");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlashSaleDetail",
                table: "FlashSaleDetail",
                columns: new[] { "FoodId", "FlashSaleId" });

            migrationBuilder.CreateIndex(
                name: "IX_FlashSaleDetail_FoodId_FlashSaleId",
                table: "FlashSaleDetail",
                columns: new[] { "FoodId", "FlashSaleId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FlashSaleDetail",
                table: "FlashSaleDetail");

            migrationBuilder.DropIndex(
                name: "IX_FlashSaleDetail_FoodId_FlashSaleId",
                table: "FlashSaleDetail");

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "FlashSales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "FlashSaleDetail",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "FlashSaleDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "FlashSaleDetail",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "FlashSaleDetail",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddPrimaryKey(
                name: "PK_FlashSaleDetail",
                table: "FlashSaleDetail",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_FlashSaleDetail_FoodId",
                table: "FlashSaleDetail",
                column: "FoodId");
        }
    }
}
