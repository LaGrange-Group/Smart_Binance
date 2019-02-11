using Binance.Net;
using Binance.Net.Objects;
using Smart_Binance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class Cancel
    {
        public bool Trade(Trade trade)
        {
            using (var client = new BinanceClient())
            {
                var orderStatus = client.QueryOrder(trade.Market, trade.OrderId);
                if (orderStatus.Success)
                {
                    if (orderStatus.Data.Status == OrderStatus.Canceled)
                    {
                        return true;
                    }
                    if (orderStatus.Data.Status != OrderStatus.Filled)
                    {
                        var cancelOrder = client.CancelOrder(trade.Market, trade.OrderId);
                        if (cancelOrder.Success)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }

        public async Task<bool> TradeAsync(Trade trade)
        {
            using (var client = new BinanceClient())
            {
                var orderStatus = await client.QueryOrderAsync(trade.Market, trade.OrderId);
                if (orderStatus.Success)
                {
                    if (orderStatus.Data.Status == OrderStatus.Canceled)
                    {
                        return true;
                    }
                    if (orderStatus.Data.Status != OrderStatus.Filled)
                    {
                        var cancelOrder = await client.CancelOrderAsync(trade.Market, trade.OrderId);
                        if (cancelOrder.Success)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                return false;
            }
        }
    }
}
