namespace CurrencyLayer.API.Models
{
    public class AppSettings
    {
        public string BaseUrl { get; set; }
        public string Token { get; set; }
        public int CacheSeconds { get; set; }
    }
}