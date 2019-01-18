using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_Binance.Data.Migrations
{
    public partial class APIGUID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Guid",
                table: "APIs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Guid",
                table: "APIs");
        }
    }
}
