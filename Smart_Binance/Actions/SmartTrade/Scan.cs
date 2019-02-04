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

        public TradeResult MiddleOrderFlip(Trade trade)
        {
            // Take Profit Stop Loss Trailing Stop Loss
            decimal middle = build.TakeProfitPrice - ((build.TakeProfitPrice - build.StopLossPrice) / 2);
            bool active = true;
            bool take = true;
            bool initial = true;
            bool trailingTake = false;
            int priceDecimal = BitConverter.GetBytes(decimal.GetBits(trade.StopLossPrice)[3])[2];
            decimal prevStopLossPrice = build.StopLossPrice;
            decimal percentStop = decimal.Round((build.StopLossPrice - build.Price) / build.Price * -1m, 2);
            TradeResult result = new TradeResult();
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Market, KlineInterval.OneMinute, (data) =>
                {
                    if (build.TrailingStopLoss)
                    {
                        if (((trade.StopLossPrice - data.Data.High) / data.Data.High * -1) > percentStop)
                        {
                            prevStopLossPrice = trade.StopLossPrice;
                            trade.StopLossPrice = decimal.Round(data.Data.High - (data.Data.High * percentStop), priceDecimal);
                            middle = trade.TakeProfitPrice - ((trade.TakeProfitPrice - trade.StopLossPrice) / 2);
                        }
                    }
                    if (data.Data.Low > middle && !take)
                    {
                        if (!build.TrailingTakeProfit)
                        {
                            Cancel cancel = new Cancel();
                            if (cancel.Trade(trade))
                            {
                                Sell sell = new Sell(build, api);
                                trade = sell.Limit(trade, trade.TakeProfitPrice);
                                if (trade.Success)
                                {
                                    take = true;
                                }
                            }
                        }else if (data.Data.High > trade.TakeProfitPrice)
                        {
                            trailingTake = true;
                        }

                    }else if (data.Data.Low <= middle && take || data.Data.Low <= middle && initial || trade.StopLossPrice > prevStopLossPrice && data.Data.Low <= middle )
                    {
                        Cancel cancel = new Cancel();
                        if (cancel.Trade(trade) || initial)
                        {
                            Sell sell = new Sell(build, api);
                            trade = sell.LimitStop(trade, trade.StopLossPrice);
                            if (trade.Success)
                            {
                                take = false;
                            }
                        }
                    }
                    initial = false;
                });

                while (active && !trailingTake)
                {
                    using (var clientRest = new BinanceClient())
                    {
                        var orderStatus = clientRest.QueryOrder(build.Market, trade.OrderId);
                        if (orderStatus.Success)
                        {
                            if (orderStatus.Data.Status == OrderStatus.Filled)
                            {
                                result.SellPrice = orderStatus.Data.Price;
                                result.SellOrderId = trade.OrderId;
                                result.TradeId = trade.Id;
                                result.EndTime = DateTime.Now;
                                CalculateResult calculate = new CalculateResult();
                                result = calculate.PercentDifference(result, trade);
                                result.Success = true;
                                active = false;
                            }
                            // Find way to end if user cancels trade
                        }
                    }
                }
            }
            if (trailingTake == true)
            {
                decimal trailingTakePerc = build.TrailingTakePercent < 0 ? build.TrailingTakePercent * -1m : build.TrailingTakePercent;
                trade.StopLossPrice = decimal.Round(build.TakeProfitPrice - (build.TakeProfitPrice * (trailingTakePerc / 100)), priceDecimal);
                return TrailingStopLoss(trade);
            }
            else
            {
                return result;
            }

        }

        public TradeResult TrailingStopLoss(Trade trade)
        {
            bool active = true;
            int priceDecimal = BitConverter.GetBytes(decimal.GetBits(trade.StopLossPrice)[3])[2];
            decimal prevStopLossPrice = build.StopLossPrice;
            decimal percentStop = decimal.Round((build.StopLossPrice - build.Price) / build.Price * -1, 3);
            TradeResult result = new TradeResult();
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Market, KlineInterval.OneMinute, (data) =>
                {
                    decimal newPercentDiff = decimal.Round((trade.StopLossPrice - data.Data.High) / data.Data.High, 3);
                    newPercentDiff = newPercentDiff < 0 ? newPercentDiff * -1m : newPercentDiff;
                    if (newPercentDiff > percentStop)
                    {
                        trade.StopLossPrice = decimal.Round(data.Data.High - (data.Data.High * percentStop), priceDecimal);
                        Cancel cancel = new Cancel();
                        if (cancel.Trade(trade))
                        {
                            Sell sell = new Sell(build, api);
                            trade = sell.LimitStop(trade, trade.StopLossPrice);
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
                                result.SellPrice = orderStatus.Data.Price;
                                result.SellOrderId = trade.OrderId;
                                result.TradeId = trade.Id;
                                result.EndTime = DateTime.Now;
                                CalculateResult calculate = new CalculateResult();
                                result = calculate.PercentDifference(result, trade);
                                result.Success = true;
                                active = false;
                            }
                            // Find way to end if user cancels trade
                        }
                    }
                }
            }
            return result;
        }

        public TradeResult TrailingTakeProfit(Trade trade)
        {
            bool active = true;
            int priceDecimal = BitConverter.GetBytes(decimal.GetBits(trade.TakeProfitPrice)[3])[2];
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Market, KlineInterval.OneMinute, (data) =>
                {
                    if (data.Data.High >= build.TakeProfitPrice)
                    {
                        active = false;
                    }
                });

                while (active)
                {

                }
            }
            decimal trailingTakePerc = build.TrailingTakePercent < 0 ? build.TrailingTakePercent * -1m : build.TrailingTakePercent;
            trade.StopLossPrice = decimal.Round(build.TakeProfitPrice - (build.TakeProfitPrice * (trailingTakePerc / 100)), priceDecimal);
            return TrailingStopLoss(trade);
        }

        public async Task<TradeResult> ConclusionStopLossAsync(Trade trade)
        {
            bool filled = false;
            TradeResult result = new TradeResult();
            while (!filled)
            {
                using (var client = new BinanceClient())
                {
                    var orderStatus = await client.QueryOrderAsync(build.Market, trade.OrderId);
                    if (orderStatus.Success)
                    {
                        if (orderStatus.Data.Status == OrderStatus.Filled || orderStatus.Data.Status == OrderStatus.Canceled)
                        {
                            result.SellPrice = orderStatus.Data.Status == OrderStatus.Filled ? orderStatus.Data.Price : build.Price;
                            result.SellOrderId = trade.OrderId;
                            result.TradeId = trade.Id;
                            result.EndTime = DateTime.Now;
                            CalculateResult calculate = new CalculateResult();
                            result = calculate.PercentDifference(result, trade);
                            result.Success = true;
                            filled = true;
                        }
                    }
                }
                System.Threading.Thread.Sleep(5000);
            }
            return result;
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


    }
}
