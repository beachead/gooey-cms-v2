using System;
using Gooeycms.Constants;
using Gooeycms.Data.Model;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Collections.Generic;
using Gooeycms.Business.Util;
using Gooeycms.Extensions;
using Beachead.Persistence.Hibernate;
using System.Reflection;

namespace Gooeycms.Business
{
    public static class GooeyConfigManager
    {
        private static String DefaultSitePackageTos = @"I certify that I am the creator of this website, and agree that Gooey CMS and its users may use, copy, display and store this website on other Gooey CMS powered websites, according to Gooey CMS's <a href=""#"" onclick=""window.open('/tos.aspx#site','','width=500,height=450'); return false;"">Terms of Service</a>.";
        private static String DefaultThemePackageTos = @"I certify that I am the creator of this theme, and agree that Gooey CMS and its users may use, copy, display and store this theme on other Gooey CMS powered websites, according to Gooey CMS's <a href=""#"" onclick=""window.open('/tos.aspx#themes','','width=500,height=450'); return false;"">Terms of Service</a>.";
        private static String DefaultFlashCrossDomainFile =
@"<?xml version=""1.0"" ?>
<cross-domain-policy>
  <site-control permitted-cross-domain-policies=""master-only""/>
  <allow-access-from domain=""*""/>
  <allow-http-request-headers-from domain=""*"" headers=""*""/>
</cross-domain-policy>";

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
                    cache[key] = result;
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

        public static String GetValueOrDefault(String key)
        {
            String result = GetAsString(key);
            if (String.IsNullOrWhiteSpace(result))
            {
                result = GetCachedValue(key);
            }
            return result;
        }

        public static String GetValueByReflection(String propertyName)
        {
            String result = "";
            PropertyInfo info = typeof(GooeyConfigManager).GetProperty(propertyName, BindingFlags.Static | BindingFlags.FlattenHierarchy | BindingFlags.Public);
            if (info != null)
            {
                Object temp = info.GetValue(null, null);
                if (temp != null)
                    result = temp.ToString();
            }
            return result;
        }

        public static void SetValueAndUpdateCache(String configKey, String value)
        {
            Persist(configKey, value);
            cache[configKey] = value;
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

        public static String DefaultTemplateName
        {
            get { return GetCachedValue(ConfigConstants.DefaultTemplateName,"Gooey Default Page Template"); }
        }

        public static String DefaultThemeName
        {
            get { return GetCachedValue(ConfigConstants.DefaultThemeName, "Gooey Default Theme"); }
        }

        public static String DefaultThemeDescription
        {
            get { return GetCachedValue(ConfigConstants.DefaultThemeDescription, "A bare-bones default theme"); }
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
                    throw new ApplicationException("The campaign option price has not been configured in the configuration table (key=" + ConfigConstants.SalesForcePrice + "). This is a required field for the application to function properly.");

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

        public static String PaypalUsername
        {
            get { return GetCachedValue(ConfigConstants.PaypalUsername); }
        }

        public static String PaypalPassword
        {
            get { return GetCachedValue(ConfigConstants.PaypalPassword); }
        }

        public static String PaypalSignature
        {
            get { return GetCachedValue(ConfigConstants.PaypalSignature); }
        }

        public static String PaypalApiVersion
        {
            get { return GetCachedValue(ConfigConstants.PaypalApiVersion, defaultValue: "54.0"); }
        }

        public static String PaypalReturnUrl
        {
            get { return GetCachedValue(ConfigConstants.PaypalReturnUrl, defaultValue: SignupSiteHost + "/activate.aspx"); }
        }

        public static String PaypalCancelUrl
        {
            get { return GetCachedValue(ConfigConstants.PaypalCancelUrl, defaultValue: SignupSiteHost + "/cancel.aspx"); }
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
                String result = GetCachedValue(ConfigConstants.PaypalUrl, ConfigConstants.PaypalSandboxUrl);
                return result;
            }
            set
            {
                Persist(ConfigConstants.PaypalUrl, value);
                cache[ConfigConstants.PaypalUrl] = value;
            }
        }

        public static Int32 PaypalMaxFailedPayments
        {
            get
            {
                return Int32.Parse(GetCachedValue(ConfigConstants.PaypalMaxFailedPayments, defaultValue: "2"));
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
                String result = GetCachedValue(ConfigConstants.DefaultAdminDomain, "control.gooeycms.net");
                return result;
            }
        }

        public static String StoreSiteHost
        {
            get
            {
                return GetCachedValue(ConfigConstants.DefaultSiteDomain, "http://store.gooeycms.com");
            }
        }

        public static String SignupSiteHost
        {
            get
            {
                return GetCachedValue(ConfigConstants.DefaultSignupDomain, "http://secure.gooeycms.com/signup");
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
                List<String> categories = new List<String>();

                String result = GetCachedValue(ConfigConstants.StorePackageCategories);
                if (result != null)
                    categories = new List<String>(result.SplitAsList(TextConstants.CommaSeparator));

                categories.Sort();
                return categories;
            }
        }

        public static void AddStorePackageCategory(String category)
        {
            Boolean valid = true;
            IList<String> items = StorePackageCategories;
            foreach (String item in items)
            {
                if (item.ToLower().Equals(category.ToLower()))
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                items.Add(category);
                String result = items.AsString<String>(TextConstants.CommaSeparator.ToString());

                Persist(ConfigConstants.StorePackageCategories, result);
                cache[ConfigConstants.StorePackageCategories] = result;
            }
        }

        public static void RemoveStorePackageCategory(String category)
        {
            IList<String> items = StorePackageCategories;
            Boolean removed = items.Remove(category);
            if (removed)
            {
                String result = items.AsString<String>(TextConstants.CommaSeparator.ToString());

                Persist(ConfigConstants.StorePackageCategories, result);
                cache[ConfigConstants.StorePackageCategories] = result;
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

        public static String SitePackageTos
        {
            get
            {
                return GetCachedValue(ConfigConstants.SitePackageTos, defaultValue: DefaultSitePackageTos);
            }
        }

        public static Int32 DefaultAsyncTimeout
        {
            get
            {
                return Int32.Parse(GetCachedValue(ConfigConstants.DefaultAsyncTimeout,defaultValue: "600"));
            }
        }

        public static String FlashCrossDomainFile
        {
            get
            {
                return GetCachedValue(ConfigConstants.DefaultFlashCrossDomainFile, DefaultFlashCrossDomainFile);
            }
        }

        public static Int32 FreeTrialLength
        {
            get
            {
                return Int32.Parse(GetCachedValue(ConfigConstants.FreeTrialLength,defaultValue: "30"));
            }
        }

        public static String SiteDisabledRedirect
        {
            get
            {
                return GetCachedValue(ConfigConstants.SiteDisabledRedirect, defaultValue: "http://www.gooeycms.com");
            }
        }

        public static String StaticResourceUrl
        {
            get
            {
                return GetCachedValue(ConfigConstants.StaticResourceUrl, defaultValue: "http://www.gooeycms.com");
            }
        }

        public static Boolean IsInviteEnabled
        {
            get
            {
                return Boolean.Parse(GetCachedValue(ConfigConstants.InviteEnabled, defaultValue: "true"));
            }
        }

        public static String TwilioAccountSid
        {
            get { return GetCachedValue(ConfigConstants.TwilioAccountSid); }
        }

        public static String TwilioAccountToken
        {
            get { return GetCachedValue(ConfigConstants.TwilioAccountToken); }
        }

        public static String TwilioPhoneHandlerUrl
        {
            get { return "http://" + GooeyConfigManager.AdminSiteHost + "/twilio/forward-init.mxl"; }
        }

        public static Int32 CampaignMaxPhoneNumbers
        {
            get { return Int32.Parse(GetCachedValue(ConfigConstants.CampaignMaxPhoneNumbers, defaultValue: "5")); }
        }

        public static String GetEmailTemplate(String templateType)
        {
            return GetCachedValue("email-template-" + templateType);
        }

        public static void SetEmailTemplate(String templateType, String content)
        {
            SetValueAndUpdateCache("email-template-" + templateType, content);
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
