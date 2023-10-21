using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class nn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Food_Category_CategoryId",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodCombo_Food_FoodId",
                table: "FoodCombo");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Food_FoodId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Store_StoreId1",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Order_OrderId1",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                table: "React");

            migrationBuilder.DropForeignKey(
                name: "FK_React_Comment_CommentId1",
                table: "React");

            migrationBuilder.DropIndex(
                name: "IX_React_CommentId1",
                table: "React");

            migrationBuilder.DropIndex(
                name: "IX_OrderDetail_OrderId1",
                table: "OrderDetail");

            migrationBuilder.DropIndex(
                name: "IX_Inventory_StoreId1",
                table: "Inventory");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6347060e-7515-4f46-99e3-12cdf400d857");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9a58032a-0b72-40f7-a05e-e087670ef386");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d6f799c2-4aaa-47a6-b3bb-a78646bcb05a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e413f934-f60f-4193-8964-849c789fc4c9");

            migrationBuilder.DropColumn(
                name: "CommentId1",
                table: "React");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                table: "OrderDetail");

            migrationBuilder.DropColumn(
                name: "StoreId1",
                table: "Inventory");

            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "08a7cc5a-be90-4f84-8d2d-67c18f2b5d5f", "46ba54fe-2733-42a9-867c-5f85c8390c1d", "User", "ApplicationRole", "User", "USER" },
                    { "302e56e6-8b56-4568-b354-c98414cf4d16", "76f9f99d-45a0-4272-acc1-529b8f18fafb", "Admin", "ApplicationRole", "Admin", "ADMIN" },
                    { "6469dbe1-49ef-48ca-9536-7fd10f7e5bea", "3f1a4290-4bdc-448d-8724-a4f31c54af04", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "a078b1bd-3a8a-47e6-9d82-da709ef5235d", "08267316-3a6c-47a3-aa15-206cc3dcb46c", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comment_CommentId",
                table: "Comment",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Comment_CommentId",
                table: "Comment",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Category_CategoryId",
                table: "Food",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCombo_Food_FoodId",
                table: "FoodCombo",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Food_FoodId",
                table: "Inventory",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                table: "React",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Comment_CommentId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Food_Category_CategoryId",
                table: "Food");

            migrationBuilder.DropForeignKey(
                name: "FK_FoodCombo_Food_FoodId",
                table: "FoodCombo");

            migrationBuilder.DropForeignKey(
                name: "FK_Inventory_Food_FoodId",
                table: "Inventory");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail");

            migrationBuilder.DropForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                table: "React");

            migrationBuilder.DropIndex(
                name: "IX_Comment_CommentId",
                table: "Comment");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "08a7cc5a-be90-4f84-8d2d-67c18f2b5d5f");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "302e56e6-8b56-4568-b354-c98414cf4d16");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6469dbe1-49ef-48ca-9536-7fd10f7e5bea");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a078b1bd-3a8a-47e6-9d82-da709ef5235d");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Comment");

            migrationBuilder.AddColumn<int>(
                name: "CommentId1",
                table: "React",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                table: "OrderDetail",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StoreId1",
                table: "Inventory",
                type: "int",
                nullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "6347060e-7515-4f46-99e3-12cdf400d857", "43cfa24d-d290-4cca-a3dd-a8940be4fb34", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "9a58032a-0b72-40f7-a05e-e087670ef386", "5f98bb9e-89b9-4f56-b547-8ef373faf662", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "d6f799c2-4aaa-47a6-b3bb-a78646bcb05a", "12c7241f-857d-4eeb-9ce5-d7c82f98cbd4", "Admin", "ApplicationRole", "Admin", "ADMIN" },
                    { "e413f934-f60f-4193-8964-849c789fc4c9", "0a5cb195-768b-4074-8c24-f245ba02354d", "User", "ApplicationRole", "User", "USER" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_React_CommentId1",
                table: "React",
                column: "CommentId1");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetail_OrderId1",
                table: "OrderDetail",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Inventory_StoreId1",
                table: "Inventory",
                column: "StoreId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Food_Category_CategoryId",
                table: "Food",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FoodCombo_Food_FoodId",
                table: "FoodCombo",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Food_FoodId",
                table: "Inventory",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Inventory_Store_StoreId1",
                table: "Inventory",
                column: "StoreId1",
                principalTable: "Store",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Food_FoodId",
                table: "OrderDetail",
                column: "FoodId",
                principalTable: "Food",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderDetail_Order_OrderId1",
                table: "OrderDetail",
                column: "OrderId1",
                principalTable: "Order",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_React_AspNetUsers_UserId",
                table: "React",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_React_Comment_CommentId1",
                table: "React",
                column: "CommentId1",
                principalTable: "Comment",
                principalColumn: "Id");
        }
    }
}
