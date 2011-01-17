using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Xml;
using System.IO;

namespace Gooeycms.Business.Twilio
{
    public class TwilioClient
    {
        private const String API_VERSION = "2010-04-01";
        private const String POST = "POST";
        private const String GET = "GET";

        private readonly String accountSid;
        private readonly String accountToken;
        private TwilioRest.Account client;

        public TwilioClient(String accountSid, String accountToken)
        {
            this.accountSid = accountSid;
            this.accountToken = accountToken;

            this.client = new TwilioRest.Account(this.accountSid, this.accountToken);
        }



        /// <summary>
        /// Searches for available local numbers <br />
        /// (optional: area code)
        /// </summary>
        /// <param name="areaCode">Specifies the area code to limit the search to</param>
        /// <returns></returns>
        public IList<AvailablePhoneNumber> SearchAvailableLocalNumbers(String areaCode = null)
        {
            String url = String.Format("/{0}/Accounts/{1}/AvailablePhoneNumbers/US/Local",API_VERSION,this.accountSid);

            Hashtable fields = new Hashtable();
            if (!String.IsNullOrEmpty(areaCode))
                fields.Add("AreaCode", areaCode);

            return ParseAvailablePhoneNumber(Execute(url, fields));
        }

        public IList<AvailablePhoneNumber> SearchAvailableTollFreeNumbers(String searchPattern = null)
        {
            String url = String.Format("/{0}/Accounts/{1}/AvailablePhoneNumbers/US/TollFree", API_VERSION, this.accountSid);

            Hashtable fields = new Hashtable();
            if (!String.IsNullOrEmpty(searchPattern))
                fields.Add("Contains", searchPattern);

            return TwilioClient.ParseAvailablePhoneNumber(Execute(url,fields));
        }

        /// <summary>
        /// Purchases a random phone number in the specified area code
        /// </summary>
        /// <param name="areaCode"></param>
        /// <param name="handlerUri"></param>
        /// <returns></returns>
        public AssignedPhoneNumber PurchasePhoneNumber(String areaCode, String handlerUri)
        {
            if (String.IsNullOrEmpty(areaCode))
                throw new ArgumentException("You must specify an area code when purchasing a new number");

            String url = String.Format("/{0}/Accounts/{1}/IncomingPhoneNumbers", API_VERSION, this.accountSid);

            Hashtable fields = new Hashtable();

            fields.Add("AreaCode", areaCode);
            fields.Add("VoiceUrl", handlerUri.ToString());
            fields.Add("VoiceCallerIdLookup", "true");

            return ParseAssignedPhoneNumber(Execute(url, fields, POST));
        }

        /// <summary>
        /// Attempts to purchase the specified phone number
        /// </summary>
        /// <param name="number"></param>
        /// <param name="handlerUri"></param>
        /// <returns></returns>
        public AssignedPhoneNumber PurchasePhoneNumber(AvailablePhoneNumber number, String handlerUri)
        {
            if (number == null)
                throw new ArgumentException("You must specify a phone number when purchasing a new number.");

            String url = String.Format("/{0}/Accounts/{1}/IncomingPhoneNumbers", API_VERSION, this.accountSid);

            Hashtable fields = new Hashtable();
            fields.Add("PhoneNumber", number.PhoneNumber);
            fields.Add("VoiceUrl", handlerUri.ToString());
            fields.Add("VoiceCallerIdLookup", "true");

            return ParseAssignedPhoneNumber(Execute(url, fields, POST));
        }

        public void ReleasePhoneNumber(String phoneSid, AvailablePhoneNumber availablePhoneNumber)
        {
            String url = String.Format("/{0}/Accounts/{1}/IncomingPhoneNumbers/{2}", API_VERSION, this.accountSid, phoneSid);
            String result = Execute(url, null, "DELETE");
        }

        private static XmlDocument GetXmlDocument(String text)
        {
            XmlDocument xml = new XmlDocument();
            xml.Load(new StringReader(text));

            return xml;
        }

        private static AssignedPhoneNumber ParseAssignedPhoneNumber(String text)
        {
            XmlDocument xml = GetXmlDocument(text);

            AssignedPhoneNumber result = new AssignedPhoneNumber();
            XmlNode response = xml.SelectSingleNode("//IncomingPhoneNumber");
            result.Sid = response.SelectSingleNode("Sid").InnerText;
            result.AccountId = response.SelectSingleNode("AccountSid").InnerText;
            result.FriendlyName = response.SelectSingleNode("FriendlyName").InnerText;
            result.PhoneNumber = response.SelectSingleNode("PhoneNumber").InnerText;
            result.VoiceUrl = response.SelectSingleNode("VoiceUrl").InnerText;
            result.InfoUri = response.SelectSingleNode("Uri").InnerText;

            return result;
        }

        private static List<AvailablePhoneNumber> ParseAvailablePhoneNumber(String text)
        {
            List<AvailablePhoneNumber> results = new List<AvailablePhoneNumber>();

            XmlDocument xml = GetXmlDocument(text);
            XmlNodeList list = xml.SelectNodes("//AvailablePhoneNumber");
            foreach (XmlNode node in list)
            {
                String name = node.SelectSingleNode("FriendlyName").InnerText;
                String number = node.SelectSingleNode("PhoneNumber").InnerText;

                AvailablePhoneNumber item = new AvailablePhoneNumber();
                item.FriendlyName = name;
                item.PhoneNumber = number;

                results.Add(item);
            }
            results.Sort((d, n) => d.PhoneNumber.CompareTo(n.PhoneNumber));

            return results;
        }

        private String Execute(String url, Hashtable fields, String method = "GET")
        {
            if (fields == null)
                fields = new Hashtable();

            String text;
            if ((fields != null) && (fields.Count > 0))
                text = this.client.request(url, method, fields);
            else
                text = this.client.request(url, method);
            return text;
        }
    }
}
