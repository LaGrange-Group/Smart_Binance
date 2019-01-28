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

        public async Task<int> PriceDecimal(string market)
        {
            using (var client = new BinanceClient())
            {
                var klineCandles = await client.GetKlinesAsync(market, KlineInterval.FiveteenMinutes,null, null, 5);
                if (klineCandles.Success)
                {
                    int largestDecimalPlace = 0;
                    List<BinanceKline> klines = klineCandles.Data.ToList();
                    foreach (BinanceKline kline in klines)
                    {
                        decimal quantity = Convert.ToDecimal(kline.High.ToString("0.################"));
                        int decimalCount = BitConverter.GetBytes(decimal.GetBits(quantity)[3])[2];
                        if (decimalCount > largestDecimalPlace)
                        {
                            largestDecimalPlace = decimalCount;
                        }
                        quantity = Convert.ToDecimal(kline.Low.ToString("0.################"));
                        decimalCount = BitConverter.GetBytes(decimal.GetBits(quantity)[3])[2];
                        if (decimalCount > largestDecimalPlace)
                        {
                            largestDecimalPlace = decimalCount;
                        }
                        quantity = Convert.ToDecimal(kline.Open.ToString("0.################"));
                        decimalCount = BitConverter.GetBytes(decimal.GetBits(quantity)[3])[2];
                        if (decimalCount > largestDecimalPlace)
                        {
                            largestDecimalPlace = decimalCount;
                        }
                        quantity = Convert.ToDecimal(kline.High.ToString("0.################"));
                        decimalCount = BitConverter.GetBytes(decimal.GetBits(quantity)[3])[2];
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
