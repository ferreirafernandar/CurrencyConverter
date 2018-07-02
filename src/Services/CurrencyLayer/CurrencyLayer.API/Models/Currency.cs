using System;
using System.Collections.Generic;

namespace CurrencyLayer.API.Models
{
    public class Currency
    {
        public Currency()
        {
            Date = DateTime.Now;
        }

        public DateTime Date { get; }
        public bool Success { get; set; }
        public string Source { get; set; }
        public Dictionary<string, decimal> Quotes { get; set; }
    }
}