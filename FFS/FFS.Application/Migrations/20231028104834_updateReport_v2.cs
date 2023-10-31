using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class updateReport_v2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Report",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "TargetId",
                table: "Report",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Description", "Discriminator", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "27457fd6-1b87-4763-a34b-457096e07712", "ff3d9dd9-b22f-42e3-8805-e1bf4dd73566", "User", "ApplicationRole", "User", "USER" },
                    { "6ccef452-4cd3-42c9-abbf-98e564274234", "f35dbd95-8e1a-4e58-bf07-1dd337d31c13", "Shipper", "ApplicationRole", "Shipper", "SHIPPER" },
                    { "968aa216-565f-4c58-85fe-7e4264ee41d1", "2f0d87b5-2467-444a-9d6e-3eae6f762c6d", "Admin", "ApplicationRole", "Admin", "ADMIN" },
                    { "a701e3c3-c912-4e30-8761-bdf82df4e1f0", "b7f686d6-bdfb-4b49-a181-2437baabda2e", "StoreOwner", "ApplicationRole", "StoreOwner", "STOREOWNER" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27457fd6-1b87-4763-a34b-457096e07712");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "6ccef452-4cd3-42c9-abbf-98e564274234");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "968aa216-565f-4c58-85fe-7e4264ee41d1");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a701e3c3-c912-4e30-8761-bdf82df4e1f0");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Report",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<int>(
                name: "TargetId",
                table: "Report",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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
    }
}
