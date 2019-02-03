using Binance.Net;
using Binance.Net.Objects;
using Smart_Binance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class Scan
    {
        private BuildTrade build;
        private readonly API api;
        public Scan(BuildTrade build, API api = null)
        {
            this.build = build;
            this.api = api;
        }

        public async Task<Trade> ConclusionTakeProfitAsync(Trade trade)
        {
            bool filled = false;
            while (!filled)
            {
                using (var client = new BinanceClient())
                {
                    var orderStatus = await client.QueryOrderAsync(build.Market, trade.OrderId);
                    if (orderStatus.Success)
                    {
                        if (orderStatus.Data.Status == OrderStatus.Filled)
                        {
                            filled = true;
                            trade.Status = false;
                            trade.Success = true;
                            return trade;
                        }
                        else if (orderStatus.Data.Status == OrderStatus.Canceled)
                        {
                            trade.Success = true;
                            return trade;
                        }
                    }
                }
                System.Threading.Thread.Sleep(5000);
            }
            trade.Status = false;
            trade.Success = false;
            return trade;
        }

        public TradeResult StopLoss(Trade trade)
        {
            bool active = true;
            TradeResult result = new TradeResult();
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Market, KlineInterval.OneMinute, (data) =>
               {
                   if (data.Data.Low < build.StopLossPrice && active)
                   {
                       Cancel cancel = new Cancel();
                       if (cancel.Trade(trade))
                       {
                           Sell sell = new Sell(build, api);
                           result = sell.Market(trade);
                           if (result.Success)
                           {
                               CalculateResult calculate = new CalculateResult();
                               result = calculate.PercentDifference(result, trade);
                               active = false;
                           }
                       }
                   }
               });

                while (active)
                {
                    using (var clientRest = new BinanceClient())
                    {
                        var orderStatus = clientRest.QueryOrder(build.Market, trade.OrderId);
                        if (orderStatus.Success)
                        {
                            if (orderStatus.Data.Status == OrderStatus.Filled)
                            {
                                result.SellPrice = build.TakeProfitPrice;
                                result.SellOrderId = trade.OrderId;
                                result.TradeId = trade.Id;
                                result.EndTime = DateTime.Now;
                                CalculateResult calculate = new CalculateResult();
                                result = calculate.PercentDifference(result, trade);
                                result.Success = true;
                                active = false;
                            }
                        }
                    }
                }
            }
            return result;
        }
    }
}
