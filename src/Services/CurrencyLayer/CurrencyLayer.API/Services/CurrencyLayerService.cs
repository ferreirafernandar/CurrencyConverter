using CurrencyLayer.API.Models;
using CurrencyLayer.API.Utils;
using Shared;
using System;
using System.Data;

namespace CurrencyLayer.API.Services
{
    public class CurrencyLayerService : ICurrencyLayerService
    {
        private readonly IHttpClient _httpClient;
        private static ListCurrencies _listCurrencies;
        private static Currency _currency;

        public CurrencyLayerService(IHttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public T GetBaseHttpTask<T>(string endpoint)
        {
            return _httpClient.GetBaseHttpTask<T>(Singleton.AppSettings.BaseUrl, endpoint, $"access_key={Singleton.AppSettings.Token}");
        }

        public ListCurrencies GetCurrencies()
        {
            if (_listCurrencies != null)
                return _listCurrencies;

            _listCurrencies = GetBaseHttpTask<ListCurrencies>("list");

            if (_listCurrencies.Success)
                return _listCurrencies;

            _listCurrencies = null;

            throw new DataException("Error in GetCurrencies");

        }

        public Currency GetCurrency()
        {
            if (_currency != null && _currency.Date >= DateTime.Now.AddSeconds(-Singleton.AppSettings.CacheSeconds))
                return _currency;

            try
            {
                var tmp = GetBaseHttpTask<Currency>("live");
                if (!tmp.Success)
                    throw new DataException("Error in GetCurrency");

                _currency = tmp;
            }
            catch
            {
                if (_currency == null)
                    throw;
            }

            return _currency;
        }

        public void ClearCache()
        {
            _currency = null;
            _listCurrencies = null;
        }
    }
}