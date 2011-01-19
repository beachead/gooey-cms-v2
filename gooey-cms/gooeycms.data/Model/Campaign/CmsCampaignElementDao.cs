using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Campaign
{
    public class CmsCampaignElementDao : BaseDao
    {
        public CmsCampaignElement FindByElementGuid(Guid elementGuid)
        {
            String hql = "select element from CmsCampaignElement element where element.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("guid", elementGuid.Value).UniqueResult<CmsCampaignElement>();
        }

        public IList<CmsCampaignElement> FindByCampaignGuid(Guid campaignGuid)
        {
            String hql = "select element from CmsCampaignElement element where element.Campaign.Guid = :guid order by element.Priority desc";
            return base.NewHqlQuery(hql).SetString("guid", campaignGuid.Value).List<CmsCampaignElement>();
        }

        public IList<CmsCampaignElement> FindByPageUrl(string path)
        {
            String hql = "select element from CmsCampaignElement element where element._Pages like '%' + :path + '" + CmsCampaignElement.ElementSeparator + "%'";
            return base.NewHqlQuery(hql).SetString("path", path).List<CmsCampaignElement>();
        }
    }
}
