using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Gooeycms.Data.Model.Form
{
    public class CmsSavedFormDao : BaseDao
    {
        public CmsSavedForm FindBySiteAndName(Data.Guid siteGuid, string name)
        {
            String hql = "select form from CmsSavedForm form where form.SubscriptionId = :guid and form.Name = :name";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetString("name", name).UniqueResult<CmsSavedForm>();
        }

        public CmsSavedForm FindBySiteAndGuid(Guid siteGuid, Guid guid)
        {
            String hql = "select form from CmsSavedForm form where form.SubscriptionId = :siteGuid and form.Guid = :guid";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid.Value).SetString("guid", guid.Value).UniqueResult<CmsSavedForm>();
        }

        public IList<CmsSavedForm> FindBySite(Guid siteGuid)
        {
            String hql = "select forms from CmsSavedForm forms where forms.SubscriptionId = :siteGuid";
            return base.NewHqlQuery(hql).SetString("siteGuid", siteGuid.Value).List<CmsSavedForm>();
        }
    }
}
