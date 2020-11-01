using ExchangeRates.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace ExchangeRates.Services
{
    public class ExchangeRatesService : IExchangeRatesService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IApiRequestsRepository _apiRequestsRepository;

        public ExchangeRatesService(IHttpClientFactory clientFactory, IApiRequestsRepository apiRequestsRepository)
        {
            _clientFactory = clientFactory;
            _apiRequestsRepository = apiRequestsRepository;
        }

        public async Task<HttpResponseMessage> GetRatesAsync(IDictionary<string, string> currencyCodes, DateTime startDate, DateTime endDate, bool cacheRequest)
        {
            if (startDate == endDate && startDate.DayOfWeek == DayOfWeek.Saturday || startDate.DayOfWeek == DayOfWeek.Sunday)
            {
                if (startDate.DayOfWeek == DayOfWeek.Saturday)
                {
                    startDate = startDate.AddDays(-1);
                }
                else
                {
                    startDate = startDate.AddDays(-2);
                }

                endDate = startDate;
            }

            var fixedStartDate = startDate.ToString("yyyy-MM-dd");
            var fixedEndDate = endDate.ToString("yyyy-MM-dd");

            var request = new HttpRequestMessage(HttpMethod.Get, "https://sdw-wsrest.ecb.europa.eu/service/data/EXR/D." + currencyCodes.First().Key + "." + currencyCodes.First().Value + "..?startPeriod=" + fixedStartDate + "&endPeriod=" + fixedEndDate);

            if (cacheRequest)
            {
               await _apiRequestsRepository.SaveApiRequestToDatabase(request);
            }

            var client = _clientFactory.CreateClient();

            var response = await client.GetAsync(request.RequestUri);

            return response;
        }
    }
}
