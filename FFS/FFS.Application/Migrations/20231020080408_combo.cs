using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class combo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Food_Combo_ComboId",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Wishlist_WishlistId",
                table: "Food");

            migrationBuilder.DropIndex(
                name: "IX_Food_ComboId",
                table: "Food");

            migrationBuilder.DropIndex(
                name: "IX_Food_WishlistId",
                table: "Food");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21a40c99-7e4c-4768-8a3d-f4db8f4c63e2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3fa51a77-ca74-4501-8ae6-972ddbf4f420");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "5f0cc353-19c1-4f1c-aebd-fa4dfab76ca2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e1d96487-a80a-4c77-ac91-6d149234cf3d");

            migrationBuilder.DropColumn(
                name: "ComboId",
                table: "Food");

            migrationBuilder.DropColumn(
                name: "WishlistId",
                table: "Food");

            migrationBuilder.AddColumn<int>(
                name: "FoodId",
                table: "Wishlist",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PhoneNumber",
                table: "Location",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Receiver",
                table: "Location",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "FoodCombo",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StoreId = table.Column<int>(type: "int", nullable: false),
                    FoodId = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodCombo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FoodCombo_Food_FoodId",
                        column: x => x.FoodId,
                        principalTable: "Food",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FoodCombo_Store_StoreId",
                        column: x => x.StoreId,
                        principalTable: "Store",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "2ab9a210-bbb9-4600-beb6-e967f8bcd817", "9a78465d-d3d8-4a1b-a459-1563a860e42c", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "6441d09b-1fec-4c9b-b8eb-51802abd32cf", "ac0dcb28-691d-45fb-ae63-86878d79ce3e", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "99efe734-8e8f-4349-bf12-4d17daccbdc1", "973a50ca-0ab4-424c-b883-f7cc270a4037", "Admin", "ApplicationRole", "Admin", "ADMIN" },
                    { "d51efd8d-5ce0-4887-8aed-b635caba805a", "15c4c0f2-578f-4444-8cdc-22f7ad1520f2", "User", "ApplicationRole", "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Wishlist_FoodId",
                table: "Wishlist",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodCombo_FoodId",
                table: "FoodCombo",
                column: "FoodId");

            migrationBuilder.CreateIndex(
                name: "IX_FoodCombo_StoreId",
                table: "FoodCombo",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_Wishlist_Food_FoodId",
                table: "Wishlist",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wishlist_Food_FoodId",
                table: "Wishlist");

            migrationBuilder.DropTable(
                name: "FoodCombo");

            migrationBuilder.DropIndex(
                name: "IX_Wishlist_FoodId",
                table: "Wishlist");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2ab9a210-bbb9-4600-beb6-e967f8bcd817");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6441d09b-1fec-4c9b-b8eb-51802abd32cf");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "99efe734-8e8f-4349-bf12-4d17daccbdc1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d51efd8d-5ce0-4887-8aed-b635caba805a");

            migrationBuilder.DropColumn(
                name: "FoodId",
                table: "Wishlist");

            migrationBuilder.DropColumn(
                name: "PhoneNumber",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "Receiver",
                table: "Location");

            migrationBuilder.AddColumn<int>(
                name: "ComboId",
                table: "Food",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WishlistId",
                table: "Food",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21a40c99-7e4c-4768-8a3d-f4db8f4c63e2", "b830ba24-d7d9-4c31-a56c-0bc9def5c9b1", "Admin", "ApplicationRole", "Admin", "ADMIN" },
                    { "3fa51a77-ca74-4501-8ae6-972ddbf4f420", "b1f7d074-b381-41d7-bf7b-1b75bd4a3a15", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "5f0cc353-19c1-4f1c-aebd-fa4dfab76ca2", "4b9808f6-9e47-45f1-a7b3-7c0e8fbc4b87", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "e1d96487-a80a-4c77-ac91-6d149234cf3d", "95559893-6b25-4391-8681-47df122a712c", "User", "ApplicationRole", "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Food_ComboId",
                table: "Food",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_Food_WishlistId",
                table: "Food",
                column: "WishlistId");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Combo_ComboId",
                table: "Food",
                column: "ComboId",
                principalTable: "Combo",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Wishlist_WishlistId",
                table: "Food",
                column: "WishlistId",
                principalTable: "Wishlist",
                principalColumn: "Id");
        }
    }
}
