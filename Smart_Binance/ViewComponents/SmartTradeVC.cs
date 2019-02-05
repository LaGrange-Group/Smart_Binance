using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Data;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class SmartTradeVC : ViewComponent
    {
        public SmartTradeVC()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            SmartTradesViewModel smartTrades = new SmartTradesViewModel();
            using (var db = new ApplicationDbContext())
            {
                smartTrades.Trades = db.Trades.Where(t => t.Status == true).ToList();
            }
            return View(smartTrades);
        }
    }
}
