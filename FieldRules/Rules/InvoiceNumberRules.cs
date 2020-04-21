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
                "Factura de venta",
                "Pedido",
                "Remision",
                "Número"
            };

            //Only process value when contains one of the FieldName.
            if (!fieldName.Contains(value))
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
                " "
            };

            string result = string.Empty;

            rulesList.ForEach(x =>
                result = value.Replace(x, string.Empty).Trim()
            );

            return result;
        }
    }
}
