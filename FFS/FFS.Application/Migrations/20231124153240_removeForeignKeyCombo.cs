using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class removeForeignKeyCombo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodCombo_Combo_ComboId",
                table: "FoodCombo");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodCombo_Food_FoodId",
                table: "FoodCombo");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodCombo_Store_StoreId",
                table: "FoodCombo");

            migrationBuilder.DropIndex(
                name: "IX_FoodCombo_ComboId",
                table: "FoodCombo");

            migrationBuilder.DropIndex(
                name: "IX_FoodCombo_FoodId",
                table: "FoodCombo");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCombo_Store_StoreId",
                table: "FoodCombo",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodCombo_Store_StoreId",
                table: "FoodCombo");

            migrationBuilder.CreateIndex(
                name: "IX_FoodCombo_ComboId",
                table: "FoodCombo",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodCombo_FoodId",
                table: "FoodCombo",
                column: "FoodId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCombo_Combo_ComboId",
                table: "FoodCombo",
                column: "ComboId",
                principalTable: "Combo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCombo_Food_FoodId",
                table: "FoodCombo",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCombo_Store_StoreId",
                table: "FoodCombo",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }
    }
}
