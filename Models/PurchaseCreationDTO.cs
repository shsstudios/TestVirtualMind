using System.ComponentModel.DataAnnotations;
using Test.Validation;

namespace Test.Models
{
    public class PurchaseCreationDTO
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        [IsoCurrencyValidation()]
        public string IsoCurrency { get; set; }
    }
}
