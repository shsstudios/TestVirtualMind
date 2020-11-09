using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Test.Validation
{
    public class IsoCurrencyValidation : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return ValidationResult.Success;

            return (value.ToString().ToUpper()) switch
            {
                "USD" => ValidationResult.Success,
                "BRL" => ValidationResult.Success,
                _ => new ValidationResult("Exchange currency entered is invalid"),
            };
        }
    }
}
