using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models.ViewModels
{
    public class TradeViewModel
    {
        public Trade Trade { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal CurrentPercentage { get; set; }
        public decimal VisualPercentage { get; set; }
        public bool BelowZeroPercent { get; set; }
    }
}
