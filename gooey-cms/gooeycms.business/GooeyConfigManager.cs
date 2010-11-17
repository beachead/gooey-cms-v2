using System;
using Gooeycms.Constants;
using Gooeycms.Data.Model;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Collections.Generic;
using Gooeycms.Business.Util;
using Gooeycms.Extensions;
using Beachead.Persistence.Hibernate;

namespace Gooeycms.Business
{
    public static class GooeyConfigManager
    {
        private static Dictionary<String, object> cache = new Dictionary<string, object>();

        private static string GetCachedValue(String key, String defaultValue = null, Boolean required = false)
        {
            String result = (String)cache.GetValue<String, Object>(key);
            if (result == null)
            {
                result = GetAsString(key);
                if (result == null)
                {
                    if ((defaultValue == null) && (required))
                        throw new ArgumentException("The configuration value for " + key + " has not been specified and is a required field.");

                    result = defaultValue;
                    cache.Add(key, result);
                }
                cache[key] = result;
            }
            return result;
        }

        public static Nullable<Double> GetAsDouble(String key)
        {
            String temp = GetCachedValue(key);

            Nullable<Double> result = null;
            if (temp != null)
            {
                double val = Double.MinValue;
                Double.TryParse(temp, out val);

                if (val != Double.MinValue)
                {
                    result = new Nullable<Double>(val);
                }
            }

            return result;
        }

        public static Boolean GetAsBoolean(String key, Boolean defaultValue)
        {
            Boolean result = defaultValue;
            String temp = GetCachedValue(key);
            Boolean.TryParse(temp, out result);

            return result;
        }

        public static String GetAsString(String key)
        {
            ConfigurationDao dao = new ConfigurationDao();
            Configuration config = dao.FindByKey(key);

            String result = null;
            if (config != null)
                result = config.Value;

            return result;
        }

        public static void Persist(String key, String value)
        {
            ConfigurationDao dao = new ConfigurationDao();

            Configuration config = dao.FindByKey(key);
            if (config == null)
            {
                config = new Configuration();
                config.Name = key;
            }
            config.Value = value;

            using (Transaction tx = new Transaction())
            {
                dao.Save(config);
                tx.Commit();
            }
        }

        public static String DefaultTemplate
        {
            get
            {
                String defaultTemplate =
@"
    <div>
        {header}
    </div>
    <div>
        {content}
    </div>
    <div>
        {footer}
    </div>";
                return GetCachedValue(ConfigConstants.DefaultTemplate,defaultTemplate);
            }
        }

        public static String DefaultHomepage
        {
            get
            {
                String defaultHomepage = 
@"
## Welcome to gooey cms. 

This is your home page.
";
                return GetCachedValue(ConfigConstants.DefaultHomepage,defaultHomepage);
            }
        }

        public static String DefaultCulture
        {
            get { return "en-us"; }
        }

        public static Double CampaignOptionPrice
        {
            get
            {
                Nullable<Double> result = GooeyConfigManager.GetAsDouble(ConfigConstants.CampaignOptionPrice);
                if (!result.HasValue)
                    throw new ApplicationException("The sales force price has not been configured in the configuration table (key=" + ConfigConstants.SalesForcePrice + "). This is a required field for the application to function properly.");

                return result.Value;
            }
        }

        public static Double SalesForcePrice
        {
            get
            {
                Nullable<Double> result = GooeyConfigManager.GetAsDouble(ConfigConstants.SalesForcePrice);
                if (!result.HasValue)
                    throw new ApplicationException("The sales force price has not been configured in the configuration table (key=" + ConfigConstants.SalesForcePrice + "). This is a required field for the application to function properly.");

                return result.Value;
            }
        }

        public static String DefaultCmsDomain
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.DefaultCmsDomain);
                if (String.IsNullOrEmpty(result))
                    throw new ApplicationException("The default cms domain has not been configured in the configuration table (key=" + ConfigConstants.DefaultCmsDomain + "). This is a required field for the application to function properly.");
                if (!result.StartsWith("."))
                    throw new ApplicationException("The deafult domain must start with a period. The current value is " + result + ", but should be '." + result + "')");

                return result;
            }
        }

        public static String SmtpServer
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.SmtpServer);
                return result;
            }
        }

        public static String SmtpPort
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.SmtpPort,"2525");
                return result;
            }
        }

        public static String PaypalPdtToken
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.PaypalPdt, "3X14M2n_ysG7k2c5AK0OHj_vf7RvBWTEkkinHwBZL7UccC65sD4IL0pBScK");
                return result;
            }
        }

        public static String PaypalPostUrl
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.PaypalUrl, "https://www.sandbox.paypal.com/cgi-bin/webscr");
                return result;
            }
        }

        public static Boolean IsPaypalSandbox
        {
            get { return (PaypalPostUrl.Contains("sandbox")); }
        }

        public static String AdminSiteHost
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.PaypalUrl, "control.gooeycms.net");
                return result;
            }
        }

        public static Type SubscriptionProcessorClassType
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.SubscriptionProcessor);
                if (String.IsNullOrEmpty(result))
                    throw new ApplicationException("The configuration processor has not been configured in the configuration table (key=" + ConfigConstants.SubscriptionProcessor + "). This is a required field for the application to function properly.");

                Type type = Type.GetType(result);
                if (type == null)
                    throw new ApplicationException("Could not determine the class type for the subscription processor, " + result + ". Please check the value in the configuration table and verify that it is correct.");

                return type;
            }
        }

        public static Boolean IsLoggingEnabled
        {
            get
            {
                return (RoleEnvironment.IsAvailable);
            }
        }

        public static Boolean IsDevelopmentEnvironment
        {
            get { return !(RoleEnvironment.IsAvailable); }
        }

        public static String DefaultPageName
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.DefaultPageName,"default.aspx");
                return result;
            }
        }

        public static String DefaultStagingPrefix
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.DefaultStagingPrefix, "staging-");
                return result;
            }
        }

        public static String TokenEncyrptionKey
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.TokenEncyrptionKey, "token135adfjasdfk#GAGDJ a asfl;jasdf$%WT%WEGJKAFD");
                return result;
            }
        }

        public static String DefaultSystemFormFields
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.DefaultSystemFormFields, "submit-email,downloadfile,gooeykeycheck,submit,campaign,gooey-filename");
                return result;
            }
        }

        public static IList<String> StorePackageCategories
        {
            get
            {
                IList<String> categories = new List<String>();

                String result = GetCachedValue(ConfigConstants.StorePackageCategories);
                if (result != null)
                    categories = result.SplitAsList(TextConstants.CommaSeparator);

                return categories;
            }
        }

        public static String InviteEmailTemplate
        {
            get
            {
                String result = GetCachedValue(ConfigConstants.InviteEmailTemplate);
                return result;
            }
            set
            {
                Persist(ConfigConstants.InviteEmailTemplate, value);
                cache[ConfigConstants.InviteEmailTemplate] = value;
            }
        }

        public static String ApprovedEmailTemplate
        {
            get
            {
                return GetCachedValue(ConfigConstants.ApprovedEmailTemplate);
            }
            set
            {
                Persist(ConfigConstants.ApprovedEmailTemplate, value);
                cache[ConfigConstants.ApprovedEmailTemplate] = value;
            }
        }

        public static class EmailAddresses
        {
            public static String SiteAdmin
            {
                get
                {
                    return GetCachedValue(ConfigConstants.EmailAddressesSiteAdmin, required: true);
                }
            }
        }
    }
}
