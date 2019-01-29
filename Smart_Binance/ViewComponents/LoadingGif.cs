using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.ViewComponents
{
    public class LoadingGif : ViewComponent
    {
        public LoadingGif()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(string type)
        {
            LoadingGifViewModel loadingGif = new LoadingGifViewModel();
            loadingGif.Type = type;
            return View(loadingGif);
        }
    }
}
