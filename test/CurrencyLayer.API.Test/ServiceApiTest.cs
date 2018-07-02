using System;
using System.Data;
using CurrencyLayer.API.Models;
using CurrencyLayer.API.Services;
using CurrencyLayer.API.Utils;
using Moq;
using Shared;
using Xunit;

namespace CurrencyLayer.API.Test
{
    public class ServiceApiTest
    {

        public ServiceApiTest()
        {
            Singleton.AppSettings.Token = "";
            Singleton.AppSettings.BaseUrl = "";
        }


        [Fact]
        public void Get_currencies_currencylayer_success()
        {

            var mock = new Mock<IHttpClient>();

            mock.Setup(x => x.GetBaseHttpTask<ListCurrencies>(Singleton.AppSettings.BaseUrl, "list", $"access_key={Singleton.AppSettings.Token}"))
                .Returns(new ListCurrencies { Success = true });

            var service = new CurrencyLayerService(mock.Object);
            service.ClearCache();

            service.GetCurrencies();
            mock.Verify(x => x.GetBaseHttpTask<ListCurrencies>(Singleton.AppSettings.BaseUrl, "list", $"access_key={Singleton.AppSettings.Token}"), Times.Once);
            service.GetCurrencies();
            mock.Verify(x => x.GetBaseHttpTask<ListCurrencies>(Singleton.AppSettings.BaseUrl, "list", $"access_key={Singleton.AppSettings.Token}"), Times.Once);
        }

        [Fact]
        public void Get_currencies_currencylayer_ThrowsDataException()
        {
            var mock = new Mock<IHttpClient>();

            mock.Setup(x => x.GetBaseHttpTask<ListCurrencies>(Singleton.AppSettings.BaseUrl, "list", $"access_key={Singleton.AppSettings.Token}"))
                .Returns(new ListCurrencies { Success = false });

            var service = new CurrencyLayerService(mock.Object);
            service.ClearCache();

            Assert.Throws<DataException>(() => service.GetCurrencies());
        }

        [Theory]
        [InlineData(60)]
        [InlineData(0)]
        public void Get_currency_currencylayer_success(int cacheSeconds)
        {

            Singleton.AppSettings.CacheSeconds = cacheSeconds;

            var mock = new Mock<IHttpClient>();
            mock.Setup(x => x.GetBaseHttpTask<Currency>(Singleton.AppSettings.BaseUrl, "live", $"access_key={Singleton.AppSettings.Token}"))
                .Returns(new Currency { Success = true });
            var service = new CurrencyLayerService(mock.Object);
            service.ClearCache();

            var r1 = service.GetCurrency();
            service.GetCurrency();
            if (r1.Date >= DateTime.Now.AddSeconds(-cacheSeconds))
            {
                mock.Verify(x => x.GetBaseHttpTask<Currency>(Singleton.AppSettings.BaseUrl, "live", $"access_key={Singleton.AppSettings.Token}"), Times.Once);
            }
            else
            {
                mock.Verify(x => x.GetBaseHttpTask<Currency>(Singleton.AppSettings.BaseUrl, "live", $"access_key={Singleton.AppSettings.Token}"), Times.Exactly(2));
            }
        }

        [Fact]
        public void Get_currency_currencylayer_ThrowsDataException()
        {
            Singleton.AppSettings.CacheSeconds = 0;

            var mock = new Mock<IHttpClient>();

            mock.Setup(x => x.GetBaseHttpTask<Currency>(Singleton.AppSettings.BaseUrl, "live", $"access_key={Singleton.AppSettings.Token}"))
                .Returns(new Currency { Success = false });

            var service = new CurrencyLayerService(mock.Object);
            service.ClearCache();

            Assert.Throws<DataException>(() => service.GetCurrency());
        }

        [Fact]
        public void Get_currency_currencylayer_ThrowsDataException_But_InCache()
        {
            Singleton.AppSettings.CacheSeconds = 0;

            var mock = new Mock<IHttpClient>();

            mock.Setup(x => x.GetBaseHttpTask<Currency>(Singleton.AppSettings.BaseUrl, "live", $"access_key={Singleton.AppSettings.Token}"))
                .Returns(new Currency { Success = true });

            var service = new CurrencyLayerService(mock.Object);
            service.ClearCache();

            var r1 = service.GetCurrency();

            mock.Setup(x => x.GetBaseHttpTask<Currency>(Singleton.AppSettings.BaseUrl, "live", $"access_key={Singleton.AppSettings.Token}"))
                .Returns(new Currency { Success = false });

            var r2 = service.GetCurrency();

            Assert.Equal(r1, r2);

        }
    }
}