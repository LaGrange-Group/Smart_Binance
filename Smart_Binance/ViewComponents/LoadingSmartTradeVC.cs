using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class LoadingSmartTradeVC : ViewComponent
    {
        public LoadingSmartTradeVC()
        {

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View();
        }
    }
}
