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
    public class BOVC : ViewComponent
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
            tradeView.Trade.BuyPrice = decimal.Round(tradeView.Trade.BuyPrice, tradeView.Trade.PriceDecimal);
            tradeView.CurrentPrice = decimal.Round(await getTrade.CurrentPrice(tradeView.Trade.Market), tradeView.Trade.PriceDecimal);
            tradeView.CurrentPercentage = tradeView.CurrentPrice - tradeView.Trade.BuyPrice != 0 ? decimal.Round((tradeView.CurrentPrice - tradeView.Trade.BuyPrice) / tradeView.Trade.BuyPrice * 100, 2) : 00.00m;
            tradeView.VisualPercentage = getTrade.VisualPercent(tradeView.Trade.BuyPrice - (tradeView.Trade.BuyPrice * 0.75m), tradeView.Trade.BuyPrice * 2, tradeView.Trade.BuyPrice, tradeView.CurrentPrice);
            // Make dynamic by saying if trade visualPercent equal to 75 then recalculate visual percent at take at 2.5 X and so on
            tradeView.VisualPercentage = tradeView.VisualPercentage > 100 ? 100m : tradeView.VisualPercentage;
            return View(tradeView);
        }
    }
}
