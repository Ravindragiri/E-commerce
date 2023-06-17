using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SourceFuse.E_commerce.API.Migrations
{
    public partial class OrderSchemaCardNumberAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CardNumber",
                table: "Orders",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CardNumber",
                table: "Orders");
        }
    }
}
