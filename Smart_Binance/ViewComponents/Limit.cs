using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Actions.SmartTrade;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class Limit : ViewComponent
    {
        public Limit()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(string market = null, string percentType = "button-basepercent-25", decimal baseAmount = 0m)
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
                token = baseAmount == 0 ? await getBuy.Info(market, getBuy.AmountPercent(percentType)) : await getBuy.InfoDeterminedBase(market, Convert.ToDecimal(baseAmount));
                token.PercentType = baseAmount == 0 ? "#" + percentType : "";
                return View(token);
            }
        }
    }
}
