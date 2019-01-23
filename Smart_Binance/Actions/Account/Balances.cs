using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using Smart_Binance.Models;
using Smart_Binance.Models.DataStructures;

namespace Smart_Binance.Actions
{
    public class Balances
    {
        private API api;
         
        public Balances(API api)
        {
            this.api = api;
        }
        private async Task<List<BinanceBalance>> GetBalances()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(api.Key, api.Secret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var accountInfo = await client.GetAccountInfoAsync();
                if (accountInfo.Success)
                {
                    List<BinanceBalance> balances = accountInfo.Data.Balances.Where(b => b.Total > 0 && b.Asset != "SBTC" && b.Asset != "BCX" && b.Asset != "ETF" && b.Asset != "ONG").ToList();
                    stopwatch.Stop();
                    TimeSpan ts = stopwatch.Elapsed;
                    return balances;
                }
            }
            return null;
        }

        public async  Task<List<BalanceModel>> GetBalanceValues()
        {
            List<BinanceBalance> balances = await GetBalances();
            if (balances != null)
            {
                using (var client = new BinanceClient())
                {
                    decimal bitcoinTotal = 0;
                    var lastPrices = await client.Get24HPricesListAsync();
                    decimal bitcoinPrice = lastPrices.Data.Where(p => p.Symbol == "BTCUSDT").Select(p => p.LastPrice).Single();
                    List<Binance24HPrice> btc = lastPrices.Data.Where(p => p.Symbol.Contains("BTC")).ToList();
                    List<BalanceModel> balanceModels = new List<BalanceModel>();
                    if (lastPrices.Success)
                    {
                        foreach (BinanceBalance balance in balances)
                        {
                            BalanceModel balanceModel = new BalanceModel();
                            if (balance.Asset != "BTC")
                            {
                                decimal bitcoinValue = btc.Where(a => a.Symbol.Contains(balance.Asset)).Select(a => a.LastPrice).Single() * balance.Total;
                                balanceModel.BitcoinValue = bitcoinValue;
                                bitcoinTotal += bitcoinValue;
                            }
                            else
                            {
                                bitcoinTotal += balance.Total;
                                balanceModel.BitcoinValue = balance.Total;
                            }
                            balanceModel.Symbol = balance.Asset;
                            balanceModel.Amount = balance.Total;
                            balanceModel.USDValue = decimal.Round(bitcoinPrice * balanceModel.BitcoinValue, 2);
                            balanceModels.Add(balanceModel);
                        }
                        bitcoinTotal = decimal.Round(bitcoinTotal, 7);
                    }
                    BalanceModel binanceBalance = new BalanceModel()
                    {
                        Symbol = "TotalBitcoin",
                        Amount = bitcoinTotal,
                        USDValue = decimal.Round(bitcoinPrice * bitcoinTotal, 2)
                    };
                    balanceModels.Add(binanceBalance);
                    return balanceModels;
                }
            }
            else
            {
                return null;
            }
        }
    }
}
