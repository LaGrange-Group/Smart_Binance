using Binance.Net;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class GetBuy
    {
        public async Task<TokenViewModel> Info(string market, decimal baseAmount = 1.0m)
        {
            TokenViewModel viewModel = new TokenViewModel();
            using (var client = new BinanceClient())
            {
                var currentPrice = await client.Get24HPriceAsync(market);
                if (currentPrice.Success)
                {
                    CalculateAmountDecimal amountDecimal = new CalculateAmountDecimal();
                    viewModel.Name = market;
                    viewModel.LastPrice = currentPrice.Data.LastPrice;
                    viewModel.BaseAmount = baseAmount;
                    viewModel.Amount = decimal.Round(baseAmount / viewModel.LastPrice, await amountDecimal.OrderBookDecimal(market));
                    return viewModel;
                }
            }
            return null;
        }
    }
}
