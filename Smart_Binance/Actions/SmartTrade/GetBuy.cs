﻿using Binance.Net;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class GetBuy
    {
        public async Task<TokenViewModel> Info(string market, decimal amountPercent)
        {
            
            TokenViewModel viewModel = new TokenViewModel();
            viewModel.BaseType = BaseType(market);
            viewModel.MinValue = MinTradeValue(viewModel.BaseType);
            using (var client = new BinanceClient())
            {
                var baseBalance = await client.GetAccountInfoAsync();
                if (baseBalance.Success)
                {
                    CalculateAmountDecimal amountDecimal = new CalculateAmountDecimal();
                    viewModel.BaseDecimalAmount = viewModel.BaseType == "USDT" || viewModel.BaseType == "TUSD" || viewModel.BaseType == "USDC" || viewModel.BaseType == "PAX" ? 2 : await amountDecimal.OrderBookDecimal(viewModel.BaseType + "USDT");
                    viewModel.BaseTotal = decimal.Round(baseBalance.Data.Balances.Where(b => b.Asset == viewModel.BaseType).Select(b => b.Free).Single(), viewModel.BaseDecimalAmount);
                    viewModel.BaseTotal = decimal.Round(viewModel.BaseTotal - (viewModel.BaseTotal * 0.01m), viewModel.BaseDecimalAmount);
                    viewModel.BaseAmount = decimal.Round(viewModel.BaseTotal * amountPercent, viewModel.BaseDecimalAmount);
                    var currentPrice = await client.Get24HPriceAsync(market);
                    if (currentPrice.Success)
                    {
                        viewModel.PriceDecimalAmount = await amountDecimal.PriceDecimal(market);
                        viewModel.AssetDecimalAmount = await amountDecimal.OrderBookDecimal(market);
                        viewModel.Name = market;
                        viewModel.LastPrice = currentPrice.Data.LastPrice;
                        viewModel.Amount = decimal.Round(viewModel.BaseAmount / viewModel.LastPrice, viewModel.AssetDecimalAmount);
                        return viewModel;
                    }
                }
            }
            return null;
        }

        public async Task<TokenViewModel> InfoDeterminedBase(string market, decimal baseAmount)
        {

            TokenViewModel viewModel = new TokenViewModel();
            viewModel.BaseType = BaseType(market);
            viewModel.MinValue = MinTradeValue(viewModel.BaseType);
            using (var client = new BinanceClient())
            {
                var baseBalance = await client.GetAccountInfoAsync();
                if (baseBalance.Success)
                {
                    viewModel.BaseTotal = baseBalance.Data.Balances.Where(b => b.Asset == viewModel.BaseType).Select(b => b.Free).Single();
                    viewModel.BaseAmount = baseAmount;
                    var currentPrice = await client.Get24HPriceAsync(market);
                    if (currentPrice.Success)
                    {
                        CalculateAmountDecimal amountDecimal = new CalculateAmountDecimal();
                        viewModel.Name = market;
                        viewModel.LastPrice = currentPrice.Data.LastPrice;
                        viewModel.Amount = decimal.Round(viewModel.BaseAmount / viewModel.LastPrice, await amountDecimal.OrderBookDecimal(market));
                        return viewModel;
                    }
                }
            }
            return null;
        }

        public decimal AmountPercent(string type)
        {
            switch (type)
            {
                case "button-basepercent-10":
                    return 0.10m;
                case "button-basepercent-25":
                    return 0.25m;
                case "button-basepercent-50":
                    return 0.50m;
                case "button-basepercent-10-limit":
                    return 0.10m;
                case "button-basepercent-25-limit":
                    return 0.25m;
                case "button-basepercent-50-limit":
                    return 0.50m;
                default:
                    return 1m;
            }
        }

        private string BaseType(string market)
        {
            string end = market.Substring(market.Length - 4);
            end = end.Contains("BNB") ? "BNB" : end;
            end = end.Contains("BTC") ? "BTC" : end;
            end = end.Contains("USDT") ? "USDT" : end;
            end = end.Contains("TUSD") ? "TUSD" : end;
            end = end.Contains("USDC") ? "USDC" : end;
            end = end.Contains("PAX") ? "PAX" : end;
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
