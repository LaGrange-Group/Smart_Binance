using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Actions.SmartTrade;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class StopLossVC : ViewComponent
    {
        public StopLossVC()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(decimal price = 0.000m, string market = "")
        {
            TradeConditionsViewModel tradeConditions = new TradeConditionsViewModel();
            tradeConditions.Price = price;
            CalculateAmountDecimal amountDecimal = new CalculateAmountDecimal();
            tradeConditions.PriceDecimal = market != "" ? await amountDecimal.PriceDecimal(market) : 0;
            return View(tradeConditions);
        }
    }
}
