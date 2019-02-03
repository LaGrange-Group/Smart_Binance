using Binance.Net;
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
    }
}
