using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public static class ValidGeneratorNameRuleAsync
    {
        public static Task<List<string>> ValidateGeneratorNameAsync(string Name)
        {
            var error = Task.Factory.StartNew<List<string>>(() =>
            {
                string SpecialCharactersPattern = @"[^A-Za-z0-9\-_\s+]+";
                if (Regex.Match(Name, SpecialCharactersPattern).Success)
                    return new List<string>(){"No special characters allowed"};
                return null;
            });
            return error;
        }
    }
}
