using Binance.Net;
using Binance.Net.Objects;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Smart_Binance.Actions
{
    public class Connection
    {
        public async Task<bool> Check(string apiKey, string apiSecret)
        {
            BinanceClient.SetDefaultOptions(new BinanceClientOptions()
            {
                ApiCredentials = new ApiCredentials(apiKey, apiSecret),
                LogVerbosity = LogVerbosity.Debug,
                LogWriters = new List<TextWriter> { Console.Out }
            });
            using (var client = new BinanceClient())
            {
                var accountInfo = await client.GetAccountInfoAsync();
                if (accountInfo.Success)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
