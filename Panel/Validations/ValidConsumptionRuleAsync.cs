using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public class ValidConsumptionRuleAsync
    {
        public static Task<List<string>> ValidateConsumptionRuleAsync(double Consumption)
        {
            var error = Task.Run<List<string>>(() =>
            {
                string NoLetter = @"[A-Za-z]+";
                string OnlyDoublesPattern = @"\d+(?:\.\d+)?";
                string SpecialCharactersPattern = @"[^A-Za-z0-9\-_\.\s+]+";

                if ((Regex.Match(Consumption.ToString(), NoLetter).Success))
                    return new List<string>() { $"Your input must be a number" };
                else if (!(Regex.Match(Consumption.ToString(), OnlyDoublesPattern).Success))
                    return new List<string>() { $"Your input must be a number" };
                else if (Regex.Match(Consumption.ToString(), SpecialCharactersPattern).Success)
                    return new List<string>() { $"No special characters allowed" };

                return null;
            });
            return error;
        }
    }
}
