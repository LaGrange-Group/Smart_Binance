using Binance.Net;
using Binance.Net.Objects;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class GetSell
    {
        public async Task<TokenViewModel> Info(string market)
        {

            TokenViewModel viewModel = new TokenViewModel();
            viewModel.BaseType = BaseType(market);
            viewModel.MinValue = MinTradeValue(viewModel.BaseType);
            string asset = market.Replace(viewModel.BaseType, "");
            using (var client = new BinanceClient())
            {
                var balances = await client.GetAccountInfoAsync();
                if (balances.Success)
                {
                    CalculateAmountDecimal amountDecimal = new CalculateAmountDecimal();
                    BinanceBalance balance = balances.Data.Balances.Where(b => b.Asset == asset).Single();
                    viewModel.BaseDecimalAmount = viewModel.BaseType == "USDT" || viewModel.BaseType == "TUSD" || viewModel.BaseType == "USDC" || viewModel.BaseType == "PAX" ? 2 : await amountDecimal.OrderBookDecimal(viewModel.BaseType + "USDT");
                    var currentPrice = await client.Get24HPriceAsync(market);
                    if (currentPrice.Success)
                    {
                        viewModel.PriceDecimalAmount = await amountDecimal.PriceDecimal(market);
                        viewModel.AssetDecimalAmount = await amountDecimal.OrderBookDecimal(market);
                        viewModel.Name = market;
                        viewModel.Amount = decimal.Round(balance.Free, viewModel.AssetDecimalAmount);
                        viewModel.LastPrice = currentPrice.Data.LastPrice;
                        return viewModel;
                    }
                }
            }
            return viewModel;
        }
        private string BaseType(string market)
        {
            string end = market.Substring(market.Length - 4);
            end = end.Contains("BNB") ? "BNB" : end;
            end = end.Contains("BTC") ? "BTC" : end;
            end = end.Contains("USDT") ? "USDT" : end;
            end = end.Contains("ETH") ? "ETH" : end;
            end = end.Contains("XRP") ? "XRP" : end;
            return end;
        }

        private decimal MinTradeValue(string type)
        {
            switch (type)
            {
                case "BTC":
                    return 0.001m;
                case "BNB":
                    return 1m;
                case "ETH":
                    return 0.01m;
                default:
                    return 10m;
            }
        }
    }
}
