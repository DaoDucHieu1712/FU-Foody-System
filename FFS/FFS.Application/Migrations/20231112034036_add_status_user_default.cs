using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class add_status_user_default : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "staus",
                table: "AspNetUsers",
                newName: "Staus");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Staus",
                table: "AspNetUsers",
                newName: "staus");
        }
    }
}
