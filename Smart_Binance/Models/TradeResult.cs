using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models
{
    public class TradeResult
    {
        [Key]
        public int Id { get; set; }
        public decimal SellPrice { get; set; }
        public long SellOrderId { get; set; }
        public decimal PercentDiff { get; set; }
        public DateTime EndTime { get; set; }
        [ForeignKey("Trade")]
        public int TradeId { get; set; }
        public Trade Trade { get; set; }
        public bool Success { get; set; }
    }
}
