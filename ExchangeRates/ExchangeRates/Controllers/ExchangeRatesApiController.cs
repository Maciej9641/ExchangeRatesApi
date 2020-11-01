using System;
using System.Collections.Generic;
using ExchangeRates.Config;
using ExchangeRates.Services;
using ExchangeRates.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRates.Controllers
{
    [ApiController]
    [Route("api/exchangeRates")]
    public class ExchangeRatesApiController : ControllerBase
    {
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly IApiKeyGeneratorService _apiKeyGeneratorService;
        private readonly MyConfig _config;

        public ExchangeRatesApiController(IExchangeRatesService exchangeRatesService, IApiKeyGeneratorService apiKeyGeneratorService, MyConfig config)
        {
            _exchangeRatesService = exchangeRatesService;
            _apiKeyGeneratorService = apiKeyGeneratorService;
            _config = config;
        }

        [HttpGet("rates")]
        public IActionResult GetExchangeRates([FromQuery] IDictionary<string, string> currencyCodes, DateTime startDate, DateTime endDate, string apiKey)
        {
            bool cacheRequest = true; //set this to save request to database

            if (apiKey != _config.ApiKey)
            {
                return Unauthorized();
            }

            if (startDate > DateTime.Today || endDate > DateTime.Today)
            {
                return NotFound();
            }

            var response = _exchangeRatesService.GetRatesAsync(currencyCodes, startDate, endDate, cacheRequest);

            if (response.Result.IsSuccessStatusCode)
            {
                return Ok(response.Result.Content.ReadAsStringAsync());
            }
            else
            {
                return BadRequest(response);
            }
        }

        [HttpPost("key")]
        public IActionResult GenerateNewKey()
        {
            var key = _apiKeyGeneratorService.GetApiKey();
            
            return Ok(key);
        }
    }
}
