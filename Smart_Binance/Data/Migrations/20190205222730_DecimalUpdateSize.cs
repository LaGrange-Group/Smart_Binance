using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_Binance.Data.Migrations
{
    public partial class DecimalUpdateSize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TakeProfitPrice",
                table: "Trades",
                type: "decimal(8, 8)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "StopLossPrice",
                table: "Trades",
                type: "decimal(8, 8)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Trades",
                type: "decimal(8, 8)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Trades",
                type: "decimal(8, 8)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "SellPrice",
                table: "TradeResults",
                type: "decimal(8, 8)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentDiff",
                table: "TradeResults",
                type: "decimal(8, 8)",
                nullable: false,
                oldClrType: typeof(decimal));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TakeProfitPrice",
                table: "Trades",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "StopLossPrice",
                table: "Trades",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Trades",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Trades",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellPrice",
                table: "TradeResults",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 8)");

            migrationBuilder.AlterColumn<decimal>(
                name: "PercentDiff",
                table: "TradeResults",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(8, 8)");
        }
    }
}
