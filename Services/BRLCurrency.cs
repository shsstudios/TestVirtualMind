using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Test.Entities;

namespace Test.Services
{
    public class BRLCurrency : ICurrency
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BRLCurrency(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        private readonly string brlUri = "https://www.bancoprovincia.com.ar/Principal/Dolar";

        public string CurrencyIso => "BRL";

        public async Task<RateData> GetRate()
        {
            var request = new HttpRequestMessage(HttpMethod.Get, brlUri);

            var client = _httpClientFactory.CreateClient();

            var response = await client.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            JArray contentArray = JArray.Parse(content);

            double purchase = double.Parse(contentArray[0].ToString(), CultureInfo.InvariantCulture) / 4;
            double sell = double.Parse(contentArray[1].ToString(), CultureInfo.InvariantCulture) / 4;
            string date = contentArray[2].ToString();

            return new RateData
            {
                Purchase = purchase,
                Sell = sell,
                UpdateDate = date
            };
        }

        public async Task<PurchaseData> GetPurchaseData(List<Purchase> purchases, double amountToExchange)
        {
            int limitCurrencyPurchase = 300;

            var monthCurrencyPurchased = purchases.Sum(x => x.Amount / x.SellRate);

            var purchaseData = new PurchaseData
            {
                EnabledPurchase = false,
                AmountCurrencyPurchased = null,
                Sell = 0,
                Buy = 0,
                ExchangeRate = null
            };

            if (monthCurrencyPurchased >= limitCurrencyPurchase)
                return purchaseData;

            var rate = await GetRate();

            var currencyPurchase = amountToExchange / rate.Sell;

            if (currencyPurchase + monthCurrencyPurchased >= limitCurrencyPurchase)
                return purchaseData;

            string amountCurrencyPurchased = currencyPurchase.ToString(CultureInfo.InvariantCulture) + " " + "reals";
            purchaseData.EnabledPurchase = true;
            purchaseData.AmountCurrencyPurchased = amountCurrencyPurchased;
            purchaseData.Sell = rate.Sell;
            purchaseData.Buy = rate.Purchase;
            purchaseData.ExchangeRate = JsonSerializer.Serialize(rate);

            return purchaseData;
        }
    }
}
