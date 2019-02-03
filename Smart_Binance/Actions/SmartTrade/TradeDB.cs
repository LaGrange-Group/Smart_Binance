using Smart_Binance.Data;
using Smart_Binance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions.SmartTrade
{
    public class TradeDB
    {
        public async Task Add(Trade trade)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Trades.Add(trade);
                await db.SaveChangesAsync();
            }
        }

        public async Task Update(Trade trade)
        {
            using (var db = new ApplicationDbContext())
            {
                db.Update(trade);
                await db.SaveChangesAsync();
            }
        }
    }
}
