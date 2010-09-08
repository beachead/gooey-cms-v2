using System;
using Gooeycms.Constants;
using Gooeycms.Data.Model;
using Microsoft.WindowsAzure.ServiceRuntime;
using System.Collections.Generic;
using Gooeycms.Business.Util;

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

        public static String AdminSiteHost
        {
            get
            {
                return "control.gooeycms.net";
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
                    result = "submit-email,downloadfile,gooeykeycheck,submit,campaign";

                return result;
            }
        }
    }
}
