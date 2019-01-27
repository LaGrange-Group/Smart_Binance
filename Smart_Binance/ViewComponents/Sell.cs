using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Actions.SmartTrade;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class Sell : ViewComponent
    {
        public Sell()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(string market = null)
        {

            TokenViewModel token;
            if (market == null)
            {
                token = new TokenViewModel()
                {
                    LastPrice = 0.000m,
                    Amount = 0.000m
                };
                return View(token);
            }
            else
            {
                GetSell getSell = new GetSell();
                token = await getSell.Info(market);
                return View(token);
            }
        }
    }
}
