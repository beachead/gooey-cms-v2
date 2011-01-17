using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Campaign
{
    public class CmsCampaignDao : BaseDao
    {
        public CmsCampaign FindByTrackingCodeAndSite(Data.Guid guid, String trackingCode)
        {
            String hql = "select campaign from CmsCampaign campaign where campaign.SubscriptionId = :guid and campaign.TrackingCode = :code";
            return base.NewHqlQuery(hql).SetString("guid", guid.Value).SetString("code", trackingCode).UniqueResult<CmsCampaign>();
        }

        public IList<CmsCampaign> FindBySite(Guid siteGuid)
        {
            String hql = "select campaigns from CmsCampaign campaigns where campaigns.SubscriptionId = :guid order by campaigns.Name asc";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).List<CmsCampaign>();
        }

        public CmsCampaign FindBySiteAndGuid(Guid siteGuid, Guid campaignGuid)
        {
            String hql = "select campaign from CmsCampaign campaign where campaign.SubscriptionId = :siteGuid and campaign.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("siteGuid",siteGuid.Value).SetString("guid", campaignGuid.Value).UniqueResult<CmsCampaign>();
        }

        public CmsCampaign FindByPhoneNumber(string phone)
        {
            String hql = "select campaign from CmsCampaign campaign where campaign.PhoneNumber = :phone";
            return base.NewHqlQuery(hql).SetString("phone", phone).UniqueResult<CmsCampaign>();
        }
    }
}
