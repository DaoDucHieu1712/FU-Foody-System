using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class updateReport : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "24997a3d-21f1-4b60-967d-e1e0d0747c66");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "59ad91a3-56b0-4e4c-a1ec-9dd4e5d23065");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "625d0321-7d28-43b2-a21e-deb97f33b7a4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "ff713312-a03f-4cc5-a223-ef3a303bd37e");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e86400b-e8b9-42e1-b90f-39e995f51cf4", "b987ef4a-80aa-45b4-85a2-e726acab7dea", "User", "ApplicationRole", "User", "USER" },
                    { "9b314877-faa5-48b9-8e5b-f70b50c286f2", "e3ca57e0-560a-4f3f-930f-1c38b525c8c0", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "a6a2c4ca-a679-4c79-bc6c-b4b6af690ce2", "126c66ed-1684-4270-8f0d-8ecb1e224e4a", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "f3f32b0a-b6d1-4c6c-9433-6cbe8b770459", "fee9258d-e64c-4c3a-b66d-4a6f25288bfe", "Admin", "ApplicationRole", "Admin", "ADMIN" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e86400b-e8b9-42e1-b90f-39e995f51cf4");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "9b314877-faa5-48b9-8e5b-f70b50c286f2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a6a2c4ca-a679-4c79-bc6c-b4b6af690ce2");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f3f32b0a-b6d1-4c6c-9433-6cbe8b770459");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "24997a3d-21f1-4b60-967d-e1e0d0747c66", "908985f5-4ad7-4770-a252-3822738a9c5e", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "59ad91a3-56b0-4e4c-a1ec-9dd4e5d23065", "7b9aa8b1-e2ef-4f98-9935-c91e3f47d66b", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" },
                    { "625d0321-7d28-43b2-a21e-deb97f33b7a4", "ade3da91-05d6-4300-881b-a89e6c5d28fe", "User", "ApplicationRole", "User", "USER" },
                    { "ff713312-a03f-4cc5-a223-ef3a303bd37e", "12d8c6ee-da4f-4926-bf1d-511d5819b1d4", "Admin", "ApplicationRole", "Admin", "ADMIN" }
                });
        }
    }
}
