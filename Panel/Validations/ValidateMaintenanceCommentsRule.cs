using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public class ValidateMaintenanceCommentsRule
    {
        public static Task<List<string>> ValidateMaintenanceCommentsAsync(string Comments)
        {
            var error = Task.Factory.StartNew<List<string>>(() =>
            {
                string SpecialCharactersPattern = @"[^A-Za-z0-9\-_\s+]+";
                if (Regex.Match(Comments, SpecialCharactersPattern).Success)
                    return new List<string>() { "No special characters allowed" };
                return null;
            });
            return error;
        }
    }
}
