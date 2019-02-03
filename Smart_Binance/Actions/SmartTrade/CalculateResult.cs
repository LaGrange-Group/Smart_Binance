using Smart_Binance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class CalculateResult
    {
        public TradeResult PercentDifference(TradeResult result, Trade trade)
        {
            result.PercentDiff = (result.SellPrice - trade.BuyPrice) / trade.BuyPrice * 100;
            return result;
        }
    }
}
