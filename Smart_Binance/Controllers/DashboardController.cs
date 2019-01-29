using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Binance.Net.Objects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Smart_Binance.Actions;
using Smart_Binance.Data;
using Smart_Binance.Models;
using Smart_Binance.Models.ViewModels;

namespace Smart_Binance.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext db;
        public DashboardController(ApplicationDbContext context)
        {
            db = context;
        }
        public async Task<IActionResult> Index()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            DashboardViewModel dashboard = new DashboardViewModel();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            API api = db.Customers.Include(c => c.API).Where(c => c.UserId == userId).Select(a => a.API).Single();
            Balances balances = new Balances(api);
            dashboard.Balances = await balances.GetBalanceValues();
            stopwatch.Stop();
            TimeSpan ts = stopwatch.Elapsed;
            List<string> Symbols = new List<string> { "ZILBTC", "TRXBTC", "BNBBTC", "BTCUSDT", "XZCXRP", "XRPUSDT", "WAVESETH", "ADABNB" };
            ViewBag.Markets = new SelectList(Symbols);
            return View(dashboard);
        }



        public IActionResult GetMySellViewComponent(string marketPass)
        {
            return ViewComponent("Sell", new { market = marketPass });
        }

        public IActionResult ResetStopLoss()
        {
            return ViewComponent("ActivateStopVC");
        }

        public IActionResult ResetTakeProfit()
        {
            return ViewComponent("ActivateTakeVC");
        }
        public IActionResult GetMyMarketViewComponent(string marketPass, string percentTypePass = "button-basepercent-25")
        {
            return ViewComponent("Market", new { market = marketPass, percentType = percentTypePass });
        }
        public IActionResult SwitchToMarketViewComponent(string marketPass, string percentTypePass = "button-basepercent-25", decimal currentBasePass = 0m)
        {
            return ViewComponent("Market", new { market = marketPass, percentType = percentTypePass, baseAmount = currentBasePass });
        }
        public IActionResult GetMyLimitViewComponent(string marketPass, string percentTypePass = "button-basepercent-25")
        {
            return ViewComponent("Limit", new { market = marketPass, percentType = percentTypePass });
        }

        public IActionResult SwitchToLimitViewComponent(string marketPass, string percentTypePass = "button-basepercent-25", decimal currentBasePass = 0m)
        {
            return ViewComponent("Limit", new { market = marketPass, percentType = percentTypePass, baseAmount = currentBasePass });
        }

        public IActionResult UpdateTakeProfitViewComponent(decimal pricePass, string marketPass)
        {
            return ViewComponent("TakeProfitVC", new { price = pricePass, market = marketPass});
        }

        public IActionResult UpdateStopLossViewComponent(decimal pricePass, string marketPass)
        {
            return ViewComponent("StopLossVC", new { price = pricePass, market = marketPass });
        }

        public IActionResult LoadingGif(string typePass)
        {
            return ViewComponent("LoadingGif", new { type = typePass });
        }
    }
}