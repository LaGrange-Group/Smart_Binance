using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Smart_Binance.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class Sell
    {
        private readonly BuildTrade build;
        private readonly API api;
        public Sell(BuildTrade build, API api)
        {
            this.build = build;
            this.api = api;
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
                var limitOrder = await client.PlaceOrderAsync(build.Market, OrderSide.Sell, OrderType.Limit, build.Amount, null, price, TimeInForce.GoodTillCancel);
                if (limitOrder.Success)
                {
                    trade.TakeProfitPrice = price;
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

        public Trade Limit(Trade trade, decimal price)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(api.Key, api.Secret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var limitOrder = client.PlaceOrder(build.Market, OrderSide.Sell, OrderType.Limit, build.Amount, null, price, TimeInForce.GoodTillCancel);
                if (limitOrder.Success)
                {
                    trade.TakeProfitPrice = price;
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

        public Trade LimitStop(Trade trade, decimal price)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(api.Key, api.Secret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var limitOrder = client.PlaceOrder(build.Market, OrderSide.Sell, OrderType.StopLossLimit, build.Amount, null, price, TimeInForce.GoodTillCancel, price);
                if (limitOrder.Success)
                {
                    trade.StopLossPrice = price;
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

        public async Task<Trade> LimitStopAsync(Trade trade, decimal price)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(api.Key, api.Secret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var limitOrder = await client.PlaceOrderAsync(build.Market, OrderSide.Sell, OrderType.StopLossLimit, build.Amount, null, price, TimeInForce.GoodTillCancel, price);
                if (limitOrder.Success)
                {
                    trade.StopLossPrice = price;
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
                var marketOrder = await client.PlaceOrderAsync(build.Market, OrderSide.Sell, OrderType.Market, build.Amount);
                if (marketOrder.Success)
                {
                    trade.BuyPrice = marketOrder.Data.Fills[0].Price;
                    trade.Amount = build.Amount;
                    trade.Success = true;
                    return trade;
                }
            }
            return null;
        }

        public TradeResult Market(Trade trade)
        {
            TradeResult result = new TradeResult();
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(api.Key, api.Secret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var marketOrder = client.PlaceOrder(build.Market, OrderSide.Sell, OrderType.Market, trade.Amount);
                if (marketOrder.Success)
                {
                    
                    result.SellPrice = marketOrder.Data.Fills[0].Price;
                    result.SellOrderId = marketOrder.Data.OrderId;
                    result.EndTime = DateTime.Now;
                    result.TradeId = trade.Id;
                    result.Success = true;
                    return result;
                }
            }
            result.Success = false;
            return result;
        }
    }
}
