using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class Add_ComboFood : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4cb4c81a-b027-492f-93b7-b720a60b6cb2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "61d235f2-d6a9-4c53-aadb-2e8b07f32bd4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "72e3bb62-ff7a-49fe-9608-86bf22680da0");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "cb5f6a06-f5ba-49cd-9835-2cced428a6c5");

            migrationBuilder.AddColumn<int>(
                name: "ComboId",
                table: "FoodCombo",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_FoodCombo_ComboId",
                table: "FoodCombo",
                column: "ComboId");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCombo_Combo_ComboId",
                table: "FoodCombo",
                column: "ComboId",
                principalTable: "Combo",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FoodCombo_Combo_ComboId",
                table: "FoodCombo");

            migrationBuilder.DropIndex(
                name: "IX_FoodCombo_ComboId",
                table: "FoodCombo");

            migrationBuilder.DropColumn(
                name: "ComboId",
                table: "FoodCombo");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4cb4c81a-b027-492f-93b7-b720a60b6cb2", "c006e02f-bd9d-4362-ad1e-1dc085177dab", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "61d235f2-d6a9-4c53-aadb-2e8b07f32bd4", "d2042c1d-31bf-4a33-925a-023b7766c654", "User", "ApplicationRole", "User", "USER" },
                    { "72e3bb62-ff7a-49fe-9608-86bf22680da0", "3bd0e5ef-088d-4eaa-b6e2-e22774c31ab4", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "cb5f6a06-f5ba-49cd-9835-2cced428a6c5", "2f007a69-0243-49e0-b3bb-cef852080648", "Admin", "ApplicationRole", "Admin", "ADMIN" }
                });
        }
    }
}
