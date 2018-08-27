using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public static class ValidGeneratorNameRuleAsync
    {
        public static Task<List<string>> ValidateGeneratorNameAsync(string Name)
        {
            var error = Task.Run<List<string>>(() =>
            {
                string SpecialCharactersPattern = @"[^A-Za-z0-9\-_\s+]+";
                string Numbers = @"^\d+";
                string Spaces = @"^\s+";

                if (Regex.Match(Name, SpecialCharactersPattern).Success)
                    return new List<string>(){"No special characters allowed"};
                else if (Regex.Match(Name, Numbers).Success)
                    return new List<string>() { "Name cannot start with a number" };
                else if (Regex.Match(Name, Spaces).Success)
                    return new List<string>() { "Name cannot start with space character" };

                return null;
            });
            return error;
        }
    }
}
