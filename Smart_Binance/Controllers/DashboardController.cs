﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            return View(dashboard);
        }
    }
}