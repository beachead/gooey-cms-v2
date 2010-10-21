using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using gooeycms.business.salesforce.Salesforce;
using System.Web.Services.Protocols;
using System.Reflection;

namespace gooeycms.business.salesforce
{
    public enum LoginStatus
    {
        Success,
        InvalidLogin
    }

    /// <summary>
    /// Basic Salesforce.com Web Service Client.
    /// 
    /// This client exposes strongly-typed functionality for callers to interface with
    /// the salesforce.com webservice. The WSDL is generated per-client and therefore
    /// each new client must download their custom WSDL from salesforce.com and generate
    /// the necessary objects used to interface to the WSDL.
    /// 
    /// The code here has been left specifically generic in the hopes that it will support
    /// any WSDL that salesforce.com will generate for any client.
    /// </summary>
    public class SalesforceClient
    {
        private SforceService binding = null;
        LoginResult result = null;

        /// <summary>
        /// Login to the salesforce webservice using the specified credentials.
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        public void Login(String username, String password)
        {
            LoginStatus status = new LoginStatus();
            String errorMessage = "";


            this.binding = new SforceService();
            try
            {
                this.result = binding.login(username, password);
            }
            catch (SoapException e)
            {
                try
                {
                    ExceptionCode error = (ExceptionCode)Enum.Parse(typeof(ExceptionCode), e.Code.Name);
                    switch (error)
                    {
                        case ExceptionCode.INVALID_LOGIN:
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
        /// Adds a new lead into the lead system.
        /// 
        /// This method takes a dictionary of key/value pairs where the key
        /// must match a property on the Lead object. 
        /// 
        /// This allows the adding of leads to be completely decoupled from the actual implementation
        /// and allows users to only use the fields they find necessary without having to have
        /// a ton of null checks all over the place.
        /// </summary>
        public void AddLead(Dictionary<String, String> values)
        {
            if (this.result == null)
                throw new ApplicationException("The client has not logged into the salesforce service or is no longer connected.");

            sObject[] container = new sObject[1];

            Lead lead = new Lead();
            SetFields<Lead>(lead, values);

            //Verify the required fields are there, if not, supply default values
            if (String.IsNullOrEmpty(lead.FirstName))
                lead.FirstName = "NotSupplied";
            if (String.IsNullOrEmpty(lead.LastName))
                lead.LastName = "NotSupplied";
            if (String.IsNullOrEmpty(lead.Company))
                lead.Company = "NotSupplied";

            container[0] = lead;
            SaveResult[] results = binding.create(container);
        }

        public static IList<String> GetAvailableFields()
        {
            List<String> results = new List<String>();

            Type leadType = typeof(Lead);
            PropertyInfo[] properties = leadType.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                if ((property.PropertyType == typeof(String)) ||
                     (property.PropertyType == typeof(Nullable<bool>)))
                {
                    if (!property.Name.EndsWith("Specified"))
                        results.Add(property.Name);
                }
            }

            results.Sort();

            return results;
        }

        /// <summary>
        /// Sets the fields on the object based upon a dictionary of key/values
        /// </summary>
        /// <param name="fields"></param>
        private void SetFields<T>(T item, Dictionary<String, String> fields)
        {
            Type type = item.GetType();
            foreach (String name in fields.Keys)
            {
                try
                {
                    PropertyInfo info = type.GetProperty(name);
                    if (info != null)
                    {
                        if (info.PropertyType == typeof(Nullable<Boolean>))
                        {
                            PropertyInfo isSetInfo = type.GetProperty(name + "Specified");

                            Boolean result = false;
                            String temp = fields[name];
                            if (!String.IsNullOrEmpty(temp))
                                result = true;

                            if (result)
                            {
                                info.SetValue(item, result, null);

                                if (isSetInfo != null)
                                {
                                    isSetInfo.SetValue(item, true, null);
                                }
                            }
                        }
                        else if (info.PropertyType == typeof(String))
                        {
                            info.SetValue(item, fields[name], null);
                        }
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
