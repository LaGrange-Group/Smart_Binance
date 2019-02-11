using Binance.Net;
using Binance.Net.Objects;
using Microsoft.EntityFrameworkCore;
using Smart_Binance.Actions.SmartTrade;
using Smart_Binance.Data;
using Smart_Binance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions
{
    public class CreateSmartTrade
    {
        private readonly BuildTrade build;
        private API api { get; set; }
        private readonly Customer customer;
        private Trade trade;
        public CreateSmartTrade(BuildTrade build, Customer customer)
        {
            this.build = build;
            this.customer = customer;
            trade = new Trade();
            
            SetAPI();
        }

        private void SetAPI()
        {
            using (var db = new ApplicationDbContext())
            {
                api = db.Customers.Include(c => c.API).Where(c => c.Id == customer.Id).Select(c => c.API).Single();
            }
        }

        public async Task InitializeTrade()
        {
            trade.CustomerId = customer.Id;
            trade.BuyPrice = build.Price;
            trade.TakeProfitPrice = build.TakeProfitPrice;
            trade.StopLossPrice = build.StopLossPrice;
            trade.Market = build.Market;
            trade.Amount = build.Amount;
            trade.Type = build.TradeType;
            trade.AmountDecimal = build.AmountDecimal;
            trade.BasePriceDecimal = build.BasePriceDecimal;
            trade.PriceDecimal = build.AssetPriceDecimal;
            TradeDB tradeDB = new TradeDB();
            await tradeDB.Add(trade);
        }

        public async Task ElicitBuyOrSell()
        {
            if (build.TradeType == "market")
            {
                await PurchaseAtMarket();
            }
            else if (build.TradeType == "limit")
            {
                await PurchaseAtLimit();
            } else if (build.TradeType == "sell")
            {
                trade.Status = true;
                TradeDB tradeDB = new TradeDB();
                await tradeDB.UpdateAsync(trade);
                await ElicitConditions();
            }
        }

        private async Task ElicitConditions()
        {
            var implementConditions = Task.Run(async () => {
                if (!build.StopLoss && build.TakeProfit && !build.TrailingTakeProfit)
                {
                    // Take Profit
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitAsync(trade, build.TakeProfitPrice);
                    if (trade.Success)
                    {
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        Scan scan = new Scan(build);
                        trade = await scan.ConclusionTakeProfitAsync(trade);
                        if (trade.Success)
                        {
                            TradeResult result = new TradeResult();
                            result.TradeId = trade.Id;
                            result.SellPrice = build.TakeProfitPrice;
                            result.EndTime = DateTime.Now;
                            CalculateResult calculate = new CalculateResult();
                            result = calculate.PercentDifference(result, trade);
                            result.Success = true;
                            TradeResultDB resultDB = new TradeResultDB();
                            resultDB.Add(result);
                            await tradeDB.UpdateAsync(trade);
                        }
                    }

                }
                else if (!build.StopLoss && build.TakeProfit && build.TrailingTakeProfit)
                {
                    // Take Profit
                    // Trailing Take Profit
                    int priceDecimal = BitConverter.GetBytes(decimal.GetBits(trade.TakeProfitPrice)[3])[2];
                    decimal tempTakeProfitPrice = decimal.Round(build.TakeProfitPrice * 2, priceDecimal);
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitAsync(trade, tempTakeProfitPrice);
                    if (trade.Success)
                    {
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        Scan scan = new Scan(build, api);
                        TradeResult result = scan.TrailingTakeProfit(trade);
                        if (result.Success)
                        {
                            TradeResultDB resultDB = new TradeResultDB();
                            resultDB.Add(result);
                            trade.Status = false;
                            await tradeDB.UpdateAsync(trade);
                        }
                    }

                }
                else if (build.StopLoss && !build.TrailingStopLoss && !build.TakeProfit)
                {
                    // Stop Loss
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitStopAsync(trade, build.StopLossPrice);
                    if (trade.Success)
                    {
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        Scan scan = new Scan(build, api);
                        TradeResult result = await scan.ConclusionStopLossAsync(trade);
                        if (result.Success)
                        {
                            TradeResultDB resultDB = new TradeResultDB();
                            resultDB.Add(result);
                            trade.Status = false;
                            await tradeDB.UpdateAsync(trade);
                        }
                    }
                }
                else if (build.StopLoss && build.TrailingStopLoss && !build.TakeProfit)
                {
                    // Stop Loss
                    // Trailing Stop Loss
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitStopAsync(trade, trade.StopLossPrice);
                    if (trade.Success)
                    {
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        var trailingStop = Task.Run(async () =>
                        {
                            Scan scan = new Scan(build, api);
                            TradeResult result = scan.TrailingStopLoss(trade);
                            if (result.Success)
                            {
                                TradeResultDB resultDB = new TradeResultDB();
                                resultDB.Add(result);
                                trade.Status = false;
                                await tradeDB.UpdateAsync(trade);
                            }
                        });
                    }

                }
                else if (build.StopLoss && build.TakeProfit && !build.TrailingStopLoss && !build.TrailingTakeProfit)
                {
                    // Stop Loss
                    // Take Profit
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitAsync(trade, build.TakeProfitPrice);
                    if (trade.Success)
                    {
                        trade.DisplayType = "TPSL";
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        var scanStop = Task.Run(async () => {
                            Scan scan = new Scan(build, api);
                            TradeResult result = scan.MiddleOrderFlip(trade);
                            if (result.Success)
                            {
                                TradeResultDB resultDB = new TradeResultDB();
                                resultDB.Add(result);
                                trade.Status = false;
                                await tradeDB.UpdateAsync(trade);
                            }
                        });
                    }
                }
                else if (build.StopLoss && build.TakeProfit && build.TrailingStopLoss && !build.TrailingTakeProfit)
                {
                    // Stop Loss
                    // Take Profit
                    // Trailing Stop Loss
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitAsync(trade, build.TakeProfitPrice);
                    if (trade.Success)
                    {
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        var scanStop = Task.Run(async () => {
                            Scan scan = new Scan(build, api);
                            TradeResult result = scan.MiddleOrderFlip(trade);
                            if (result.Success)
                            {
                                TradeResultDB resultDB = new TradeResultDB();
                                resultDB.Add(result);
                                trade.Status = false;
                                await tradeDB.UpdateAsync(trade);
                            }
                        });
                    }

                }
                else if (build.StopLoss && build.TakeProfit && !build.TrailingStopLoss && build.TrailingTakeProfit)
                {
                    // Stop Loss
                    // Take Profit
                    // Trailing Take Profit
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitAsync(trade, build.TakeProfitPrice);
                    if (trade.Success)
                    {
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        var scanStop = Task.Run(async () => {
                            Scan scan = new Scan(build, api);
                            TradeResult result = scan.MiddleOrderFlip(trade);
                            if (result.Success)
                            {
                                TradeResultDB resultDB = new TradeResultDB();
                                resultDB.Add(result);
                                trade.Status = false;
                                await tradeDB.UpdateAsync(trade);
                            }
                        });
                    }

                }
                else if (build.StopLoss && build.TakeProfit && build.TrailingStopLoss && build.TrailingTakeProfit)
                {
                    // Stop Loss
                    // Take Profit
                    // Trailing Stop Loss
                    // Trailing Take Profit
                    Sell sell = new Sell(build, api);
                    trade = await sell.LimitAsync(trade, build.TakeProfitPrice);
                    if (trade.Success)
                    {
                        TradeDB tradeDB = new TradeDB();
                        await tradeDB.UpdateAsync(trade);
                        var scanStop = Task.Run(async () => {
                            Scan scan = new Scan(build, api);
                            TradeResult result = scan.MiddleOrderFlip(trade);
                            if (result.Success)
                            {
                                TradeResultDB resultDB = new TradeResultDB();
                                resultDB.Add(result);
                                trade.Status = false;
                                await tradeDB.UpdateAsync(trade);
                            }
                        });
                    }

                }
            }); 
        }

        private async Task PurchaseAtLimit()
        {
            var limitBuy = Task.Run(async () =>{
                Buy buy = new Buy(build, api);
                trade = await buy.LimitAsync(trade, build.Price);
                if (trade.Success)
                {
                    trade.LimitPending = true;
                    trade.Status = true;
                    TradeDB tradeDB = new TradeDB();
                    await tradeDB.UpdateAsync(trade);
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
                                    trade.LimitPending = false;
                                    await tradeDB.UpdateAsync(trade);
                                    await ElicitConditions();
                                }
                                else if (orderStatus.Data.Status == OrderStatus.Canceled)
                                {
                                    trade.Success = false;
                                    await tradeDB.UpdateAsync(trade);
                                    return;
                                }
                            }
                        }
                        System.Threading.Thread.Sleep(5000);
                    }
                    return;
                }
                // Add Error Notification
                return;
            });
        }

        private async Task PurchaseAtMarket()
        {
            Buy buy = new Buy(build, api);
            trade = await buy.MarketAsync(trade);
            if (trade.Success)
            {
                trade.Status = true;
                TradeDB tradeDB = new TradeDB();
                await tradeDB.UpdateAsync(trade);
                await ElicitConditions();
                return;
            }
            else
            {
                
            }
            // Add Error Notification
            return;
        }
    }
}
