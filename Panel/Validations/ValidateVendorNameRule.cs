using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public class ValidateVendorNameRule
    {
        public static Task<List<string>> ValidateVendorNameAsync(string Name)
        {
            var error = Task.Factory.StartNew<List<string>>(() =>
            {
                string SpaceOrNothingPattern = @"^\s+";
                if (Regex.Match(Name, SpaceOrNothingPattern).Success)
                    return new List<string>() { "Vendor name cannot be empty" };
                else
                    return null;
            });
            return error;
        }
    }
}
