using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_Binance.Data.Migrations
{
    public partial class TradeUpdateDiplayTypeIsTrailing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DisplayType",
                table: "Trades",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrailing",
                table: "Trades",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DisplayType",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "IsTrailing",
                table: "Trades");
        }
    }
}
