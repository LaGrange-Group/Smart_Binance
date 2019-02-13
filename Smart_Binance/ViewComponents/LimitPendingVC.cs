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
    public class LimitPendingVC : ViewComponent
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
            return View(tradeView);
        }
    }
}
