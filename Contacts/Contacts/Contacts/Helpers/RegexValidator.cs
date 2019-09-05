using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Contacts.Helpers
{
    public class RegexValidator
    {
        public static bool ValidEmail(string email)
        {
            return !string.IsNullOrEmpty(email) &&
                Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
                RegexOptions.IgnoreCase);

        }
    }
}
