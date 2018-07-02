using System.Collections.Generic;

namespace CurrencyLayer.API.Models
{
    public class ListCurrencies
    {
        public bool Success { get; set; }
        public Dictionary<string, string> Currencies { get; set; }
    }
}