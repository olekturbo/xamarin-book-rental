﻿using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace Books.Validation
{
    public class EmailRule<T> : IValidationRule<T>
    {
        public string ValidationMessage { get; set; }

        public bool Check(T value)
        {
            if (value == null)
            {
                return false;
            }

            var str = value as string;

            try
            {
                MailAddress email = new MailAddress(str);
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
