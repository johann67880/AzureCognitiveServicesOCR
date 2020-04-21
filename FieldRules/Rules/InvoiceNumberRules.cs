using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FieldRules
{
    public static class InvoiceNumberRules
    {
        public static bool Validate(string value)
        {
            return new List<Func<bool>>
            {
                //You can add as many rules as you want
                () => IsInvoiceNumber(value)
            }.Any(number => number());
        }

        public static bool IsInvoiceNumber(string value)
        {
            List<string> fieldName = new List<string>()
            {
                "Nro",
                "Número",
                "N°"
            };

            //Only process value when contains one of the FieldName.
            var contains = fieldName.Any(x => value.Contains(x, StringComparison.OrdinalIgnoreCase));

            if (!contains)
                return false;

            string result = ExtractInvoiceNumber(value);

            List<string> validFormats = new List<string>()
            {
                @"(.|\s)*\S(.|\s)*"
            };

            //returns true when at least one pattern is valid.
            return validFormats.Any(x => Regex.IsMatch(result, x));
        }

        public static string ExtractInvoiceNumber(string value)
        {
            //When is NIT, then remove all the follow words or symbols.
            List<string> rulesList = new List<string>()
            {
                "N°",
                "NO",
                ".",
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
