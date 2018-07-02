using CurrencyConverter.API.Controllers;
using CurrencyConverter.API.Services;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace UnitTest.CurrencyConverter.Application
{
    public class ControllerApiTest
    {
        public ControllerApiTest()
        {

        }

        private static Dictionary<string, string> GetCurrenciesFake()
        {
            return new Dictionary<string, string>
            {
                {"AED", "United Arab Emirates Dirham"},
                {"BRL", "Brazilian Real"},
                {"USD", "United States Dollar"}
            };
        }

        [Fact]
        public void Get_currencies_converterlayer_success()
        {
            var fakeGetCurrencies = GetCurrenciesFake();

            var mock = new Mock<ICurrencyConverterService>();
            mock.Setup(x => x.GetCurrencies())
                .Returns(fakeGetCurrencies);

            var currencyController = new ConverterController(mock.Object);
            var currencies = currencyController.GetCurrencies();

            Assert.Equal(fakeGetCurrencies, currencies);
        }

        [Theory]
        [InlineData("USD", "BRL", 10, 2, 20)]
        [InlineData("USD", "EUR", 10, 0.8, 8)]
        [InlineData("USD", "BRL", 25, 2, 50)]
        [InlineData("USD", "EUR", 25, 0.8, 20)]

        [InlineData("EUR", "USD", 10, 1.25, 12.5)]
        [InlineData("EUR", "BRL", 10, 2.5, 25)]
        [InlineData("EUR", "USD", 25, 1.25, 31.25)]
        [InlineData("EUR", "BRL", 25, 2.5, 62.5)]

        [InlineData("BRL", "USD", 10, 0.5, 5)]
        [InlineData("BRL", "EUR", 10, 0.4, 4)]
        [InlineData("BRL", "USD", 25, 0.5, 12.5)]
        [InlineData("BRL", "EUR", 25, 0.4, 10)]
        public void Get_currencyconversion_converterlayer_success(string from, string to, decimal amount, decimal currency, decimal resultExpected)
        {
            var mock = new Mock<ICurrencyConverterService>();
            mock.Setup(x => x.GetCurrency(from, to))
                .Returns(currency);

            var currencyController = new ConverterController(mock.Object);
            var result = currencyController.CurrencyConversion(from, to, amount);

            Assert.Equal(resultExpected, result);
        }

        [Theory]
        [InlineData("", "", -1)]
        [InlineData("", "", 1)]
        [InlineData("", "BRL", 1)]
        [InlineData("USD", "", -1)]
        [InlineData("USD", "", 1)]
        [InlineData("USD", "BRL", -1)]
        public void Get_currencyconversion_converterlayer_ThrowArgumentException(string from, string to, decimal amount)
        {
            var mock = new Mock<ICurrencyConverterService>();
            var controller = new ConverterController(mock.Object);

            Assert.Throws<ArgumentException>(() => controller.CurrencyConversion(from, to, amount));
        }
    }
}