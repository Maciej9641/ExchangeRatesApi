using ExchangeRates.Config;
using ExchangeRates.Services.Abstract;
using System;
using System.Security.Cryptography;

namespace ExchangeRates.Services.Implementation
{
    public class ApiKeyGeneratorService : IApiKeyGeneratorService
    {
        private readonly MyConfig _config;
        public ApiKeyGeneratorService(MyConfig config)
        {
            _config = config;
        }

        public string GetApiKey()
        {
            var key = new byte[32];

            using (var generator = RandomNumberGenerator.Create())
                generator.GetBytes(key);

            var uniqueKey = Convert.ToBase64String(key);

            _config.ApiKey = uniqueKey;

            return uniqueKey;
        }
    }
}
