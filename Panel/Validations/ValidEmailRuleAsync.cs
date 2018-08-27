using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Panel.Validations
{
    public static class ValidEmailRuleAsync
    {
        public static Task<List<string>> ValidateEmailRuleAsync(string Email)
        {
            var error = Task.Run<List<string>>(() =>
            {
                string EmailPattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
                if (Regex.Match(Email, EmailPattern).Success)
                    return new List<string>(){"Invalid Email Format"};
                return null;
            });
            return error;
        }
    }
}
