using AutoFixture;
using ExchangeRates.Controllers;
using ExchangeRates.Services;
using System;
using System.Collections.Generic;
using Xunit;
using NSubstitute;
using ExchangeRates.Services.Abstract;
using ExchangeRates.Config;
using Microsoft.AspNetCore.Mvc;

namespace ExchangeRates.Tests
{
    public class ExchangeRatesApiControllerTests
    {
        private readonly IFixture _fixture;
        private readonly IExchangeRatesService _exchangeRatesService;
        private readonly IApiKeyGeneratorService _apiKeyGeneratorService;
        private readonly MyConfig _config;
        
        public ExchangeRatesApiControllerTests()
        {
            _fixture = new Fixture();
            _exchangeRatesService = Substitute.For<IExchangeRatesService>();
            _apiKeyGeneratorService = Substitute.For<IApiKeyGeneratorService>();
            _config = _fixture.Create<MyConfig>();
        }

        [Fact]
        public void GetExchangeRates_Returns_Unauthorized_When_Got_Wrong_Apikey()
        {
            // Arrange
            string apiKeyMock = _fixture.Create<string>();
            IDictionary<string,string> currencyCodesMock = _fixture.Create<IDictionary<string, string>>();
            DateTime startDateMock = _fixture.Create<DateTime>();
            DateTime endDateMock = _fixture.Create<DateTime>();

            ExchangeRatesApiController controller = new ExchangeRatesApiController(_exchangeRatesService, _apiKeyGeneratorService, _config);
            
            // Act
            var result = controller.GetExchangeRates(currencyCodesMock, startDateMock, endDateMock, apiKeyMock);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public void GetExchangeRates_Returns_NotFound_When_Got_Future_StartDate()
        {
            // Arrange
            string apiKeyMock = _fixture.Create<string>();
            IDictionary<string, string> currencyCodesMock = _fixture.Create<IDictionary<string, string>>();
            DateTime startDateMock = DateTime.Today.AddDays(1);
            DateTime endDateMock = _fixture.Create<DateTime>();

            _config.ApiKey = apiKeyMock; // to pass the authorization

            ExchangeRatesApiController controller = new ExchangeRatesApiController(_exchangeRatesService, _apiKeyGeneratorService, _config);

            // Act
            var result = controller.GetExchangeRates(currencyCodesMock, startDateMock, endDateMock, apiKeyMock);

            // Assert
            Assert.NotNull(result);
            Assert.IsType<NotFoundResult>(result);
        }
    }
}
