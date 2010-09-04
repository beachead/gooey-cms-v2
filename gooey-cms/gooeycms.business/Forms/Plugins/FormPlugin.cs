using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Business.Forms.Plugins
{
    public abstract class FormPlugin : IFormPlugin
    {
        private Dictionary<String, String> fields = null;

        public struct CommonInfo
        {
            public String Culture;
            public String Resource;
            public String Email;
            public String IpAddress;
            public String Campaigns;
        }

        #region IFormPlugin Members

        public abstract bool IsEnabled();
        public abstract void Process();

        /// <summary>
        /// By default exceptions are not fatal to the entire posting process
        /// </summary>
        /// <returns></returns>
        public virtual Boolean IsExceptionFatal()
        {
            return false;
        }

        public Dictionary<String, String> FormFields
        {
            set { this.fields = value; }
            get { return this.fields; }
        }

        /// <summary>
        /// Gets the specified field or returns null if it doesn't exist.
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        protected String GetField(String key)
        {
            String result = null;
            if (fields.ContainsKey(key))
                result = fields[key].Trim();

            return result;
        }

        /// <summary>
        /// Checks if the specified key is a valid form field.
        /// </summary>
        /// <param name="key"></param>
        protected Boolean IsValidField(String key)
        {
            return FormManager.IsValidField(key);
        }

        /// <summary>
        /// Utility method that all of the plugins can use to parse the common info
        /// from the dictionary
        /// </summary>
        /// <param name="fields"></param>
        /// <returns></returns>
        protected CommonInfo ParseCommonInfo()
        {
            CommonInfo info = new CommonInfo();
            info.Campaigns = this.GetField("campaign");
            info.Culture = this.GetField("culture");
            info.Resource = this.GetField("resource");
            info.Email = this.GetField("email");
            if (info.Email == null)
                info.Email = this.GetField("Email");

            Web.WebRequestContext context = new Web.WebRequestContext();
            info.IpAddress = context.Request.UserHostAddress;

            return info;
        }

        #endregion
    }
}
