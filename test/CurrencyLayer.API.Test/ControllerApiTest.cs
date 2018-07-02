using System;
using System.Collections.Generic;
using CurrencyLayer.API.Controllers;
using CurrencyLayer.API.Models;
using CurrencyLayer.API.Services;
using Moq;
using Xunit;

namespace CurrencyLayer.API.Test
{
    public class ControllerApiTest
    {
        public ControllerApiTest()
        {
        }


        private static ListCurrencies GetCurrenciesFake()
        {
            return new ListCurrencies
            {
                Currencies = new Dictionary<string, string>
                {
                    {"AED", "United Arab Emirates Dirham"},
                    {"BRL", "Brazilian Real"},
                    {"USD", "United States Dollar"}
                }
            };
        }

        [Theory]
        [InlineData("BRL", "EUR", 2.5, "BRL", 1, 0.4)]
        [InlineData("BRL", "BRL", 1, "EUR", 2.5, 2.5)]
        [InlineData("BRL", "USD", 2, "BRL", 1, 0.5)]
        [InlineData("BRL", "BRL", 1, "USD", 2, 2)]
        [InlineData("BRL", "EUR", 2.5, "USD", 2, 0.8)]
        [InlineData("BRL", "USD", 2, "EUR", 2.5, 1.25)]
        [InlineData("USD", "EUR", 1.25, "BRL", 0.5, 0.4)]
        [InlineData("USD", "BRL", 0.5, "EUR", 1.25, 2.5)]
        [InlineData("USD", "USD", 1, "BRL", 0.5, 0.5)]
        [InlineData("USD", "BRL", 0.5, "USD", 1, 2)]
        [InlineData("USD", "EUR", 1.25, "USD", 1, 0.8)]
        [InlineData("USD", "USD", 1, "EUR", 1.25, 1.25)]
        [InlineData("EUR", "EUR", 1, "BRL", 0.4, 0.4)]
        [InlineData("EUR", "BRL", 0.4, "EUR", 1, 2.5)]
        [InlineData("EUR", "USD", 0.8, "BRL", 0.4, 0.5)]
        [InlineData("EUR", "BRL", 0.4, "USD", 0.8, 2)]
        [InlineData("EUR", "EUR", 1, "USD", 0.8, 0.8)]
        [InlineData("EUR", "USD", 0.8, "EUR", 1, 1.25)]

        public void Get_currency_currencylayer_success(string source, string from, decimal fromSourceValue, string to, decimal toSourceValue, decimal resultExpected)
        {
            var fakeGetCurrency = new Currency
            {
                Success = true,
                Quotes = new Dictionary<string, decimal>(),
                Source = source
            };

            if (source != from)
                fakeGetCurrency.Quotes.Add(source + from, fromSourceValue);

            if (source != to)
                fakeGetCurrency.Quotes.Add(source + to, toSourceValue);

            var mock = new Mock<ICurrencyLayerService>();
            mock.Setup(x => x.GetCurrency())
                .Returns(fakeGetCurrency);

            var currencyController = new CurrencyController(mock.Object);
            var currency = currencyController.GetCurrency(from, to);

            Assert.Equal(resultExpected, currency);
        }


        [Theory]
        [InlineData(null, null)]
        [InlineData("EUR", null)]
        [InlineData(null, "EUR")]
        [InlineData("", "")]
        [InlineData("EUR", "")]
        [InlineData("", "EUR")]
        [InlineData("XXX", "XXX")]
        [InlineData("", "XXX")]
        [InlineData("XXX", "")]
        public void Get_currency_currencylayer_ThrowsArgumentException(string from, string to)
        {
            var fakeGetCurrency = new Currency
            {
                Success = true,
                Quotes = new Dictionary<string, decimal>
                {
                    {"EUR", 1.25m},
                    {"BRL", 0.5m}
                },
                Source = "USD"
            };

            var mock = new Mock<ICurrencyLayerService>();
            mock.Setup(x => x.GetCurrency()).Returns(fakeGetCurrency);

            var currencyController = new CurrencyController(mock.Object);
            Assert.Throws<ArgumentException>(() => currencyController.GetCurrency(from, to));
        }

        [Fact]
        public void Get_currencies_currencylayer_success()
        {
            var fakeCurrencies = new Dictionary<string, string>
            {
                {"AED", "United Arab Emirates Dirham"},
                {"BRL", "Brazilian Real"},
                {"USD", "United States Dollar"}
            };

            var fakeGetCurrencies = GetCurrenciesFake();

            var mock = new Mock<ICurrencyLayerService>();
            mock.Setup(x => x.GetCurrencies()).Returns(fakeGetCurrencies);

            var currencyController = new CurrencyController(mock.Object);
            var currencies = currencyController.GetCurrencies();

            Assert.Equal(fakeCurrencies, currencies);
        }
    }
}