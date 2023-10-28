using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class updateComment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Store_StoreId",
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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comment",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)");

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "Comment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "ParentCommentId",
                table: "Comment",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Store_StoreId",
                table: "Comment",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment");

            migrationBuilder.DropForeignKey(
                name: "FK_Comment_Store_StoreId",
                table: "Comment");

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

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Comment",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StoreId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ParentCommentId",
                table: "Comment",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Content",
                table: "Comment",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_AspNetUsers_UserId",
                table: "Comment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comment_Store_StoreId",
                table: "Comment",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
