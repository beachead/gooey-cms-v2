using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;
using System.Web;
using Gooeycms.Data.Model.Subscription;
using Gooeycms.Data.Model.Campaign;
using Gooeycms.Data.Model;
using Gooeycms.Business.Crypto;
using Gooeycms.Business.Util;

namespace Gooeycms.Business.Twilio.Handlers
{
    public abstract class BaseTwilioHandler : BaseHttpHandler
    {
        private HttpRequest request;
        private CmsSubscriptionPhoneNumber associatedNumber = null;
        private CmsCampaign associatedCampaign = null;

        protected abstract void WriteInstructions(HttpContext context, StringBuilder instructions);

        protected override void Process(System.Web.HttpContext context)
        {
            this.request = context.Request;

            //Force the "CurrentSite" helper to the active subscription for this phone number
            SiteHelper.SetActiveSiteCookie(GetAssociatedPhoneNumber().SubscriptionId);

            StringBuilder builder = new StringBuilder();
            builder.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            builder.AppendLine("<Response>");
            this.WriteInstructions(context, builder);
            builder.AppendLine("</Response>");

            byte[] data = Encoding.UTF8.GetBytes(builder.ToString());

            context.Response.Clear();
            context.Response.ClearHeaders();
            context.Response.ClearContent();
            context.Response.ContentType = "text/xml";
            context.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            context.Response.OutputStream.Write(data, 0, data.Length);
            context.Response.End();
        }

        protected String CallFromNumber
        {
            get { return Get(this.request["From"]); }
        }

        protected String CallFromCity
        {
            get { return Get(this.request["FromCity"]); }
        }

        protected String CallFromState
        {
            get { return Get(this.request["FromState"]); }
        }

        protected String CallFromZip
        {
            get { return Get(this.request["FromZip"]); }
        }

        protected String CallName
        {
            get { return Get(this.request["CallerName"]); }
        }

        protected String CallTo
        {
            get { return Get(this.request["To"]); }
        }

        protected CmsSubscriptionPhoneNumber GetAssociatedPhoneNumber()
        {
            if (this.associatedNumber == null)
            {
                CmsSubscriptionPhoneDao dao = new CmsSubscriptionPhoneDao();
                this.associatedNumber = dao.FindByPhoneNumber(this.CallTo);
            }
            return this.associatedNumber;
        }

        protected CmsCampaign GetAssociatedCampaign()
        {
            if (this.associatedCampaign == null)
            {
                CmsCampaignDao dao = new CmsCampaignDao();
                this.associatedCampaign = dao.FindByPhoneNumber(this.CallTo);
            }

            return this.associatedCampaign;
        }

        protected String GetSiteConfigurationValue(String key)
        {
            SiteConfigurationDao dao = new SiteConfigurationDao();
            SiteConfiguration config = dao.FindByKey(GetAssociatedCampaign().SubscriptionId, key);
            
            String result = null;
            if (config != null)
                result = config.Value;

            return result;
        }

        protected String GetEncryptedConfigurationValue(String key)
        {
            String result = null;
            String encrypted = GetSiteConfigurationValue(key);
            if (encrypted != null)
            {
                try
                {
                    TextEncryption crypto = new TextEncryption(this.GetAssociatedPhoneNumber().SubscriptionId);
                    result = crypto.Decrypt(encrypted);
                }
                catch (Exception) { }
            }

            return result;
        }

        private String Get(String temp)
        {
            String result = temp;
            if (String.IsNullOrWhiteSpace(result))
                result = "";

            return result;
        }
    }
}
