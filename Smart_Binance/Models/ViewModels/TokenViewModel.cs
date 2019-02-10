using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models.ViewModels
{
    public class TokenViewModel
    {
        public string Name { get; set; }
        public decimal LastPrice { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal BaseTotal { get; set; }
        public decimal Amount { get; set; }
        public decimal PercentBaseAmount { get; set; }
        public string BuyType { get; set; }
        public string PercentType { get; set; }
        public string BaseType { get; set; }
        public int AssetDecimalAmount { get; set; }
        public int BaseDecimalAmount { get; set; }
        public int PriceDecimalAmount { get; set; }
        public decimal MinValue { get; set; }
    }
}
