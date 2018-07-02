using System.Threading.Tasks;
using CurrencyLayer.API.Models;

namespace CurrencyLayer.API.Services
{
    public interface ICurrencyLayerService
    {
        ListCurrencies GetCurrencies();
        Currency GetCurrency();
    }
}