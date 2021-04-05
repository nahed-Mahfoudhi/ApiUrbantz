using ApiUrbantz.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ApiUrbantz.UTILS
{
    public static class Utils
    {

        const string LX = "LX";
        const string LC = "LC";
        const string PDC = "PDC";
        const string LS = "LS";
        const string LIV = "LIV";
        const string LD = "LD";
        const string RS = "RS";
        const string LI = "LI";
        const string DMI = "DMI";
        const string LM = "LM";
        const string LE = "LE";
        const string CC = "CC";

        const string DELIVERY = "delivery";
        const string PICKUP = "pickup";
        const string SAV = "";

        public enum TraficLibelle
        {
            LIV,
            REP,
            SAV
        }
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

        public static string GetPhoneNumberByCodePays(string CodePays , CommandeEntete commandeEntete )
        {
            var phone = "";
            var FormattedPhone = "";
            if (CodePays == "FR")
            {
                if ((commandeEntete.PortableDestinataire != null) && ((commandeEntete.PortableDestinataire.Substring(0, 2) == "06")
                   || (commandeEntete.PortableDestinataire.Substring(0, 2) == "07"))
                   )

                {
                    phone =  commandeEntete.PortableDestinataire;
                }
                else if (commandeEntete.TelephoneDestinataire != null)
                {
                    if ((commandeEntete.TelephoneDestinataire.Substring(0, 2) == "06")
                    || (commandeEntete.TelephoneDestinataire.Substring(0, 2) == "07"))

                    {
                        phone = commandeEntete.TelephoneDestinataire;
                    }
                }

                if (!string.IsNullOrEmpty(phone))
                {
                    if ((phone.Substring(0, 2) == "06") || (phone.Substring(0, 2) == "07"))
                    {
                        FormattedPhone = string.Concat("+33", phone.Substring(1, phone.Length - 1));
                    }
                    else if (phone.Substring(0, 3) == "+33")
                        FormattedPhone = phone;
                }

                return FormattedPhone;

            }
            else if (new List<string>() { "LU", "BE" }.Contains(CodePays))
            {
                return commandeEntete.PortableDestinataire ?? commandeEntete.TelephoneDestinataire;
            }
            return null; 
        }


        public static int GetPoidsByPrestation(double totalPoids, string code)
        {
            if (totalPoids < 30)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 3; }
                else if (new List<string>() { LS, LIV, LD, RS, LM }.Contains(code))
                { return 5; }
                else if (new List<string>() { LI, DMI, LE }.Contains(code))
                return 10;
            }else if (totalPoids < 50)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 5; }
                else if (new List<string>() { LS, LIV, LD, RS, LM }.Contains(code))
                { return 7; }
                else if (new List<string>() { LI, DMI, LE }.Contains(code))
                    return 10;
            }
            else if (totalPoids < 80)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 5; }
                else if (new List<string>() { LS, LIV }.Contains(code))
                { return 7; }
                else if (new List<string>() { LD,RS,LM,LE }.Contains(code))
                { return 10; }
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 15;
            }
            else if (totalPoids < 100)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 7; }
                else if (new List<string>() { LS, LIV , LD,RS,LM,LE }.Contains(code))
                { return 10; }
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 15;
            }
            else if (totalPoids < 120)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 7; }
                else if (new List<string>() { LS, LIV, LE }.Contains(code))
                { return 10; }
                else if (new List<string>() { LD,RS,LM }.Contains(code))
                    return 12;
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 19;
            }
            else if (totalPoids < 150)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 7; }
                else if (new List<string>() { LS, LIV, LE }.Contains(code))
                { return 10; }
                else if (new List<string>() { LD, RS, LM }.Contains(code))
                    return 12;
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 22;
            }
            else if (totalPoids < 200)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 10; }
                else if (new List<string>() { LS, LIV, LE }.Contains(code))
                { return 15; }
                else if (new List<string>() { LD, RS, LM }.Contains(code))
                    return 12;
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 22;
            }
            else if (totalPoids < 300)
            {
                if (new List<string>() { LX, LC, PDC }.Contains(code))
                { return 10; }
                else if (new List<string>() { LS, LIV, LE,LD,RS,LM }.Contains(code))
                { return 15; }
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 27;
            }
            else if (totalPoids < 400)
            {
                if (new List<string>() { LX, LC, PDC ,LE}.Contains(code))
                { return 15; }
                else if (new List<string>() { LS, LIV,  LD, RS, LM }.Contains(code))
                { return 20; }
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 32;
            }else
            {
                if (new List<string>() { LX, LC, PDC, LE }.Contains(code))
                { return 15; }
                else if (new List<string>() { LS, LIV, LD, RS, LM }.Contains(code))
                { return 30; }
                else if (new List<string>() { LI, DMI }.Contains(code))
                    return 45;
                else if (code == CC)
                    return 3;
            }

            return 0; 
        }

        public static string GetDeliveryTypeByTraficCode(string code)
        {
            if (code.Equals(TraficLibelle.LIV.ToString()) || code.Equals(TraficLibelle.SAV.ToString()))
                return DELIVERY;
                return PICKUP;

        }
    }
}