using Smart_Binance.Data;
using Smart_Binance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class TradeResultDB
    {
        public async Task AddAsync(TradeResult result)
        {
            using (var db = new ApplicationDbContext())
            {
                db.TradeResults.Add(result);
                await db.SaveChangesAsync();
            }
        }
        public void Add(TradeResult result)
        {
            using (var db = new ApplicationDbContext())
            {
                db.TradeResults.Add(result);
                db.SaveChanges();
            }
        }

        public async Task UpdateAsync(TradeResult result)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Update(result);
                await db.SaveChangesAsync();
            }
        }

        public void Update(TradeResult result)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Update(result);
                db.SaveChanges();
            }
        }
    }
}
