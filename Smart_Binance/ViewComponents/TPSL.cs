using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Actions.SmartTrade;
using Smart_Binance.Data;
using Smart_Binance.Models;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents.TradeTypes
{
    public class TPSL : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            CalculateAmountDecimal calculate = new CalculateAmountDecimal();

            TradeViewModel tradeView = new TradeViewModel();
            using (var db = new ApplicationDbContext())
            {
                tradeView.Trade = db.Trades.Where(t => t.Id == id).Single();
            }
            int amountDecimal = await calculate.OrderBookDecimal(tradeView.Trade.Market);
            int priceDecimal = await calculate.PriceDecimal(tradeView.Trade.Market);
            GetTrade getTrade = new GetTrade();
            if (tradeView.Trade != null)
            {
                tradeView.Trade.Amount = decimal.Round(tradeView.Trade.Amount, tradeView.Trade.AmountDecimal);
                tradeView.Trade.TakeProfitPrice = decimal.Round(tradeView.Trade.TakeProfitPrice, tradeView.Trade.PriceDecimal);
                tradeView.Trade.StopLossPrice = decimal.Round(tradeView.Trade.StopLossPrice, tradeView.Trade.PriceDecimal);
                tradeView.Trade.BuyPrice = decimal.Round(tradeView.Trade.BuyPrice, tradeView.Trade.PriceDecimal);
                tradeView.CurrentPrice = decimal.Round(await getTrade.CurrentPrice(tradeView.Trade.Market), tradeView.Trade.PriceDecimal);
                tradeView.CurrentPercentage = decimal.Round((tradeView.CurrentPrice - tradeView.Trade.BuyPrice) / tradeView.Trade.BuyPrice * 100, 2);
                tradeView.BelowZeroPercent = tradeView.CurrentPercentage < 0 ? true : false;
                tradeView.VisualPercentage = getTrade.VisualPercent(tradeView.Trade.StopLossPrice, tradeView.Trade.TakeProfitPrice, tradeView.Trade.BuyPrice, tradeView.CurrentPrice);
                tradeView.VisualPercentage = tradeView.VisualPercentage > 100 ? 100m : tradeView.VisualPercentage;
                return View(tradeView);
            }
            else
            {
                return await InvokeAsync(id);
            }

        }
    }
}
