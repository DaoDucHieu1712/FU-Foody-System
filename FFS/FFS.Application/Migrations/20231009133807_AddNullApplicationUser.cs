using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class AddNullApplicationUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "229019d3-4e88-42ec-9d4c-b834413a7215");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2e84f51e-8d0b-45f6-9f22-1d3a4556f6cd");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "3a4c795e-3443-4a83-92e5-95c688951184");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "97146b8f-ba2f-491a-91bb-f60be3b11377");

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<bool>(
                name: "Allow",
                table: "AspNetUsers",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "LastName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Gender",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FirstName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "BirthDay",
                table: "AspNetUsers",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Avatar",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "Allow",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "229019d3-4e88-42ec-9d4c-b834413a7215", "cd3f896a-dd72-415d-a0c9-ab785d1c6679", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "2e84f51e-8d0b-45f6-9f22-1d3a4556f6cd", "cb41977a-e430-4905-b2f7-df118a866162", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "3a4c795e-3443-4a83-92e5-95c688951184", "5e6b1f6f-de29-4a13-829f-bb7ecc23a165", "Admin", "ApplicationRole", "Admin", "ADMIN" },
                    { "97146b8f-ba2f-491a-91bb-f60be3b11377", "0c3a85a0-5e62-49e3-b55b-540ab939154b", "User", "ApplicationRole", "User", "USER" }
                });
        }
    }
}
