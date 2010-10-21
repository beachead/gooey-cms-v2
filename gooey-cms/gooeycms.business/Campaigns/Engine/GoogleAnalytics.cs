using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Business.Web;
using Gooeycms.Business.Util;
using Microsoft.Security.Application;
using Gooeycms.Data.Model.Campaign;

namespace Gooeycms.Business.Campaigns.Engine
{
    public class GoogleAnalytics : ICampaignEngine
    {
        private static String TrackingLinkFormat = @"utm_source={0}&utm_medium={1}&utm_campaign={2}";
        private static String TrackingLinkFileFormat = "/utm_source/{0}/utm_medium/{1}/utm_campaign/{2}/{3}";
        private static String TrackingSnippet = @"
<script type=""text/javascript"">
  var _gaq = _gaq || [];
  _gaq.push(['_setAccount', '{google-account-id}']);
  _gaq.push(['_trackPageview']);

  (function() {
    var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
    ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
    var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
  })();
</script>";

        /// <summary>
        /// Determines if tracking is enabled or not
        /// </summary>
        /// <returns></returns>
        public Boolean IsEnabled
        {
            get
            {
                return ((CurrentSite.Configuration.IsGoogleAnalyticsEnabled) && (!String.IsNullOrWhiteSpace(CurrentSite.Configuration.GoogleAccountId)));
            }
        }

        /// <summary>
        /// Retrieves the tracking script which needs to be embedded into the page
        /// </summary>
        /// <returns></returns>
        public String GetTrackingScript()
        {
            String accountId = CurrentSite.Configuration.GoogleAccountId.Trim();
            String result = "";
            if (!IsEnabled)
                result = "";
            else 
                result = TrackingSnippet.Replace("{google-account-id}", accountId);

            return result;
        }

        /// <summary>
        /// Generates the tracking link in a format that can be used by Google Analytics
        /// </summary>
        /// <param name="campaign"></param>
        /// <param name="source"></param>
        /// <param name="linkType"></param>
        /// <param name="landingPage"></param>
        /// <returns></returns>
        public string GetTrackingLink(CmsCampaign campaign, string source, string linkType, string landingPage, Boolean isFileType)
        {
            String temp;
            String name = campaign.TrackingCode;
            String format;

            if (!isFileType)
            {
                format = String.Format(TrackingLinkFormat, AntiXss.UrlEncode(source), AntiXss.UrlEncode(linkType), AntiXss.UrlEncode(name));
                temp = CurrentSite.ToAbsoluteUrl(landingPage);
                if (temp.Contains("?"))
                    temp = temp + "&";
                else
                    temp = temp + "?";
            }
            else
            {
                format = String.Format(TrackingLinkFileFormat, AntiXss.UrlEncode(source), AntiXss.UrlEncode(linkType), AntiXss.UrlEncode(name), AntiXss.UrlEncode(landingPage));
                temp = CurrentSite.ToAbsoluteUrl("~/gooeyfiles");
            }

            temp = temp + format;

            return temp;
        }

        public String ConvertTrackingLink(String raw)
        {
            StringBuilder result = new StringBuilder();
            Dictionary<String, String> lookup = new Dictionary<string, string>();
            String[] temp = raw.Split('/');
            if (temp.Length >= 6)
            {
                for (int i = 0; i < 6; i++)
                {
                    String key = temp[i];
                    String value = temp[++i];

                    lookup.Add(key, value);
                }

                foreach (String key in lookup.Keys)
                {
                    result.Append(key).Append("=").Append(AntiXss.UrlEncode(lookup[key])).Append("&");
                }
            }

            return result.ToString();
        }

        /// <summary>
        /// Determines from the querystring whic campaign is currently being invoked, if any.
        /// </summary>
        /// <returns></returns>
        public String GetActiveUserCampaign()
        {
            WebRequestContext context = new WebRequestContext();
            String result = context.CurrentHttpContext.Request["utm_campaign"];

            return result;
        }

        /// <summary>
        /// Returns the goal page is for use in the querystring to signify a goal page has 
        /// been reached by the client.
        /// </summary>
        /// <returns></returns>
        public String GetGoalPageId()
        {
            return "utm_gp";
        }
    }
}
