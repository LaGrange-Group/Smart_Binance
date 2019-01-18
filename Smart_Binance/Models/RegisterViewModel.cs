using Smart_Binance.Areas.Identity.Pages.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Models
{
    public class RegisterViewModel
    {
        public RegisterModel  RegisterModel { get; set; }
        public Customer Customer { get; set; }
    }
}
