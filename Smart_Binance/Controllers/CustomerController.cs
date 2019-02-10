using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Smart_Binance.Actions;
using Smart_Binance.Data;
using Smart_Binance.Models;

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
        public async Task<IActionResult> Index(CustomerViewModel customerViewModel)
        {
            Connection connection = new Connection();
            bool connected = await connection.Check(customerViewModel.API.Key, customerViewModel.API.Secret);
            if (connected)
            {
                Guid g = Guid.NewGuid();
                API api = new API();
                api.Key = customerViewModel.API.Key;
                api.Secret = customerViewModel.API.Secret;
                api.Guid = g.ToString();
                db.APIs.Add(api);
                await db.SaveChangesAsync();
                var userId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                Customer customer = new Customer();
                customer.Name = customerViewModel.Customer.Name;
                customer.PhoneNumber = customerViewModel.Customer.PhoneNumber;
                customer.APIId = db.APIs.Where(a => a.Guid == g.ToString()).Select(a => a.Id).Single();
                customer.UserId = userId;
                db.Customers.Add(customer);
                await db.SaveChangesAsync();
            }
            return View();
        }
    }
}