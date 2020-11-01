using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates.Services
{
    public interface IExchangeRatesService
    {
        Task<HttpResponseMessage> GetRatesAsync(IDictionary<string, string> currencyCodes, DateTime startDate, DateTime endDate, bool cacheRequest);
    }
}
