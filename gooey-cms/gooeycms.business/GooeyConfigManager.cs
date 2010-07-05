﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model;
using gooeycms.constants;

namespace Gooeycms.Business
{
    public static class GooeyConfigManager
    {
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
                String result = GooeyConfigManager.GetAsString(ConfigConstants.DefaultCmsDomain);
                if (String.IsNullOrEmpty(result))
                    throw new ApplicationException("The default cms domain has not been configured in the configuration table (key=" + ConfigConstants.DefaultCmsDomain + "). This is a required field for the application to function properly.");
                if (!result.StartsWith(".")) 
                    throw new ApplicationException("The deafult domain must start with a period. The current value is " + result + ", but should be '." + result + "')");

                return result;
            }
        }
    }
}
