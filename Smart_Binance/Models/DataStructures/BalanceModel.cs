using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models.DataStructures
{
    public class BalanceModel
    {
        public string Symbol { get; set; }
        public decimal Amount { get; set; }
        public decimal BitcoinValue { get; set; }
        public decimal USDValue { get; set; }
    }
}
