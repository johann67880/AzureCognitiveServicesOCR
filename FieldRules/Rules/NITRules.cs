using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FieldRules
{
    public static class NITRules
    {
        public static bool Validate(string value)
        {
            return new List<Func<bool>>
            {
                //You can add as many rules as you want
                () => IsNIT(value)
            }.Any(nit => nit());
        }

        public static bool IsNIT(string value)
        {
            List<string> fieldName = new List<string>()
            {
                "NIT",
                "Nombre cliente"
            };

            //Only process value when contains one of the FieldName.
            var contains = fieldName.Any(x => value.Contains(x, StringComparison.OrdinalIgnoreCase));

            if (!contains)
                return false;

            //Regular Expressions to determine if is a valid NIT
            List<string> validFormats = new List<string>()
            {
                "(^[0-9.]+-{1}[0-9]{1})",
                "^[0-9.]*$"
            };

            string result = ExtractNit(value);

            //returns true when at least one pattern is valid.
            return validFormats.Any(x => Regex.IsMatch(result, x));
        }

        public static string ExtractNit(string value)
        {
            //When is NIT, then remove all the follow words or symbols.
            List<string> rulesList = new List<string>()
            {
                "NIT",
                "Nombre cliente",
                ".",
                "=",
                ":",
                " "
            };

            string result = value;

            rulesList.ForEach(x =>
                result = Regex.Replace(result, x, string.Empty, RegexOptions.IgnoreCase).Trim()
            );

            return result;
        }
    }
}
