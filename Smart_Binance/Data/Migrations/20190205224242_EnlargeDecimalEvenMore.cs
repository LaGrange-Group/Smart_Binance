using Microsoft.EntityFrameworkCore.Migrations;

namespace Smart_Binance.Data.Migrations
{
    public partial class EnlargeDecimalEvenMore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TakeProfitPrice",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "StopLossPrice",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Trades",
                type: "decimal(28, 18)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(10, 10)");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TakeProfitPrice",
                table: "Trades",
                type: "decimal(10, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "StopLossPrice",
                table: "Trades",
                type: "decimal(10, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "BuyPrice",
                table: "Trades",
                type: "decimal(10, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 18)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Amount",
                table: "Trades",
                type: "decimal(10, 10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(28, 18)");
        }
    }
}
