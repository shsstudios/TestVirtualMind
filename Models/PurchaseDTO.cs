using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Models
{
    public class PurchaseDTO
    {
        public int Id { get; set; }
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public string IsoCurrency { get; set; }
        public DateTime Date { get; set; }
        public string AmountCurrencyPurchased { get; set; }
    }
}
