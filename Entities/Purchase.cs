using System;
using System.ComponentModel.DataAnnotations;

namespace Test.Entities
{
    public class Purchase
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string IsoCurrency { get; set; }
        public DateTime Date { get; set; }
        public double SellRate { get; set; }
        public double BuyRate { get; set; }
        public string ExchangeRate { get; set; }
        public string AmountCurrencyPurchased { get; set; }
    }
}
