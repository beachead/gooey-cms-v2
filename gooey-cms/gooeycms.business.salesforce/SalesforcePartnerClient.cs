using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Services.Protocols;
using gooeycms.business.salesforce.SalesforcePartner;

namespace gooeycms.business.salesforce
{
    public class SalesforcePartnerClient
    {
        public class LeadField : IComparable<LeadField>
        {
            public String Label { get; set; }
            public String ApiName { get; set; }
            public Boolean IsRequired { get; set; }

            public int CompareTo(LeadField other)
            {
                return (this.ApiName.CompareTo(other.ApiName));
            }
        }

        private SalesforcePartner.SforceService binding = null;
        SalesforcePartner.LoginResult result = null;

        /// <summary>
        /// Login to the salesforce webservice using the specified credentials.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(String username, String password)
        {
            LoginStatus status = new LoginStatus();
            String errorMessage = "";


            this.binding = new SalesforcePartner.SforceService();
            try
            {
                this.result = binding.login(username, password);
            }
            catch (SoapException e)
            {
                try
                {
                    Salesforce.ExceptionCode error = (Salesforce.ExceptionCode)Enum.Parse(typeof(Salesforce.ExceptionCode), e.Code.Name);
                    switch (error)
                    {
                        case Salesforce.ExceptionCode.INVALID_LOGIN:
                            status = LoginStatus.InvalidLogin;
                            errorMessage = "Invalid username or password specified.\r\nEnsure that your password is in the form of mypasswordXXXXXX where XXXXXX is your security token.";
                            break;
                    }
                }
                catch (Exception)
                {
                    throw new ApplicationException("An unexpected exception has occurred and could not be handled: " + e.Code.ToString());
                }
            }

            if (status != LoginStatus.Success)
                throw new LoginException(errorMessage, status);

            //reset the bindings to the ones returned from the login so that the session will stay open
            //and also use the correct endpoint to service the requests.
            binding.SessionHeaderValue = new SessionHeader();
            binding.SessionHeaderValue.sessionId = this.result.sessionId;
            binding.Url = this.result.serverUrl;
        }


        /// <summary>
        /// Logs out of the salesforce network
        /// </summary>
        public void Logout()
        {
            if (this.result != null)
            {
                try
                {
                    this.binding.logout();
                }
                catch (Exception) { }
            }
        }

        /// <summary>
        /// Gets the lead fields that are available for this account
        /// </summary>
        /// <returns></returns>
        public IList<LeadField> GetAvailableLeadFields()
        {
            DescribeSObjectResult result = this.binding.describeSObject("lead");
            Field[] fields = result.fields;

            List<LeadField> fieldnames = new List<LeadField>();
            if (fields != null)
            {
                foreach (Field field in fields)
                {
                    fieldType type = field.type;
                    if ((type == fieldType.@string) ||
                        (type == fieldType.boolean) ||
                        (type == fieldType.email) ||
                        (type == fieldType.url) ||
                        (type == fieldType.textarea) ||
                        (type == fieldType.phone))
                    {
                        if (field.createable)
                        {
                            LeadField item = new LeadField();
                            item.Label = field.label;
                            item.ApiName = field.name;
                            item.IsRequired = ((!field.nillable) && (!field.defaultedOnCreate));

                            fieldnames.Add(item);
                        }
                    }
                }
            }

            fieldnames.Sort();
            return fieldnames;
        }

        public void AddLead(Dictionary<String, String> values)
        {
            if (this.result == null)
                throw new ApplicationException("The client is not logged into salesforce");

            sObject[] leads = new sObject[1];

            sObject lead = new sObject();

            IList<LeadField> temp = GetAvailableLeadFields();
            Dictionary<String, LeadField> validFields = new Dictionary<String, LeadField>(StringComparer.InvariantCultureIgnoreCase);
            foreach (LeadField item in temp)
                validFields[item.ApiName] = item;
            
            IList<System.Xml.XmlElement> elements = new List<System.Xml.XmlElement>();
            Dictionary<String, Boolean> associatedFields = new Dictionary<string, bool>(StringComparer.InvariantCultureIgnoreCase);
            foreach (String key in values.Keys)
            {
                if (validFields.ContainsKey(key))
                {
                    elements.Add(GetNewXmlElement(validFields[key].ApiName, values[key]));
                    associatedFields[validFields[key].ApiName] = true;
                }
            }

            //Make sure all the required fields have actually been populated
            foreach (LeadField field in temp)
            {
                if (field.IsRequired)
                {
                    if (!associatedFields.ContainsKey(field.ApiName))
                    {
                        elements.Add(GetNewXmlElement(field.ApiName, "NotSupplied"));
                    }
                }
            }

            lead.Any = elements.ToArray<System.Xml.XmlElement>();
            lead.type = "Lead";
            leads[0] = lead;

            SaveResult[] results = binding.create(leads);
            if (results.Length > 0)
            {
                SaveResult result = results[0];
                if (!result.success)
                {
                    throw new ArgumentException("There was a problem saving the lead to Salesforce");
                }
            }
        }

        private System.Xml.XmlElement GetNewXmlElement(string Name, string nodeValue)
        {
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            System.Xml.XmlElement xmlel = doc.CreateElement(Name);
            xmlel.InnerText = nodeValue;
            return xmlel;
        }
    }
}
