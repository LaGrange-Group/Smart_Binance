using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class TakeProfitVC : ViewComponent
    {
        public TakeProfitVC()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(decimal price = 0.000m)
        {
            TradeConditionsViewModel tradeConditions = new TradeConditionsViewModel();
            tradeConditions.Price = price;
            return View(tradeConditions);
        }
    }
}
