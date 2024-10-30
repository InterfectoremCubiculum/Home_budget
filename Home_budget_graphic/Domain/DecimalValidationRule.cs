using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Home_budget_graphic.Domain
{
    public class DecimalValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return new ValidationResult(false, "Value cannot be empty.");

            string input = value.ToString().Replace(".", cultureInfo.NumberFormat.CurrencyDecimalSeparator);

            if (decimal.TryParse(input, NumberStyles.Number, cultureInfo, out decimal result))
            {
                return (result == Math.Round(result, 2)) ? ValidationResult.ValidResult : new ValidationResult(false, "Value can have a maximum of 2 decimal places.");
            }
            else
                return new ValidationResult(false, "Invalid number format.");
        }
    }
}
