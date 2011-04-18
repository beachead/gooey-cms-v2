using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;
using Gooeycms.Extensions;

namespace Gooeycms.Business.Forms.Plugins
{
    public class LeadEmailPlugin : FormPlugin
    {
        public override bool IsEnabled()
        {
            return CurrentSite.Configuration.IsLeadEmailEnabled; 
        }

        public override void Process()
        {
            String email = base.GetField("submit-email");
            String fromAddress = CurrentSite.Configuration.LeadEmailFromAddress;
            String subject = CurrentSite.Configuration.LeadEmailSubject;

            if ((!String.IsNullOrEmpty(email)) && (!String.IsNullOrEmpty(fromAddress)))
            {
                CommonInfo common = base.ParseCommonInfo();

                StringBuilder builder = new StringBuilder();
                builder.Append("Form Response").AppendLine();
                builder.AppendFormat("Resource: {0}", common.Resource).AppendLine();
                builder.AppendFormat("Culture:   {0}", common.Culture).AppendLine();
                builder.AppendFormat("Date: {0}", UtcDateTime.Now).AppendLine();
                builder.AppendFormat("IP Address:   {0}", common.IpAddress).AppendLine();
                builder.AppendFormat("Email:   {0}", common.Email).AppendLine();
                builder.AppendFormat("Campaign:   {0}", common.Campaigns).AppendLine();
                builder.AppendFormat("Download:   {0}", common.Filename).AppendLine(); 

                foreach (String key in base.FormFields.Keys)
                {
                    if ((base.IsValidField(key)) && 
                        (!key.EqualsCaseInsensitive("resource")) && 
                        (!key.EqualsCaseInsensitive("culture")) &&
                        (!key.EqualsCaseInsensitive("filename")))
                        builder.AppendFormat("{0}:  {1}", key, base.GetField(key)).AppendLine();
                }
                String message = builder.ToString().Trim();

                EmailClient client = EmailClient.GetDefaultClient();
                client.ToAddress = email;
                client.FromAddress = fromAddress;
                client.IsHtmlContent = false;
                client.Send(subject, message);
            }
        }
    }
}
