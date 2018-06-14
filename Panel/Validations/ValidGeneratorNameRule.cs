using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Panel.Validations
{
    public class ValidGeneratorNameRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string SpecialCharactersPattern = @"[^A-Za-z0-9\-_]+";
            if (Regex.Match(value.ToString(), SpecialCharactersPattern).Success)
                return new ValidationResult(false, "No special characters allowed");
            return ValidationResult.ValidResult;
        }
    }   
}
