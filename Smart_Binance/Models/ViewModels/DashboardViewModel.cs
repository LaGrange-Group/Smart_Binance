using Binance.Net.Objects;
using Smart_Binance.Models.DataStructures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models.ViewModels
{
    public class DashboardViewModel
    {
        public List<BalanceModel> Balances { get; set; }
    }
}
