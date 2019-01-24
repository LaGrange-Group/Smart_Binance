using Binance.Net;
using Binance.Net.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class CalculateAmountDecimal
    {
        public async Task<int> OrderBookDecimal(string market)
        {
            using (var client = new BinanceClient())
            {
                var orderBook = await client.GetRecentTradesAsync(market, 20);
                if (orderBook.Success)
                {
                    int largestDecimalPlace = 0;
                    List<BinanceRecentTrade> trades = orderBook.Data.ToList();
                    foreach (BinanceRecentTrade trade in trades)
                    {
                        decimal quantity = Convert.ToDecimal(trade.Quantity.ToString("0.################"));
                        int decimalCount = BitConverter.GetBytes(decimal.GetBits(quantity)[3])[2];
                        if (decimalCount > largestDecimalPlace)
                        {
                            largestDecimalPlace = decimalCount;
                        }
                    }
                    return largestDecimalPlace;
                }
                return 0;
            }
        }
    }
}
