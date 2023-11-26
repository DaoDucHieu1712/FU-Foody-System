using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class RemoveWairdId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WardID",
                table: "Location");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WardID",
                table: "Location",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
