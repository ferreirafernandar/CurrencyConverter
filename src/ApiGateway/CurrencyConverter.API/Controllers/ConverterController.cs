using CurrencyConverter.API.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace CurrencyConverter.API.Controllers
{
    [Route("api/[controller]")]
    [EnableCors("AllowMyOrigin")]
    [ApiController]
    public class ConverterController : ControllerBase
    {
        private readonly ICurrencyConverterService _currencyConverterService;

        public ConverterController(ICurrencyConverterService currencyConverterService) => _currencyConverterService = currencyConverterService;

        [HttpGet]
        [Route("currencies")]
        public Dictionary<string, string> GetCurrencies()
        {
            return _currencyConverterService.GetCurrencies();
        }

        [HttpGet]
        [Route("currencyconversion")]
        public decimal CurrencyConversion(string currencyFrom, string currencyTo, decimal amount)
        {
            var errorMsg = new StringBuilder();

            if (amount < 0)
                errorMsg.AppendLine($"The {nameof(amount)} can not be negative.");

            if (string.IsNullOrWhiteSpace(currencyFrom))
                errorMsg.AppendLine($"{nameof(currencyFrom)} can't null or empty value.");

            if (string.IsNullOrWhiteSpace(currencyTo))
                errorMsg.AppendLine($"{nameof(currencyTo)} can't null or empty value.");

            if (errorMsg.Length > 0)
                throw new ArgumentException(errorMsg.ToString());

            var currency = _currencyConverterService.GetCurrency(currencyFrom, currencyTo);
            return currency * amount;
        }
    }
}
