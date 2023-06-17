using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SourceFuse.E_commerce.API.Migrations
{
    public partial class CategoryDescUpdate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Categories",
                newName: "Desc");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Desc",
                table: "Categories",
                newName: "Description");
        }
    }
}
