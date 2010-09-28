using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Handler;
using Gooeycms.Business.Forms.Plugins;
using Gooeycms.Business.Crypto;
using System.Web;
using Gooeycms.Business.Campaigns;

namespace Gooeycms.Business.Forms
{
    public class FormProcessingHandler : BaseHttpHandler
    {
        private HttpRequest request;

        protected override void Process(System.Web.HttpContext context)
        {
            Exception exception = null;
            String redirect = null;

            this.request = context.Request;
            Dictionary<String, String> fields = this.Normalize();

            String postingResource = fields["resource"];
            if (fields.ContainsKey("redirect"))
                redirect = fields["redirect"];
            else
                redirect = postingResource;

            String encryptedFilename = fields["gooey-filename"];
            String decryptedFilename = "";
            if (!String.IsNullOrWhiteSpace(encryptedFilename))
                decryptedFilename = TextEncryption.Decode(encryptedFilename);
            fields.Add("filename", decryptedFilename);

            String campaignTrail = CampaignManager.Instance.GetCurrentCampaignTrailAsString();
            fields.Add("campaign", campaignTrail);

            IList<IFormPlugin> plugins = FormPluginFactory.Instance.GetPlugins();
            foreach (IFormPlugin plugin in plugins)
            {
                if (plugin.IsEnabled())
                {
                    try
                    {
                        plugin.FormFields = fields;
                        plugin.Process();
                    }
                    catch (Exception e)
                    {
                        if (plugin.IsExceptionFatal())
                        {
                            exception = e;
                            break;
                        }
                    }
                }
            }

            String goalPageIdentifier = CampaignManager.Instance.GetCampaignEngine().GetGoalPageId();
            String msg = "";
            if (exception != null)
                msg = "There was a problem processing this request: " + exception.Message;

            String append = (redirect.Contains("?")) ? "&" : "?";
            redirect = redirect + append + "msg=" + context.Server.UrlEncode(msg) + "&" + goalPageIdentifier + "=true";

            //check if we need to download a file
            if (!String.IsNullOrEmpty(encryptedFilename))
                redirect = redirect + "&d=" + context.Server.UrlEncode(encryptedFilename);

            //Redirect to the appropriate page
            context.Response.Redirect(redirect, true);       
        }

        public Dictionary<String, String> Normalize()
        {
            Dictionary<String, String> results = new Dictionary<string, string>();
            foreach (String key in this.request.Form.Keys)
            {
                String guid = key.Substring(0, key.IndexOf("_") + 1);
                String newkey = key.Replace(guid, "");

                String value = this.request.Form[key];
                results.Add(newkey, value);
            }

            return results;
        }
    }
}
