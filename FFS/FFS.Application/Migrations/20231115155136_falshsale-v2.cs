using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class falshsalev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FlashSales_Food_FoodId",
                table: "FlashSales");

            migrationBuilder.DropIndex(
                name: "IX_FlashSales_FoodId",
                table: "FlashSales");

            migrationBuilder.DropColumn(
                name: "Percent",
                table: "FlashSales");

            migrationBuilder.CreateTable(
                name: "FlashSaleDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    FlashSaleId = table.Column<int>(type: "int", nullable: false),
                    PriceAfterSale = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    SalePercent = table.Column<int>(type: "int", nullable: true),
                    NumberOfProductSale = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashSaleDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlashSaleDetail_FlashSales_FlashSaleId",
                        column: x => x.FlashSaleId,
                        principalTable: "FlashSales",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FlashSaleDetail_Food_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlashSaleDetail_FlashSaleId",
                table: "FlashSaleDetail",
                column: "FlashSaleId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashSaleDetail_FoodId",
                table: "FlashSaleDetail",
                column: "FoodId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlashSaleDetail");

            migrationBuilder.AddColumn<int>(
                name: "Percent",
                table: "FlashSales",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FlashSales_FoodId",
                table: "FlashSales",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_FlashSales_Food_FoodId",
                table: "FlashSales",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id");
        }
    }
}
