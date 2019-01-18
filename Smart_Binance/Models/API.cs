using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models
{
    public class API
    {
        [Key]
        public int Id { get; set; }
        public string Key { get; set; }
        public string Secret { get; set; }
        public string Guid { get; set; }
    }
}
