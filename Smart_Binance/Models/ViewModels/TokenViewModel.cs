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
        public decimal Amount { get; set; }
    }
}
