using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class AddTableReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "165a308c-f59b-4ebb-91b8-83742ecbef08");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8f03c5df-3d31-40fb-9eaa-5dde7898ace9");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6eda3d5-41b2-494c-abaf-fd5620053462");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "d189c5b6-ed45-4c3f-9e43-f7f24f17ef89");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Location",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    ReportType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDelete = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.Id);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Report");

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
                name: "Description",
                table: "Location");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "165a308c-f59b-4ebb-91b8-83742ecbef08", "f0f9a9d9-6e6b-4bd8-a6e5-2971e6fbe659", "User", "ApplicationRole", "User", "USER" },
                    { "8f03c5df-3d31-40fb-9eaa-5dde7898ace9", "a4dfa4be-9003-4185-840e-bb6f5455cba8", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "a6eda3d5-41b2-494c-abaf-fd5620053462", "b68edc35-2978-4991-88c8-fe99740cb85f", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "d189c5b6-ed45-4c3f-9e43-f7f24f17ef89", "4556bbf6-a473-41bd-aff5-84eddcc97ad0", "Admin", "ApplicationRole", "Admin", "ADMIN" }
                });
        }
    }
}
