using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Services
{
    public class PurchaseData
    {
        public bool EnabledPurchase { get; set; }
        public double Sell { get; set; }
        public double Buy { get; set; }
        public string AmountCurrencyPurchased { get; set; }
        public string ExchangeRate { get; set; }
    }
}
