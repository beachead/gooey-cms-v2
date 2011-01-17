using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gooeycms.Business.Twilio
{
    public class AvailablePhoneNumber
    {
        private static Regex PhonePattern = new Regex(@"(\+?1?)[ \.-]?\(?(\d{3})\)?[ \.-]?(\d{3})[\.-]?(\d{4})", RegexOptions.Compiled);

        public String FriendlyName { get; set; }

        public String PhoneNumber { get; set; }

        public String WebDisplay { get; set; }

        public String AreaCode { get; set; }
        public String Exchange { get; set; }
        public String LineNumber { get; set; }

        public static AvailablePhoneNumber Parse(String phone)
        {
            Match match = PhonePattern.Match(phone);
            if (!match.Success)
                throw new FormatException("The phone number is not in a supported format and could not be parsed. Expected Format: (510) 564-7903 or +15105647903");

            String plusone = match.Groups[1].Value;
            String areacode = match.Groups[2].Value;
            String exchange = match.Groups[3].Value;
            String last = match.Groups[4].Value;

            if (String.IsNullOrWhiteSpace(plusone))
                plusone = "+1";

            String phonenumber = plusone + areacode + exchange + last;
            String friendly = String.Format("({0}) {1}-{2}", areacode, exchange, last);
            String web = String.Format("{0}-{1}-{2}", areacode, exchange, last);

            AvailablePhoneNumber result = new AvailablePhoneNumber();
            result.PhoneNumber = phonenumber;
            result.FriendlyName = friendly;
            result.WebDisplay = web;

            result.AreaCode = areacode;
            result.Exchange = exchange;
            result.LineNumber = last;

            return result;
        }
    }
}
