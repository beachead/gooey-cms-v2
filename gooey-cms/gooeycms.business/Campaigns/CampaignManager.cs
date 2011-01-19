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
        private Lazy<ElementsManager> elementManager = new Lazy<ElementsManager>();

        public ElementsManager Elements
        {
            get { return elementManager.Value; }
        }

        public class ElementsManager
        {
            public void Save(CmsCampaignElement element)
            {
                CmsCampaignElementDao dao = new CmsCampaignElementDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Save<CmsCampaignElement>(element);
                    tx.Commit();
                }
            }

            public CmsCampaignElement GetElement(Data.Guid elementGuid, Data.Guid expectedCampaignGuid = new Data.Guid())
            {
                CmsCampaignElementDao dao = new CmsCampaignElementDao();
                CmsCampaignElement element = dao.FindByElementGuid(elementGuid);
                if ((element != null) && (!expectedCampaignGuid.IsEmpty()))
                {
                    if (!element.Campaign.Guid.Equals(expectedCampaignGuid.Value))
                        throw new ArgumentException("There was a problem retrieving the campaign element: Element belongs to campaign " + element.Campaign.Guid + ", but attempted retrieval for " + expectedCampaignGuid.Value);
                }

                return element;
            }

            public IList<CmsCampaignElement> GetElements(Data.Guid campaignGuid)
            {
                CmsCampaignElementDao dao = new CmsCampaignElementDao();
                return dao.FindByCampaignGuid(campaignGuid);
            }

            /// <summary>
            /// Gets all of the campaign elements for the specified page
            /// </summary>
            /// <param name="cmsPage"></param>
            /// <returns></returns>
            public IDictionary<String,String> GetElementsForPage(Data.Model.Page.CmsPage cmsPage)
            {
                CmsCampaignElementDao dao = new CmsCampaignElementDao();
                IList<CmsCampaignElement> elements = dao.FindByPageUrl(cmsPage.Url);

                Dictionary<String, IList<CmsCampaignElement>> groupings = new Dictionary<string, IList<CmsCampaignElement>>();
                groupings["top"] = new List<CmsCampaignElement>();
                groupings["middle"] = new List<CmsCampaignElement>();
                groupings["bottom"] = new List<CmsCampaignElement>();

                foreach (CmsCampaignElement element in elements)
                    groupings[element.Placement].Add(element);

                Dictionary<String, String> results = new Dictionary<string, string>();

                //Loop through each "group" and build up the elements
                foreach (String key in groupings.Keys)
                {
                    StringBuilder builder = new StringBuilder();

                    IList<CmsCampaignElement> temp = groupings[key];
                    foreach (CmsCampaignElement item in temp)
                    {
                        builder.Append(@"<div class=""gooey-campaign-element"">").AppendLine();
                        builder.AppendLine(item.Content);
                        builder.AppendFormat("</div>");
                    }

                    results[key] = builder.ToString();
                }

                return results;
            }

            public void Delete(CmsCampaignElement element)
            {
                if (element != null)
                {
                    CmsCampaignElementDao dao = new CmsCampaignElementDao();
                    using (Transaction tx = new Transaction())
                    {
                        dao.Delete<CmsCampaignElement>(element);
                        tx.Commit();
                    }
                }
            }
        }


        public void Add(CmsCampaign campaign)
        {
            if (campaign.SubscriptionId == null)
                throw new ApplicationException("The subscription id for this campaign has not been set.");

            if (campaign.Guid == null)
            {
                //Make sure this tracking code hasn't been used
                CmsCampaign check = GetByTrackingCode(campaign.SubscriptionId, campaign.TrackingCode);
                if (check != null)
                    throw new ArgumentException("The tracking code: " + campaign.TrackingCode + " has already been associated with camapaign: " + check.Name + " and may not be used again.");

                campaign.Guid = System.Guid.NewGuid().ToString();
            }

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

        public void Delete(Data.Guid siteGuid, Data.Guid guid)
        {
            CmsCampaign campaign = GetCampaign(siteGuid, guid);
            if (campaign != null)
            {
                CmsCampaignDao dao = new CmsCampaignDao();
                using (Transaction tx = new Transaction())
                {
                    dao.Delete<CmsCampaign>(campaign);
                    tx.Commit();
                }
            }
        }

        public void Delete(Data.Guid guid)
        {
            Delete(CurrentSite.Guid, guid);
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

        /// <summary>
        /// Gets the latest campaign that is active for the current user.
        /// </summary>
        /// <returns></returns>
        public CmsCampaign GetLastActiveCampaign()
        {
            CmsCampaign campaign = null;

            IList<String> campaigns = new List<String>(GetCurrentCampaignTrail());
            if (campaigns.Count > 0)
            {
                String last = campaigns[campaigns.Count - 1];
                campaign = this.GetByTrackingCode(CurrentSite.Guid, last);
            }

            return campaign;
        }

        public string GetActivePhoneNumber()
        {
            String result = "";

            CmsCampaign campaign = GetLastActiveCampaign();

            if (campaign != null)
                result = campaign.PhoneNumber;

            String format = CurrentSite.Configuration.PhoneSettings.DefaultPhoneFormat;
            if (String.IsNullOrWhiteSpace(result))
                result = CurrentSite.Configuration.PhoneSettings.DefaultForwardNumber;

            if (!String.IsNullOrWhiteSpace(result))
            {
                Twilio.AvailablePhoneNumber number = Twilio.AvailablePhoneNumber.Parse(result);
                result = String.Format(format, number.AreaCode, number.Exchange, number.LineNumber);
            }

            return result;
        }
    }
}
