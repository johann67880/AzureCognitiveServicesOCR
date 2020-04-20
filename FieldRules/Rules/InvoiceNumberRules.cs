using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FieldRules
{
    public static class InvoiceNumberRules
    {
        public static bool Validate(string value)
        {
            return new List<Func<bool>>
            {
                //TODO: Define here all the rules needed to validate if value is invoice number
            }.Any(number => number());
        }
    }
}
