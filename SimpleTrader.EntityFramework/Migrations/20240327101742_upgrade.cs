using Microsoft.EntityFrameworkCore.Migrations;

namespace SimpleTrader.EntityFramework.Migrations
{
    public partial class upgrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BoxRFID",
                table: "Boxes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BoxRFID",
                table: "Boxes");
        }
    }
}
