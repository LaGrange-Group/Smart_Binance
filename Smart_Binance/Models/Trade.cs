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
        public decimal Amount { get; set; }
        public decimal BuyPrice { get; set; }
        public decimal StopLossPrice { get; set; }
        public decimal TakeProfitPrice { get; set; }
        public long OrderId { get; set; }
        public DateTime StartTime { get; set; }
        public bool Status { get; set; }
        public string Type { get; set; }
        [ForeignKey("Customer")]
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public bool LimitPending { get; set; } 
        public bool Success { get; set; }
        public string DisplayType { get; set; }
        public bool IsTrailing { get; set; }
    }
}
