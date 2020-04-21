using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FieldRules
{
    public static class IVARules
    {
        public static bool Validate(string value)
        {
            return new List<Func<bool>>
            {
                //You can add as many rules as you want
                () => IsIVA(value)
            }.Any(iva => iva());
        }

        public static bool IsIVA(string value)
        {
            List<string> fieldName = new List<string>()
            {
                "IVA",
                "I.V.A."
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

            string result = ExtractIVA(value);

            //returns true when at least one pattern is valid.
            return validFormats.Any(x => Regex.IsMatch(result, x));
        }

        public static string ExtractIVA(string value)
        {
            //When is NIT, then remove all the follow words or symbols.
            List<string> rulesList = new List<string>()
            {
                "IVA",
                "I.V.A.",
                "+",
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
