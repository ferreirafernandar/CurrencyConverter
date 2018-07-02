using CurrencyConverter.API.Utils;
using Shared;
using System.Collections.Generic;

namespace CurrencyConverter.API.Services
{
    public class CurrencyConverterService : ICurrencyConverterService
    {
        private readonly IHttpClient _httpClient;

        public CurrencyConverterService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        private T GetBaseHttpTask<T>(string endpoint, params string[] parameters)
        {
            return _httpClient.GetBaseHttpTask<T>(Singleton.AppSettings.BaseUrl, endpoint, parameters);
        }

        public Dictionary<string, string> GetCurrencies()
        {
            return GetBaseHttpTask<Dictionary<string, string>>("currency/currencies");
        }

        public decimal GetCurrency(string currencyFrom, string currencyTo)
        {
            return GetBaseHttpTask<decimal>("currency", $"currencyFrom={currencyFrom}", $"currencyTo={currencyTo}");
        }
    }
}