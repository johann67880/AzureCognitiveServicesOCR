using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FieldRules
{
    public static class SubtotalRules
    {
        public static bool Validate(string value)
        {
            return new List<Func<bool>>
            {
                //You can add as many rules as you want
                () => IsSubtotal(value)
            }.Any(subtotal => subtotal());
        }

        public static bool IsSubtotal(string value)
        {
            List<string> fieldName = new List<string>()
            {
                "SUBTOTAL"
            };

            //Only process value when contains one of the FieldName.
            var contains = fieldName.Any(x => value.Contains(x, StringComparison.OrdinalIgnoreCase));

            if (!contains)
                return false;

            //Regular Expressions to determine if is a valid NIT
            List<string> validFormats = new List<string>()
            {
                "^[0-9.]*$"
            };

            string result = ExtractSubtotal(value);

            //returns true when at least one pattern is valid.
            return validFormats.Any(x => Regex.IsMatch(result, x));
        }

        public static string ExtractSubtotal(string value)
        {
            //When is NIT, then remove all the follow words or symbols.
            List<string> rulesList = new List<string>()
            {
                "SUBTOTAL",
                "$",
                ".",
                ",",
                "'",
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
