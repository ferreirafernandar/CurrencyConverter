using CurrencyLayer.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyLayerService _layerService;

        public CurrencyController(ICurrencyLayerService layerService) => _layerService = layerService;

        [HttpGet]
        [Route("currencies")]
        public Dictionary<string, string> GetCurrencies()
        {
            return _layerService.GetCurrencies().Currencies;
        }

        [HttpGet]
        public decimal GetCurrency(string currencyFrom, string currencyTo)
        {
            var errorMsg = new StringBuilder();

            if (string.IsNullOrWhiteSpace(currencyFrom))
                errorMsg.AppendLine($"{nameof(currencyFrom)} can't null or empty value.");

            if (string.IsNullOrWhiteSpace(currencyFrom))
                errorMsg.AppendLine($"{nameof(currencyTo)} can't null or empty value.");

            if (errorMsg.Length > 0)
                throw new ArgumentException(errorMsg.ToString());

            var currencies = _layerService.GetCurrency();

            decimal usdCurrencyFrom;
            decimal usdCurrencyTo;

            if (currencyFrom == currencies.Source)
                usdCurrencyFrom = 1;

            else if (!currencies.Quotes.TryGetValue(currencies.Source + currencyFrom, out usdCurrencyFrom))
                errorMsg.AppendLine($"{nameof(currencyFrom)} not valid.");

            if (currencyTo == currencies.Source)
                usdCurrencyTo = 1;
            else if (!currencies.Quotes.TryGetValue(currencies.Source + currencyTo, out usdCurrencyTo))
                errorMsg.AppendLine($"{nameof(currencyTo)} not valid.");

            if (errorMsg.Length > 0)
                throw new ArgumentException(errorMsg.ToString());

            return usdCurrencyTo / usdCurrencyFrom;
        }
    }
}
