using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class post : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order");

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

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Order",
                newName: "ShipperId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_UserId",
                table: "Order",
                newName: "IX_Order_ShipperId");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Order",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Order",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "PostId",
                table: "Comment",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Post",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Post", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Post_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Order_CustomerId",
                table: "Order",
                column: "CustomerId");

            migrationBuilder.CreateIndex(
                name: "IX_Comment_PostId",
                table: "Comment",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Post_UserId",
                table: "Post",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment",
                column: "PostId",
                principalTable: "Post",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserId",
                table: "Order",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_CustomerId",
                table: "Order",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_ShipperId",
                table: "Order",
                column: "ShipperId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Post_PostId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_ApplicationUserId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_CustomerId",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Order_AspNetUsers_ShipperId",
                table: "Order");

            migrationBuilder.DropTable(
                name: "Post");

            migrationBuilder.DropIndex(
                name: "IX_Order_ApplicationUserId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_CustomerId",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Comment_PostId",
                table: "Comment");

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

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "PostId",
                table: "Comment");

            migrationBuilder.RenameColumn(
                name: "ShipperId",
                table: "Order",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Order_ShipperId",
                table: "Order",
                newName: "IX_Order_UserId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Order_AspNetUsers_UserId",
                table: "Order",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
