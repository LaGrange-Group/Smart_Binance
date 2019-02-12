using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Smart_Binance.Models;

namespace Smart_Binance.Actions.SmartTrade
{
    public class Buy
    {
        private readonly BuildTrade build;
        private readonly API api;
        public Buy(BuildTrade build, API api)
        {
            this.build = build;
            this.api = api;
        }
        public async Task<Trade> MarketAsync(Trade trade)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(api.Key, api.Secret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var marketOrder = await client.PlaceOrderAsync(build.Market, OrderSide.Buy, OrderType.Market, build.Amount);
                if (marketOrder.Success)
                {
                    trade.BuyPrice = marketOrder.Data.Fills[0].Price;
                    trade.Amount = build.Amount;
                    trade.Success = true;
                    return trade;
                }
                else
                {
                    var error = marketOrder.Error;
                    trade.Success = false;
                    return trade;
                }
            }
        }

        public async Task<Trade> LimitAsync(Trade trade, decimal price)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(api.Key, api.Secret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var limitOrder = await client.PlaceOrderAsync(build.Market, OrderSide.Buy, OrderType.Limit, build.Amount, null, price, TimeInForce.GoodTillCancel);
                if (limitOrder.Success)
                {
                    trade.BuyPrice = price;
                    trade.Amount = build.Amount;
                    trade.OrderId = limitOrder.Data.OrderId;
                    trade.Success = true;
                    return trade;
                }
                else
                {
                    var error = limitOrder.Error;
                    trade.Success = false;
                    return trade;
                }
            }
        }
    }
}
