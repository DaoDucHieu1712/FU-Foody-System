using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FFS.Application.Migrations
{
    public partial class updateColLcation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DistictID",
                table: "Location",
                newName: "DistrictID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DistrictID",
                table: "Location",
                newName: "DistictID");
        }
    }
}
