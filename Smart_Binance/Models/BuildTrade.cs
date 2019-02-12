using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models
{
    public class BuildTrade
    {
        public string Market { get; set; }
        public string TradeType { get; set; }
        public decimal Amount { get; set; }
        public decimal Price { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal TakeProfitPrice { get; set; }
        public decimal StopLossPrice { get; set; }
        public decimal TrailingTakePercent { get; set; }
        public decimal MinValue { get; set; }
        public bool StopLoss { get; set; }
        public bool TakeProfit { get; set; }
        public bool TrailingStopLoss { get; set; }
        public bool TrailingTakeProfit { get; set; }
        public int BasePriceDecimal { get; set; }
        public int AmountDecimal { get; set; }
        public int AssetPriceDecimal { get; set; }
    }
}
