using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate;
using System.Collections;

namespace Gooeycms.Data.Model.Form
{
    public class CmsFormDao : BaseDao
    {
        public IList<String> FindUniqueForms(Data.Guid siteGuid, DateTime startdate, DateTime enddate)
        {
            String hql = "select distinct form.FormUrl from CmsForm form where form.SubscriptionId = :guid and form.Inserted between :start and :end order by form.FormUrl asc";
            return base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetDateTime("start",startdate).SetDateTime("end",enddate).List<String>();
        }

        public IList<CmsForm> FindUniqueResponses(Guid siteGuid, DateTime startdate, DateTime enddate, IList<String> filterPages)
        {
            String hql = "select form from CmsForm form where form.SubscriptionId = :guid and form.Inserted between :start and :end ";
            if ((filterPages != null) && (filterPages.Count > 0))
                hql = hql + "and form.FormUrl in (:pages) ";
            hql = hql + "order by form.Inserted desc";

            IQuery query = base.NewHqlQuery(hql).SetString("guid", siteGuid.Value).SetDateTime("start", startdate).SetDateTime("end", enddate);

            if ((filterPages != null) && (filterPages.Count > 0))
                query.SetParameterList("pages", (IList)filterPages);

            return query.List<CmsForm>();
        }
    }
}
