using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.UTILS
{
    public static class Utils
    {
        static bool IsValidPhoneNumber(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber) && ((phoneNumber.Substring(0, 2) == "06") || (phoneNumber.Substring(0, 2) == "07")
                || (phoneNumber.Substring(0, 4) == "+337") || (phoneNumber.Substring(0, 4) == "+336")
                || (phoneNumber.Substring(0, 4) == "0044")))
                return true;
                return false;
        }

       public  static string GetFormattedPhoneNumber(string phoneNumber)
        {
            string formattedPhoneNumber = phoneNumber; 
            if (!IsValidPhoneNumber(phoneNumber))
                return null;

            if ((phoneNumber.Substring(0, 2) == "06") || (phoneNumber.Substring(0, 2) == "07"))
                 formattedPhoneNumber = string.Concat("+33", phoneNumber.Substring(1, phoneNumber.Length - 1));
            else if (phoneNumber.Substring(0, 4) == "0044")
                formattedPhoneNumber = string.Concat("+", phoneNumber.Substring(2, phoneNumber.Length - 2));

            return formattedPhoneNumber; 
        }
    }
}