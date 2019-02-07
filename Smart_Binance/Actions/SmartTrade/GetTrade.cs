using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;

namespace Smart_Binance.Actions.SmartTrade
{
    public class GetTrade
    {
        public async Task<decimal> CurrentPrice(string market)
        {
            using (var client = new BinanceClient())
            {
                var currentInfo = await client.Get24HPriceAsync(market);
                if (currentInfo.Success)
                {
                    return currentInfo.Data.LastPrice;
                }
            }
            return 0m;
        }

        public decimal VisualPercent(decimal stopLoss, decimal tradeProft, decimal buyPrice, decimal currentPrice)
        {
            decimal diffFromStopLoss = stopLoss - buyPrice;
            decimal diffFromTakeProfit = tradeProft - buyPrice;
            decimal currentDiff = currentPrice - buyPrice;
            decimal currentPercentageResult = currentPrice - buyPrice < 0 ? currentDiff / diffFromStopLoss * 100 : currentDiff / diffFromTakeProfit * 100;
            return currentPercentageResult;
        }
    }
}
