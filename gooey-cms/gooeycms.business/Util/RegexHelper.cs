using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Gooeycms.Business.Util
{
    public static class RegexHelper
    {

        public static Regex PhoneNumberPattern = new Regex(
                                    @"(?:\+?1)?                     #optional us country code
                                    (?:                             #start of areacode grouping
                                      [-\(\.]?                      #look for the starting areacode tag
                                         (?<areacode>[\d]{3})       #look for the areacode itself
                                      [=\)\.]?                      #look for a closing areacode tag
                                    )?                              #end of areacode grouping
                                    [\s-\.]?                        #look for a dash or dot between area code
                                    (?<prefix>                      #start of prefix grouping
                                       [\d]{3}                      #look for exactly a three-digit area code
                                    )                               #end of prefix grouping
                                    [\s-\.]?                        #look for a dash or dot after area code
                                    (?<line>                        #start of line grouping
                                       [\d]{4}                      #the line number itself
                                    )                               #end of line grouping", RegexOptions.Compiled | RegexOptions.IgnorePatternWhitespace);

        public class PhoneNumber 
        {
            public String AreaCode { get; set; }
            public String Prefix { get; set; }
            public String LineNumber { get; set; }
        }

        public static PhoneNumber ParsePhoneNumber(String text)
        {
            PhoneNumber number = new PhoneNumber();

            Match match = PhoneNumberPattern.Match(text);
            if (match.Success)
            {
                number.AreaCode = match.Groups["areacode"].Value;
                number.Prefix = match.Groups["prefix"].Value;
                number.LineNumber = match.Groups["line"].Value;
            }

            return number;
        }

        public static String ReplacePhoneNumbers(String content, String replacement)
        {
            return PhoneNumberPattern.Replace(content, replacement);
        }
    }
}
