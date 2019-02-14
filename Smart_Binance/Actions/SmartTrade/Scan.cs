using Binance.Net;
using Binance.Net.Objects;
using Smart_Binance.Data;
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
            bool canceledSwap = false;
            bool take = true;
            bool initial = true;
            bool trailingTake = false;
            int iterationCanceled = 0;
            int priceDecimal = BitConverter.GetBytes(decimal.GetBits(trade.StopLossPrice)[3])[2];
            decimal prevStopLossPrice = build.StopLossPrice;
            decimal percentStop = decimal.Round((build.StopLossPrice - build.Price) / build.Price * -1m, 2);
            TradeResult result = new TradeResult();
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Market, KlineInterval.OneMinute, (data) =>
                {
                    if (trade.Status && iterationCanceled == 0)
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
                                    canceledSwap = true;
                                    Sell sell = new Sell(build, api);
                                    trade = sell.Limit(trade, trade.TakeProfitPrice);
                                    if (trade.Success)
                                    {
                                        canceledSwap = false;
                                        TradeDB tradeDB = new TradeDB();
                                        tradeDB.Update(trade);
                                        take = true;
                                    }
                                }
                            }
                            else if (data.Data.High > trade.TakeProfitPrice)
                            {
                                trailingTake = true;
                            }

                        }
                        else if (data.Data.Low <= middle && take || data.Data.Low <= middle && initial || trade.StopLossPrice > prevStopLossPrice && data.Data.Low <= middle)
                        {
                            Cancel cancel = new Cancel();
                            if (cancel.Trade(trade) || initial)
                            {
                                canceledSwap = true;
                                Sell sell = new Sell(build, api);
                                trade = sell.LimitStop(trade, trade.StopLossPrice);
                                if (trade.Success)
                                {
                                    canceledSwap = false;
                                    TradeDB tradeDB = new TradeDB();
                                    tradeDB.Update(trade);
                                    take = false;
                                }
                            }
                        }
                        initial = false;
                    }
                   
                });

                while (active && !trailingTake)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        Trade tradeGrab = db.Trades.Where(t => t.Id == trade.Id).Single();
                        if (!tradeGrab.Status)
                        {
                            trade.Status = false;
                        }
                    }
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
                            if (orderStatus.Data.Status == OrderStatus.Canceled && !canceledSwap)
                            {
                                iterationCanceled++;
                                if (iterationCanceled > 4)
                                {
                                    active = false;
                                    trade.Status = false;
                                    TradeDB tradeDB = new TradeDB();
                                    tradeDB.Update(trade);
                                }
                                System.Threading.Thread.Sleep(5000);
                            }
                            else if (orderStatus.Data.Status != OrderStatus.Canceled)
                            {
                                iterationCanceled = 0;
                            }
                        }
                    }
                }
            }
            if (trailingTake == true)
            {
                decimal trailingTakePerc = build.TrailingTakePercent < 0 ? build.TrailingTakePercent * -1m : build.TrailingTakePercent;
                trade.StopLossPrice = decimal.Round(build.TakeProfitPrice - (build.TakeProfitPrice * (trailingTakePerc / 100)), priceDecimal);
                trade.IsTrailingTake = true;
                TradeDB tradeDB = new TradeDB();
                tradeDB.Update(trade);
                return TrailingStopLoss(trade);
            }
            else
            {
                return result;
            }

        }

        public TradeResult TrailingStopLoss(Trade trade)
        {
            bool canceledSwap = false;
            int iterationCanceled = 0;
            bool active = true;
            int priceDecimal = BitConverter.GetBytes(decimal.GetBits(trade.StopLossPrice)[3])[2];
            decimal prevStopLossPrice = build.StopLossPrice;
            decimal percentStop = decimal.Round((build.StopLossPrice - build.Price) / build.Price * -1, 3);
            TradeResult result = new TradeResult();
            using (var client = new BinanceSocketClient())
            {
                var successKline = client.SubscribeToKlineStream(build.Market, KlineInterval.OneMinute, (data) =>
                {
                    if (trade.Status && iterationCanceled == 0)
                    {
                        decimal newPercentDiff = decimal.Round((trade.StopLossPrice - data.Data.High) / data.Data.High, 3);
                        newPercentDiff = newPercentDiff < 0 ? newPercentDiff * -1m : newPercentDiff;
                        if (newPercentDiff > percentStop)
                        {
                            trade.StopLossPrice = decimal.Round(data.Data.High - (data.Data.High * percentStop), priceDecimal);
                            Cancel cancel = new Cancel();
                            if (cancel.Trade(trade))
                            {
                                canceledSwap = true;
                                Sell sell = new Sell(build, api);
                                trade = sell.LimitStop(trade, trade.StopLossPrice);
                                if (trade.Success)
                                {
                                    canceledSwap = false;
                                    TradeDB tradeDB = new TradeDB();
                                    tradeDB.Update(trade);
                                }
                            }
                        }
                    }
                });

                while (active)
                {
                    using (var db = new ApplicationDbContext())
                    {
                        Trade tradeGrab = db.Trades.Where(t => t.Id == trade.Id).Single();
                        if (!tradeGrab.Status)
                        {
                            trade.Status = false;
                        }
                    }
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
                            if (orderStatus.Data.Status == OrderStatus.Canceled && !canceledSwap)
                            {
                                iterationCanceled++;
                                if (iterationCanceled > 4)
                                {
                                    active = false;
                                    trade.Status = false;
                                    TradeDB tradeDB = new TradeDB();
                                    tradeDB.Update(trade);
                                }
                                System.Threading.Thread.Sleep(5000);
                            }
                            else if (orderStatus.Data.Status != OrderStatus.Canceled)
                            {
                                iterationCanceled = 0;
                            }
                        }
                    }
                }
            }
            return result;
        }

        public TradeResult TrailingTakeProfit(Trade trade)
        {
            TradeResult result = new TradeResult();
            bool active = true;
            bool canceled = false;
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
                    using (var clientRest = new BinanceClient())
                    {
                        var orderStatus = clientRest.QueryOrder(build.Market, trade.OrderId);
                        if (orderStatus.Success)
                        {
                            if (orderStatus.Data.Status == OrderStatus.Canceled)
                            {
                                canceled = true;
                                active = false;
                                trade.Status = false;
                                TradeDB tradeDB = new TradeDB();
                                tradeDB.Update(trade);
                            }
                        }
                    }
                }
            }
            if (!canceled)
            {
                decimal trailingTakePerc = build.TrailingTakePercent < 0 ? build.TrailingTakePercent * -1m : build.TrailingTakePercent;
                trade.StopLossPrice = decimal.Round(build.TakeProfitPrice - (build.TakeProfitPrice * (trailingTakePerc / 100)), priceDecimal);
                trade.IsTrailingTake = true;
                TradeDB tradeDB = new TradeDB();
                tradeDB.Update(trade);
                return TrailingStopLoss(trade);
            }
            else
            {
                return result;
            }
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
                        if (orderStatus.Data.Status == OrderStatus.Filled)
                        {
                            result.SellPrice = orderStatus.Data.Status == OrderStatus.Filled ? orderStatus.Data.Price : build.Price;
                            result.SellOrderId = trade.OrderId;
                            result.TradeId = trade.Id;
                            result.EndTime = DateTime.Now;
                            CalculateResult calculate = new CalculateResult();
                            result = calculate.PercentDifference(result, trade);
                            result.Success = true;
                            filled = true;
                        }else if (orderStatus.Data.Status == OrderStatus.Canceled)
                        {
                            filled = true;
                            trade.Status = false;
                            TradeDB tradeDB = new TradeDB();
                            tradeDB.Update(trade);
                        }
                    }
                }
                System.Threading.Thread.Sleep(5000);
            }
            return result;
        }

        public async Task<TradeResult> ConclusionTakeProfitAsync(Trade trade)
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
                        if (orderStatus.Data.Status == OrderStatus.Filled)
                        {
                            result.SellPrice = orderStatus.Data.Status == OrderStatus.Filled ? orderStatus.Data.Price : build.Price;
                            result.SellOrderId = trade.OrderId;
                            result.TradeId = trade.Id;
                            result.EndTime = DateTime.Now;
                            CalculateResult calculate = new CalculateResult();
                            result = calculate.PercentDifference(result, trade);
                            result.Success = true;
                            filled = true;
                            return result;
                        }
                        else if (orderStatus.Data.Status == OrderStatus.Canceled)
                        {
                            filled = true;
                            result.Success = false;
                            return result;
                        }
                    }
                }
                System.Threading.Thread.Sleep(5000);
            }
            return result;
        }


    }
}
