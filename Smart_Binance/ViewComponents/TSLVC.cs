using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Actions.SmartTrade;
using Smart_Binance.Data;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class TSLVC : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id)
        {
            TradeViewModel tradeView = new TradeViewModel();
            using (var db = new ApplicationDbContext())
            {
                tradeView.Trade = db.Trades.Where(t => t.Id == id).Single();
            }
            GetTrade getTrade = new GetTrade();
            tradeView.Trade.Amount = decimal.Round(tradeView.Trade.Amount, tradeView.Trade.AmountDecimal);
            tradeView.Trade.StopLossPrice = decimal.Round(tradeView.Trade.StopLossPrice, tradeView.Trade.PriceDecimal);
            tradeView.Trade.BuyPrice = decimal.Round(tradeView.Trade.BuyPrice, tradeView.Trade.PriceDecimal);
            tradeView.CurrentPrice = decimal.Round(await getTrade.CurrentPrice(tradeView.Trade.Market), tradeView.Trade.PriceDecimal);
            tradeView.CurrentPercentage = tradeView.CurrentPrice - tradeView.Trade.BuyPrice != 0 ? decimal.Round((tradeView.CurrentPrice - tradeView.Trade.BuyPrice) / tradeView.Trade.BuyPrice * 100, 2) : 00.00m;
            tradeView.BelowZeroPercent = tradeView.CurrentPercentage < 0 ? true : false;
            tradeView.VisualPercentage = tradeView.CurrentPercentage != 0 ? getTrade.VisualPercent(tradeView.Trade.StopLossPrice, tradeView.Trade.BuyPrice + (tradeView.Trade.BuyPrice * ((tradeView.CurrentPercentage / 100m) * 2)), tradeView.Trade.BuyPrice, tradeView.CurrentPrice) : 00.00m;
            // Make dynamic by saying if trade visualPercent equal to 75 then recalculate visual percent at take at 2.5 X and so on -- (tradeView.CurrentPercentage / 100m) -- divides by zero no no
            tradeView.VisualPercentage = tradeView.VisualPercentage > 100 ? 100m : tradeView.VisualPercentage;
            return View(tradeView);
        }
    }
}
