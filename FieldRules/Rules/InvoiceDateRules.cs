using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace FieldRules
{
    public static class InvoiceDateRules
    {
        public static bool Validate(string value)
        {
            return new List<Func<bool>>
            {
                //You can add as many rules as you want
                () => IsDate(value)
            }.Any(date => date());
        }

        public static bool IsDate(string value)
        {
            List<string> fieldName = new List<string>()
            {
                "Fecha",
                "Expedicion"
            };

            //Only process value when contains one of the FieldName.
            var contains = fieldName.Any(x => value.Contains(x, StringComparison.OrdinalIgnoreCase));

            if (!contains)
                return false;

            //Regular Expressions to determine if is a valid NIT
            List<string> validFormats = new List<string>()
            {
                //expression to validate dates in dd/mm/yyyy, dd-mm-yyyy or dd.mm.yyyy
                @"^(?:(?:31(\/|-|\.)(?:0?[13578]|1[02]))\1|(?:(?:29|30)(\/|-|\.)(?:0?[13-9]|1[0-2])\2))(?:(?:1[6-9]|[2-9]\d)?\d{2})$|^(?:29(\/|-|\.)0?2\3(?:(?:(?:1[6-9]|[2-9]\d)?(?:0[48]|[2468][048]|[13579][26])|(?:(?:16|[2468][048]|[3579][26])00))))$|^(?:0?[1-9]|1\d|2[0-8])(\/|-|\.)(?:(?:0?[1-9])|(?:1[0-2]))\4(?:(?:1[6-9]|[2-9]\d)?\d{2})$",
                //expression to validate dates in yyyy-mm-dd
                @"([12]\d{3}-(0[1-9]|1[0-2])-(0[1-9]|[12]\d|3[01]))",
                //expression to validate dates in yyyy/mm/dd
                @"([12]\d{3}/(0[1-9]|1[0-2])/(0[1-9]|[12]\d|3[01]))",
                //expression to validate dates in yyyy.mm.dd
                @"([12]\d{3}.(0[1-9]|1[0-2]).(0[1-9]|[12]\d|3[01]))"
            };

            string result = ExtractDate(value);

            //returns true when at least one pattern is valid.
            return validFormats.Any(x => Regex.IsMatch(result, x));
        }

        public static string ExtractDate(string value)
        {
            //When is NIT, then remove all the follow words or symbols.
            List<string> rulesList = new List<string>()
            {
                "Fecha",
                "Expedicion",
                "Ciudad",
                "factura",
                "exped",
                "comprobante",
                "de",
                ":",
                " "
            };

            string result = value;

            rulesList.ForEach(x =>
                result = Regex.Replace(result, x, string.Empty, RegexOptions.IgnoreCase).Trim()
            );

            return result;
        }

        public static bool IsCurrentYear(string stringDate)
        {
            var date = Convert.ToDateTime(stringDate);
            return DateTime.Now.Year == date.Year;
        }
    }
}
