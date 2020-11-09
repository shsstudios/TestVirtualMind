using System.Collections.Generic;
using System.Threading.Tasks;
using Test.Entities;

namespace Test.Services
{
    public interface ICurrency
    {
        public string CurrencyIso { get; }
        Task<RateData> GetRate();
        Task<PurchaseData> GetPurchaseData(List<Purchase> purchases, double amountToExchange);
    }
}
