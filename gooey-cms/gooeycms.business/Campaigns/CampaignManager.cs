using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gooeycms.Data.Model.Campaign;
using Beachead.Persistence.Hibernate;
using Gooeycms.Business.Util;
using Gooeycms.Business.Web;
using System.Web;

namespace Gooeycms.Business.Campaigns
{
    public class CampaignManager
    {
        private const char CampaignCookieSeparator = ',';

        private static CampaignManager instance = new CampaignManager();
        public CampaignManager() { }
        public static CampaignManager Instance { get { return CampaignManager.instance; } }

        public void Add(CmsCampaign campaign)
        {
            if (campaign.SubscriptionId == null)
                throw new ApplicationException("The subscription id for this campaign has not been set.");

            
            //Make sure this tracking code hasn't been used
            CmsCampaign check = GetByTrackingCode(campaign.SubscriptionId, campaign.TrackingCode);
            if (check != null)
                throw new ArgumentException("The tracking code: " + campaign.TrackingCode + " has already been associated with camapaign: " + check.Name + " and may not be used again.");

            campaign.Guid = System.Guid.NewGuid().ToString();
            CmsCampaignDao dao = new CmsCampaignDao();
            using (Transaction tx = new Transaction())
            {
                dao.Save<CmsCampaign>(campaign);
                tx.Commit();
            }
        }

        public CmsCampaign GetByTrackingCode(Data.Guid siteGuid, String trackingCode)
        {
            CmsCampaignDao dao = new CmsCampaignDao();
            return dao.FindByTrackingCodeAndSite(siteGuid, trackingCode);
        }

        public CmsCampaign GetCampaign(Data.Guid campaignGuid)
        {
            return GetCampaign(CurrentSite.Guid, campaignGuid);
        }

        public CmsCampaign GetCampaign(Data.Guid siteGuid, Data.Guid campaignGuid)
        {
            CmsCampaignDao dao = new CmsCampaignDao();
            return dao.FindBySiteAndGuid(siteGuid,campaignGuid);
        }

        public IList<CmsCampaign> GetCampaigns()
        {
            return GetCampaigns(CurrentSite.Guid);
        }

        public IList<CmsCampaign> GetCampaigns(Data.Guid siteGuid)
        {
            CmsCampaignDao dao = new CmsCampaignDao();
            return dao.FindBySite(siteGuid);
        }

        /// <summary>
        /// Returns the campaign engine that is being used by the CMS
        /// Currently only one is supported, Google Analytics
        /// </summary>
        /// <returns></returns>
        internal ICampaignEngine GetCampaignEngine()
        {
            return CurrentSite.GetCampaignEngine();
        }

        /// <summary>
        /// Gets the active campaign trail and combines it into a single string
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public HashSet<String> GetCurrentCampaignTrail()
        {
            HashSet<String> keys = new HashSet<string>();

            HttpCookie cookie = WebRequestContext.Instance.Request.Cookies["campaign"];
            if (cookie != null)
            {
                String[] temp = cookie.Value.Split(CampaignCookieSeparator);
                foreach (String item in temp)
                    keys.Add(item);
            }

            return keys;
        }

        /// <summary>
        /// Combines the campaigns into a single string
        /// </summary>
        /// <param name="campaigns"></param>
        /// <returns></returns>
        public static String Combine(HashSet<String> campaigns, String separator)
        {
            StringBuilder builder = new StringBuilder();
            if (campaigns.Count > 0)
            {
                foreach (String campaign in campaigns)
                {
                    if (!String.IsNullOrWhiteSpace(campaign))
                        builder.Append(campaign.Trim()).Append(separator);
                }
                if (builder.Length > 0)
                    builder = builder.Remove(builder.Length - 1, 1);
            }

            return builder.ToString();
        }

        /// <summary>
        /// Tracks the campaign that may have brought the user to this page
        /// </summary>
        public void TrackCampaigns()
        {
            String activeCampaign = GetCampaignEngine().GetActiveUserCampaign();
            if (!String.IsNullOrWhiteSpace(activeCampaign))
            {
                HashSet<String> campaigns = this.GetCurrentCampaignTrail();
                campaigns.Add(activeCampaign);

                String campaignString = Combine(campaigns,",");
                if (!String.IsNullOrWhiteSpace(campaignString))
                {
                    HttpCookie cookie = new HttpCookie("campaign", campaignString);
                    cookie.Expires = DateTime.Now.AddMonths(1);
                    WebRequestContext.Instance.CurrentHttpContext.Response.Cookies.Add(cookie);
                }
            }
        }

        /// <summary>
        /// Generates a tracking link for the specified campaign id
        /// </summary>
        /// <param name="campaign"></param>
        /// <returns></returns>
        public String GenerateTrackingLink(Data.Guid guid, String source, String medium, String landingPage, Boolean isFileType)
        {
            CmsCampaign campaign = this.GetCampaign(guid);
            if (campaign == null)
                throw new ArgumentException("Could not find the campaign with a campaign id of " + guid.Value);

            return this.GetCampaignEngine().GetTrackingLink(campaign, source, medium, landingPage, isFileType);
        }

        /// <summary>
        /// Converts a path-based campaign string into a querystring-based campaign string
        /// </summary>
        /// <param name="campaignString"></param>
        /// <returns></returns>
        public String ConvertTrackingLink(string campaignString)
        {
            return this.GetCampaignEngine().ConvertTrackingLink(campaignString);
        }

        public string GetCurrentCampaignTrailAsString()
        {
            HashSet<String> campaigns = GetCurrentCampaignTrail();
            return Combine(campaigns, "|");
        }
    }
}
