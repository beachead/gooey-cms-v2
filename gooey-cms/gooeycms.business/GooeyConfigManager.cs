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


        public static Nullable<Double> GetAsDouble(String key)
        {
            String temp = GetAsString(key);

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
            String temp = GetAsString(key);
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
                String result = (String)cache.GetValue<String, Object>(ConfigConstants.DefaultTemplate);
                if (result == null)
                {
                    result = GooeyConfigManager.GetAsString(ConfigConstants.DefaultTemplate);
                    if (result == null)
                    {
                        result =
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
                        cache.Add(ConfigConstants.DefaultTemplate, result);
                    }
                }
                return result;
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
                String result = (String)cache.GetValue<String, Object>(ConfigConstants.DefaultCmsDomain);
                if (result == null)
                {
                    result = GooeyConfigManager.GetAsString(ConfigConstants.DefaultCmsDomain);
                    if (String.IsNullOrEmpty(result))
                        throw new ApplicationException("The default cms domain has not been configured in the configuration table (key=" + ConfigConstants.DefaultCmsDomain + "). This is a required field for the application to function properly.");
                    if (!result.StartsWith("."))
                        throw new ApplicationException("The deafult domain must start with a period. The current value is " + result + ", but should be '." + result + "')");

                    cache.Add(ConfigConstants.DefaultCmsDomain, result);
                }
                return result;
            }
        }

        public static String SmtpServer
        {
            get
            {
                String result = (String)cache.GetValue<String, Object>(ConfigConstants.SmtpServer);
                if (result == null)
                {
                    result = GooeyConfigManager.GetAsString(ConfigConstants.SmtpServer);
                    if (result == null)
                        throw new ApplicationException("An SMTP server has not been configured in the gooey site configuration. This is a required setting. (key=" + ConfigConstants.SmtpServer);

                    cache.Add(ConfigConstants.SmtpServer, result);
                }
                return result;
            }
        }

        public static String SmtpPort
        {
            get
            {
                String result = (String)cache.GetValue<String, Object>(ConfigConstants.SmtpPort);
                if (result == null)
                {
                    result = GooeyConfigManager.GetAsString(ConfigConstants.SmtpPort);
                    if (result == null)
                        result = "2525";

                    cache.Add(ConfigConstants.SmtpPort, result);
                }
                return result;
            }
        }

        public static String PaypalPdtToken
        {
            get
            {
                String result = (String)cache.GetValue<String,Object>(ConfigConstants.PaypalPdt);
                if (result == null)
                {
                    result = GooeyConfigManager.GetAsString(ConfigConstants.PaypalPdt);
                    if (String.IsNullOrEmpty(result))
                        result = "3X14M2n_ysG7k2c5AK0OHj_vf7RvBWTEkkinHwBZL7UccC65sD4IL0pBScK";
                    cache.Add(ConfigConstants.PaypalPdt, result);
                }

                return result;
            }
        }

        public static String PaypalPostUrl
        {
            get
            {
                String result = (String)cache.GetValue<String,Object>(ConfigConstants.PaypalUrl);
                if (result == null)
                {
                    result = GooeyConfigManager.GetAsString(ConfigConstants.PaypalUrl);
                    if (String.IsNullOrEmpty(result))
                        result = "https://www.sandbox.paypal.com/cgi-bin/webscr";
                    cache.Add(ConfigConstants.PaypalUrl, result);
                }

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
                String result = (String)cache.GetValue<String, Object>(ConfigConstants.GooeyAdminUrl);
                if (result == null)
                {
                    result = GooeyConfigManager.GetAsString(ConfigConstants.GooeyAdminUrl);
                    if (String.IsNullOrEmpty(result))
                        result = "control.gooeycms.net";
                    cache.Add(ConfigConstants.GooeyAdminUrl, result);
                }

                return result;
            }
        }

        public static Type SubscriptionProcessorClassType
        {
            get
            {
                String result = GooeyConfigManager.GetAsString(ConfigConstants.SubscriptionProcessor);
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

        public static String DefaultPageName
        {
            get
            {
                String result = GetAsString(ConfigConstants.DefaultPageName);
                if (String.IsNullOrEmpty(result))
                    result = "default.aspx";

                return result;
            }
        }

        public static String DefaultStagingPrefix
        {
            get
            {
                String result = GetAsString(ConfigConstants.DefaultStagingPrefix);
                if (String.IsNullOrEmpty(result))
                    result = "staging-";

                return result;
            }
        }

        public static String TokenEncyrptionKey
        {
            get
            {
                String result = GetAsString(ConfigConstants.TokenEncyrptionKey);
                if (String.IsNullOrEmpty(result))
                    result = "token135adfjasdfk#GAGDJ a asfl;jasdf$%WT%WEGJKAFD";

                return result;
            }
        }

        public static String DefaultSystemFormFields
        {
            get
            {
                String result = GetAsString(ConfigConstants.DefaultSystemFormFields);
                if (String.IsNullOrEmpty(result))
                    result = "submit-email,downloadfile,gooeykeycheck,submit,campaign,gooey-filename";

                return result;
            }
        }

        public static IList<String> StorePackageCategories
        {
            get
            {
                IList<String> categories = new List<String>();

                String result = GetAsString(ConfigConstants.StorePackageCategories);
                if (result != null)
                    categories = result.SplitAsList(TextConstants.CommaSeparator);

                return categories;
            }
        }

        public static String InviteEmailTemplate
        {
            get
            {
                String result = (String)cache.GetValue<String, Object>(ConfigConstants.InviteEmailTemplate);
                if (result == null)
                {
                    result = GetAsString(ConfigConstants.InviteEmailTemplate);
                    cache[ConfigConstants.InviteEmailTemplate] = result;
                }
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
                String result = (String)cache.GetValue<String, Object>(ConfigConstants.ApprovedEmailTemplate);
                if (result == null)
                {
                    result = GetAsString(ConfigConstants.ApprovedEmailTemplate);
                    cache[ConfigConstants.ApprovedEmailTemplate] = result;
                }
                return result;
            }
            set
            {
                Persist(ConfigConstants.ApprovedEmailTemplate, value);
                cache[ConfigConstants.ApprovedEmailTemplate] = value;
            }
        }
    }
}
