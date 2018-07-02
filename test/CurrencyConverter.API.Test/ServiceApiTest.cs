using System.Collections.Generic;
using CurrencyConverter.API.Services;
using CurrencyConverter.API.Utils;
using Moq;
using Shared;
using Xunit;

namespace UnitTest.CurrencyLayer.Application
{
    public class ServiceApiTest
    {
        public ServiceApiTest()
        {
            Singleton.AppSettings.BaseUrl = "";
        }

        [Theory]
        [InlineData("BRL", "USD")]
        [InlineData("USD", "BRL")]
        [InlineData("EUR", "BRL")]
        [InlineData("BRL", "EUR")]
        [InlineData("USD", "EUR")]
        [InlineData("EUR", "USD")]

        public void Get_currency_converterlayer_success(string from, string to)
        {
            var mock = new Mock<IHttpClient>();
            mock.Setup(x => x.GetBaseHttpTask<decimal>(Singleton.AppSettings.BaseUrl, "currency", $"currencyFrom={from}", $"currencyTo={to}"));

            var service = new CurrencyConverterService(mock.Object);

            service.GetCurrency(from, to);
            mock.Verify(x => x.GetBaseHttpTask<decimal>(Singleton.AppSettings.BaseUrl, "currency", $"currencyFrom={from}", $"currencyTo={to}"), Times.Once);
        }

        [Fact]
        public void Get_currencies_converterlayer_success()
        {
            var mock = new Mock<IHttpClient>();
            mock.Setup(x => x.GetBaseHttpTask<Dictionary<string, string>>(Singleton.AppSettings.BaseUrl, "currency/currencies"));

            var service = new CurrencyConverterService(mock.Object);

            service.GetCurrencies();
            mock.Verify(x => x.GetBaseHttpTask<Dictionary<string, string>>(Singleton.AppSettings.BaseUrl, "currency/currencies"), Times.Once);
        }
    }
}