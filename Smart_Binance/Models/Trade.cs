using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models
{
    public class Trade
    {
        [Key]
        public int Id { get; set; }
        public string Market { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal Amount { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal BuyPrice { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal StopLossPrice { get; set; }
        [Column(TypeName = "decimal(28, 18)")]
        public decimal TakeProfitPrice { get; set; }
        public long OrderId { get; set; }
        public DateTime StartTime { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        public int AmountDecimal { get; set; }
        public int PriceDecimal { get; set; }
        public int BasePriceDecimal { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public bool LimitPending { get; set; } 
        public bool Success { get; set; }
        public string DisplayType { get; set; }
        public bool IsTrailingTake { get; set; }
        public bool IsTrailingStop { get; set; }
        public bool IsStopLoss { get; set; }
        public bool IsTakeProfit { get; set; }
    }
}
