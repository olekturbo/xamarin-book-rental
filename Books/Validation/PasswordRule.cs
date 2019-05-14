using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Books.Validation
{
    public class PasswordRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            var hasMinimum6Chars = new Regex(@".{6,}");

            return hasMinimum6Chars.IsMatch(str);
        }
    }
}
