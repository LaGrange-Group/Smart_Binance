using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Actions.SmartTrade;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class Buy : ViewComponent
    {
        public Buy()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(string market = null, string percentType = "button-basepercent-25")
        {

            TokenViewModel token;
            if (market == null)
            {
                token = new TokenViewModel()
                {
                    LastPrice = 0.000m,
                    Amount = 0.000m,
                    BaseAmount = 0.000m,
                    PercentType = percentType
                };
                return View(token);
            }
            else
            {
                GetBuy getBuy = new GetBuy();
                token = await getBuy.Info(market, getBuy.AmountPercent(percentType));
                token.PercentType = "#" + percentType;
                return View(token);
            }
        }
    }
}
