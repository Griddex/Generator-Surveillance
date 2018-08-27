using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public class ValidateRunningHoursRule
    {
        public static Task<List<string>> ValidateRunningHoursRuleAsync(double RunningHours)
        {
            var error = Task.Factory.StartNew<List<string>>(() =>
            {
                string OnlyDoublesPattern = @"\d+(?:\.\d+)?";
                string SpecialCharactersPattern = @"[^A-Za-z0-9\-_\.\s+]+";

                if (!(Regex.Match(RunningHours.ToString(), OnlyDoublesPattern).Success))
                    return new List<string>() { $"Your input must be a number" };
                else if (Regex.Match(RunningHours.ToString(), SpecialCharactersPattern).Success)
                    return new List<string>() { "No special characters allowed" };

                return null;
            });
            return error;
        }
    }
}
