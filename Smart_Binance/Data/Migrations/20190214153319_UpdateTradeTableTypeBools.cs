using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_Binance.Data.Migrations
{
    public partial class UpdateTradeTableTypeBools : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsTrailing",
                table: "Trades",
                newName: "IsTrailingTake");

            migrationBuilder.AddColumn<bool>(
                name: "IsStopLoss",
                table: "Trades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTakeProfit",
                table: "Trades",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsTrailingStop",
                table: "Trades",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsStopLoss",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "IsTakeProfit",
                table: "Trades");

            migrationBuilder.DropColumn(
                name: "IsTrailingStop",
                table: "Trades");

            migrationBuilder.RenameColumn(
                name: "IsTrailingTake",
                table: "Trades",
                newName: "IsTrailing");
        }
    }
}
