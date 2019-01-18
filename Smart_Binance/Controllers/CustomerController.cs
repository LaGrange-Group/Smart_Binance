using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Data;
using Smart_Binance.Models;
using Smart_Binance_API;

namespace Smart_Binance.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ApplicationDbContext db;
        public CustomerController(ApplicationDbContext context)
        {
            db = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(CustomerViewModel customerViewModel)
        {
            Guid g = Guid.NewGuid();
            API api = new API();
            api.Key = customerViewModel.API.Key;
            api.Secret = customerViewModel.API.Secret;
            api.Guid = g.ToString();
            db.APIs.Add(api);
            db.SaveChanges();
            var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            Customer customer = new Customer();
            customer.Name = customerViewModel.Customer.Name;
            customer.PhoneNumber = customerViewModel.Customer.PhoneNumber;
            customer.APIId = db.APIs.Where(a => a.Guid == g.ToString()).Select(a => a.Id).Single();
            customer.UserId = userId;
            db.Customers.Add(customer);

            return View();
        }
    }
}