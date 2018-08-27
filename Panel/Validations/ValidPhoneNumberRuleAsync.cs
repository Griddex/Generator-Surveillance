using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public class ValidPhoneNumberRuleAsync
    {
        public static Task<List<string>> ValidatePhoneNumberRuleAsync(double PhoneNumber)
        {
            var error = Task.Factory.StartNew<List<string>>(() =>
            {
                string OnlyDoublesPattern = @"\d+(?:\.\d+)?";
                string SpecialCharactersPattern = @"[^A-Za-z0-9\-_\.\s+]+";

                if (!(Regex.Match(PhoneNumber.ToString(), OnlyDoublesPattern).Success))
                    return new List<string>() { $"Your input must be a number" };
                else if (Regex.Match(PhoneNumber.ToString(), SpecialCharactersPattern).Success)
                    return new List<string>() { $"No special characters allowed" };

                return null;
            });
            return error;
        }
    }
}
