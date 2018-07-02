using System.Collections.Generic;
using System.Threading.Tasks;

namespace CurrencyConverter.API.Services
{
    public interface ICurrencyConverterService
    {
        Dictionary<string, string> GetCurrencies();
        decimal GetCurrency(string currencyFrom, string currencyTo);
    }
}